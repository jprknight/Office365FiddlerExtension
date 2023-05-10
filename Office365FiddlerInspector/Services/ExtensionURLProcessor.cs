using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerInspector.Services
{
    internal class ExtensionURLProcessor
    {
        public async void CheckExtensionURLs()
        {
            // If disable web calls is set, don't look for any URL updates.
            if (Preferences.DisableWebCalls)
            {
                GetSetSessionFlags.Instance.WriteToFiddlerLogNoSession($"DisableWebCalls is enabled, no extension URLs update check performed.");
                return;
            }

            // If using the beta rule set use that last updated to determine if we check for updates or not.
            if (Preferences.BetaRuleSet)
            {
                if (DateTime.Now < Properties.Settings.Default.LocalMasterRulesetLastUpdated)
                {
                    GetSetSessionFlags.Instance.WriteToFiddlerLogNoSession($"Rules have been checked within the last 24 hours, no extension URLs update check performed.");
                    return;
                }
            }
            // If using the master rule set use that last updated to determine if we check for updates or not.
            else
            {
                if (DateTime.Now < Properties.Settings.Default.LocalBetaRulesetLastUpdated)
                {
                    GetSetSessionFlags.Instance.WriteToFiddlerLogNoSession($"Rules have been checked within the last 24 hours, no extension URLs update check performed.");
                    return;
                }
            }

            // If we got past those checks, check for any updates to the extension URLS.
            using (var versionCheck = new HttpClient())
            {
                try
                {
                    var response = await versionCheck.GetAsync("https://somedummyurlwhichwontwork");
                    // If we're running the beta ruleset, look to the Fiddler application preference for the URL to go to for the rulesetVersion file.
                    // This will likely be a rolling URL based on the branch name used at any time.
                    if (Preferences.BetaRuleSet)
                    {
                        response = await versionCheck.GetAsync(Properties.Settings.Default.BetaRuleSetURL);
                    }
                    // Here we're not using the beta ruleset, so pull it from the master branch.
                    else
                    {
                        response = await versionCheck.GetAsync(Properties.Settings.Default.MasterRuleSetURL);
                    }

                    //var response = await versionCheck.GetAsync("https://raw.githubusercontent.com/username/repo/master/file.json");
                    response.EnsureSuccessStatusCode();

                    var jsonString = string.Empty;

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        //Rules = await JsonSerializer.DeserializeAsync<Dictionary<string, Rule>>(stream);

                        stream.Position = 0;
                        using (var reader = new StreamReader(stream))
                        {
                            jsonString = await reader.ReadToEndAsync();
                        }

                        // jsonString came back as empty.
                        if (jsonString == null)
                        {
                            GetSetSessionFlags.Instance.WriteToFiddlerLogNoSession($"Error retrieving ruleset from Github: jsonString null.");
                        }
                        // jsonString has something in it. See if the version value on Github is newer than what we have stored locally.
                        else
                        {
                            // There's a newer ruleset published the the Github repo.
                            if (int.Parse(jsonString) >= int.Parse(Properties.Settings.Default.LocalMasterRulesetLastUpdated))
                            {
                                GetSetSessionFlags.Instance.WriteToFiddlerLogNoSession($"Local ruleset is behind Github ruleset.");

                                // Call a function here which does:
                                // * Pulls all rule files, decodes them and looks for differences, if differences are found store in local file overwriting any existing content.
                                // * Or checks each rule from something different and if different store in local file overwriting any existing content.
                                // * Assuming all the above finished without issue, bump the ruleset version number in Settings.
                            }
                            // There's not a newer ruleset published to the Github repo.
                            else
                            {
                                GetSetSessionFlags.Instance.WriteToFiddlerLogNoSession($"Local ruleset is up to date with Github ruleset.");
                                return;
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    GetSetSessionFlags.Instance.WriteToFiddlerLogNoSession($"Error retrieving ruleset from Github {ex}");
                }
            }




            var ExtensionSessionFlags = JsonConvert.DeserializeObject<SessionFlagProcessor.ExtensionSessionFlags>(this.session["Microsoft365FiddlerExtensionJson"]);


        }
    }
}
