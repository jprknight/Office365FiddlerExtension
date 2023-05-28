using Fiddler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtension.Services
{
    public class SettingsHandler
    {
        public async void GetSettingsFromGithub()
        {
            // If disable web calls is set, don't look for any URL updates.
            if (Preferences.DisableWebCalls)
            {
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: DisableWebCalls is enabled, not checking for updates for settings.json.");
                return;
            }

            if (Properties.Settings.Default._SettingsJsonLastUpdated <  DateTime.Now.AddHours(24)) 
            {
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: Updates for settings.json checked within 24 hours, no update check performed.");
                return;
            }

            Boolean exception = false;
            // Connect to the Github and see if the settings.json file needs updating.
            using (var getSettings = new HttpClient())
            {
                // Pull the Json from _SettingsJsona.
                var SettingsJson = JsonConvert.DeserializeObject<SettingsJson>(Properties.Settings.Default._SettingsJson);

                // Connect to the SettingsJson URL and check for any differences. If there are download them and update into the local _SettingsJson setting.
                try
                {
                    var response = await getSettings.GetAsync(SettingsJson.SettingsURL);

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

                        if (Properties.Settings.Default._SettingsJson != jsonString)
                        {
                            Properties.Settings.Default._SettingsJson = jsonString;
                        }
                    }
                }
                catch (Exception ex)
                {
                    exception = true;
                    FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: Error retrieving settings from Github {ex}");
                }

                // If there is no exception here, update the settings last checked.
                if (!exception)
                {
                    var localSettingsJson = JsonConvert.DeserializeObject<SettingsJson>(Properties.Settings.Default._SettingsJson);
                    FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: Local setting _SettingsJson updated from Github.");
                    localSettingsJson.SettingsLastUpdated = DateTime.Now;
                }
            }
        }

        public class SettingsJson
        {
            public String SettingsURL { get; set; }

            public DateTime SettingsLastUpdated { get; set; }
        }

    }

    public class ExtensionSettingsFlags
    {
        public string SettingsURL { get; set; }

        public string WikiURL { get; set; }

        public string ReportIssuesURL { get; set; }

        public string BetaRuleSetURL { get; set; }

        public string MasterRuleSetURL { get; set; }

        public string UpdateMessage { get; set; }

        public int WarningSessionTimeThreshold { get; set; }

        public int SlowRunningSessionThreshold { get; set; }

        public string ExtensionVersion { get; set; }

        public DateTime SettingsJsonLastUpdated { get; set; }
    }
}
