using Fiddler;
using Microsoft.CSharp;
using Office365FiddlerInspector.Properties;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Office365FiddlerInspector.Services
{
    internal class Program
    {
        private static readonly Lazy<RulesetSingleton> _instance = new Lazy<RulesetSingleton>(() => new RulesetSingleton());
        public static RulesetSingleton Ruleset => _instance.Value;

        static async Task Main(string[] args)
        {
            /*
            Settings file will have this already:
            eyJLQjIzMjkzODMiOnsiUnVsZVR5cGUiOjMsIlJ1bGVSZWdleCI6IiQqLlswLTldezR9Ki5eIiwiUnVsZVJlc3VsdCI6IlRoaXMgaXMgYSBidWcgeW8iLCJSdWxlQWN0aW9uIjozfSwiTmV3IEZlYXR1cmUgTVMzNDkzODQiOnsiUnVsZVR5cGUiOjAsIlJ1bGVSZWdleCI6IiQqLlthLXpBLVpdezh9Ki5eIiwiUnVsZVJlc3VsdCI6IlRoaXMgaXMgYSBuZXcgZmVhdHVyZSB5byIsIlJ1bGVBY3Rpb24iOjJ9fQ==

            Which decodes to:
            {
                "KB2329383": {
                    "RuleType": 3,
                    "RuleRegex": "$*.[0-9]{4}*.^",
                    "RuleResult": "This is a bug yo",
                    "RuleAction": 3
                },
                "New Feature MS349384": {
                    "RuleType": 0,
                    "RuleRegex": "$*.[a-zA-Z]{8}*.^",
                    "RuleResult": "This is a new feature yo",
                    "RuleAction": 2
                }
            }
            */

            // Example of how to programatically create rules and serialize into json:
            var sampleJson = CreateSampleRules();
            Console.WriteLine($"Sample json:{Environment.NewLine}{sampleJson}");

            // Pull rules from settings happens in the lazy initialize
            Console.WriteLine($"Current ruleset contains {Ruleset.Rules.Count} rules.");

            // Update settings from github
            Console.WriteLine("Updating rules from GitHub...");
            await Ruleset.UpdateRulesAsync();
            Console.WriteLine($"Updated ruleset contains {Ruleset.Rules.Count} rules.");
        }

        static string CreateBroadLogicChecksRules()
        {
            var Ruleset = new Dictionary<string, Rule>();




            return Ruleset;

        }

        static string CreateSampleRules()
        {
            var Ruleset = new Dictionary<string, Rule>();

            Ruleset.Add("KB2329383", new Rule()
            {
                RuleType = RuleType.Bug,
                RuleRegex = "$*.[0-9]{4}*.^",
                RuleResult = "This is a bug yo",
                RuleAction = RuleAction.Flag | RuleAction.Link | RuleAction.Note
            });

            Ruleset.Add("New Feature MS349384", new Rule()
            {
                RuleType = RuleType.Informational,
                RuleRegex = "$*.[a-zA-Z]{8}*.^",
                RuleResult = "This is a new feature yo",
                RuleAction = RuleAction.Note
            });

            // Serialize the ruleset to JSON
            return JsonSerializer.Serialize(Ruleset);
        }
    }

    public class RulesetSingleton
    {
        public Dictionary<string, Rule> Rules = new Dictionary<string, Rule>();

        internal RulesetSingleton()
        {
            // Pull default rules from settings
            var defaultRulesetBytes = Convert.FromBase64String(Properties.Settings.Default.DefaultRuleset);
            var defaultRulesetJson = Encoding.UTF8.GetString(defaultRulesetBytes);
            Rules = JsonSerializer.Deserialize<Dictionary<string, Rule>>(defaultRulesetJson);
        }

        public async Task UpdateRulesAsync()
        {
            // Shutdown the web calls before they begin if the preference is set.
            if (Preferences.DisableWebCalls)
            {
                GetSetSessionFlags.Instance.WriteToFiddlerLogNoSession("Web calls disabled. Not looking for ruleset updates.");
                return;
            }

            // REVIEW THIS.
            // Think about writing some code so ruleset checks are only done once per 24/48/72/168 hours.
            // Ruleset won't change that frequently, so there's probably no reason to call out to the Github repro on every Fiddler start.

            #region RulesetVersionCheck
            // Pull the version file to see if there is a version ruleset to update on.
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
                        Rules = await JsonSerializer.DeserializeAsync<Dictionary<string, Rule>>(stream);

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
                            if (int.Parse(jsonString) >= int.Parse(Properties.Settings.Default.RuleSetVersion))
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
            #endregion

            // Use the beta ruleset.
            if (Properties.Settings.Default.UseBetaRuleSet)
            {

            }
            // Use the master / normal / production ruleset.
            else
            {

            }

            using (var httpClient = new HttpClient())
            {
                try
                {
                    // You may want to post a separate version file so you can check if the version is newer first
                    // Alternatively if you can assume the file size is different you can pass HttpMethod.Head as the method
                    // and then pull the Content-Length from response.Headers and compare to the non-Base64 size

                    var response = await httpClient.GetAsync("https://raw.githubusercontent.com/username/repo/master/file.json");
                    response.EnsureSuccessStatusCode();

                    // You can do this two ways, but probably should do it the async way as your ruleset could get large
                    var async = true;
                    var jsonString = string.Empty;

                    // Synchronous way (not recommended but simpler, blocks thread, higher memory footprint while deserializing)
                    if (!async)
                    {
                        jsonString = await response.Content.ReadAsStringAsync();

                        // Deserialize JSON and overwrite the ruleset.  You may want to merge or do something else
                        Rules = JsonSerializer.Deserialize<Dictionary<string, Rule>>(jsonString);
                    }
                    else
                    {
                        // More complex, can probably be optimized but if you end up with a large ruleset
                        // you want to do the http get and the deserialization asynchronously
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {
                            Rules = await JsonSerializer.DeserializeAsync<Dictionary<string, Rule>>(stream);

                            // Get the string for storage
                            stream.Position = 0;
                            using (var reader = new StreamReader(stream))
                            {
                                jsonString = await reader.ReadToEndAsync();
                            }
                        }
                    }

                    // Save base64 version to settings as the new default
                    Properties.Settings.Default.DefaultRuleset = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonString));
                }
                catch (Exception ex)
                {
                    GetSetSessionFlags.Instance.WriteToFiddlerLogNoSession($"Error retrieving ruleset from Github {ex}");
                }
            }
        }

        public enum RuleType
        {
            Informational,
            Security,
            Performance,
            Bug,
            Unknown
        }
        public enum RuleAction
        {
            Ignore,
            Link,
            Note,
            Flag,
            WhoKnows
        }

        public class Rule
        {
            public RuleType RuleType { get; set; }
            public string RuleRegex { get; set; }
            public string RuleResult { get; set; }
            public RuleAction RuleAction { get; set; }
        }
    }
}