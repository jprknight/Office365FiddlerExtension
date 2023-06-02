using Fiddler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Office365FiddlerExtension.Services.SessionFlagHandler;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Handler for extension settings and URLs set in Fiddlers preferences.
    /// </summary>

    public class SettingsHandler
    {
        private static SettingsHandler _instance;
        public static SettingsHandler Instance => _instance ?? (_instance = new SettingsHandler());

        /// <summary>
        /// Create settings if they don't exist. 
        /// </summary>
        public void CreateExtensionSettingsJsonFiddlerSetting()
        {
            if (Preferences.ExtensionSettings == null || Preferences.ExtensionSettings == "")
            {
                Version applicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                var ExtensionSettings = new
                {
                    ExtensionEnabled = "True",
                    ExecutionCount = "0",
                    NeverWebCall = "False",
                    SessionAnalysisOnFiddlerLoad = "True",
                    SessionAnalysisOnLoadSaz = "True",
                    SessionAnalysisOnLiveTrace = "True",
                    UpdateMessage = "",
                    WarningSessionTimeThreshold = "2500",
                    SlowRunningSessionThreshold = "5000",
                    ExtensionDLL = Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8),
                    LastLoadedSazFile = "",
                    VersionMajor = applicationVersion.Major,
                    VersionMinor = applicationVersion.Minor,
                    VersionBuild = applicationVersion.Build,
                    NextUpdateCheck = "",
                    UseBetaRuleSet = "False",
                    UseInternalRuleset = "False",
                    LocalMasterRulesetLastUpdated = "",
                    LocalBetaRulesetLastUpdated = "",
                    SettingsJsonLastUpdated = ""
                };

                // Transform the object to a Json object.
                string jsonData = JsonConvert.SerializeObject(ExtensionSettings);

                // Save the new Json to the Fiddler setting.
                Preferences.ExtensionSettings = jsonData;
            }
        }

        public void CreateExtensionURLJsonFiddlerSetting()
        {
            // If the Extension URLs Json already exists, none of this needs to run.
            if (Preferences.ExtensionURLs != null || Preferences.ExtensionURLs == "")
            {
                return;
            }

            // REVIEW THIS. UpdateURL needs to move to master once it's a valid URL.

            var URLSettings = new
            {
                UpdateJson = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/Code-Hygiene/Office365FiddlerExtension/settings.json",
                MasterRuleSet = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/Master/RulesetVersion",
                BetaRuleSet = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/Code-Hygiene/RulesetVersion",
                Installer = "https://github.com/jprknight/EXOFiddlerExtension/releases/latest",
                Wiki = "https://github.com/jprknight/Office365FiddlerExtension/wiki",
                ReportIssues = "https://github.com/jprknight/Office365FiddlerExtension/issues"
            };

            // Transform the object to a Json object.
            string jsonData = JsonConvert.SerializeObject(URLSettings);

            // Save the new Json to the Fiddler setting.
            Preferences.ExtensionURLs = jsonData;
        }

        public void CreateExtensionVersionJsonFiddlerSetting()
        {
            // If the Extension Version Json already exists, none of this needs to run.
            if (Preferences.ExtensionVersion != null || Preferences.ExtensionVersion == "")
            {
                return;
            }

            // REVIEW THIS. UpdateURL needs to move to master once it's a valid URL.

            var VersionItems = new
            {
                UpdateMessage = "",
                VersionMajor = "",
                VersionMinor = "",
                VersionBuild = ""
            };

            // Transform the object to a Json object.
            string jsonData = JsonConvert.SerializeObject(VersionItems);

            // Save the new Json to the Fiddler setting.
            Preferences.ExtensionVersion = jsonData;
        }

        public async void UpdateExtensionURLsFromGithub()
        {
            // Function to try catch connecting to ExtensionSettings.json file from Github repo, pulling the content and writing it into the ExtensionSettings
            // Fiddler setting for recall as needed.
        }


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
