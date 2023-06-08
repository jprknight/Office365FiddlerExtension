using Fiddler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public void CreateExtensionSettingsFiddlerSetting()
        {
            if (Preferences.ExtensionSettings != null)
            {
                //MessageBox.Show("Extension settings not null.");
                return;
            }

            int upgradeExecutionCount;
            bool upgradeNeverWebCall;
            bool upgradeExtensionEnabled;

            if (Preferences.ExecutionCount > 0)
            {
                upgradeExecutionCount = Preferences.ExecutionCount;
            }
            else
            {
                upgradeExecutionCount = 0;
            }

            if (Preferences.NeverWebCall)
            {
                upgradeNeverWebCall = true;
            }
            else
            {
                upgradeNeverWebCall = false;
            }

            if (Preferences.ExtensionEnabled)
            {
                upgradeExtensionEnabled = true;
            }
            else
            {
                upgradeExtensionEnabled = false;
            }

            var ExtensionSettings = new
            {
                ExtensionEnabled = upgradeExtensionEnabled,
                ExecutionCount = upgradeExecutionCount,
                NeverWebCall = upgradeNeverWebCall,
                SessionAnalysisOnFiddlerLoad = "True",
                SessionAnalysisOnLoadSaz = "True",
                SessionAnalysisOnLiveTrace = "True",
                WarningSessionTimeThreshold = "2500",
                SlowRunningSessionThreshold = "5000",
                LastLoadedSazFile = "",
                UseBetaRuleSet = "False",
                UseHardCodedRuleset = "False",
                LocalMasterRulesetLastUpdated = "",
                LocalBetaRulesetLastUpdated = ""
            };

            // Transform the object to a Json object.
            string jsonData = JsonConvert.SerializeObject(ExtensionSettings);

            // Save the new Json to the Fiddler setting.
            Preferences.ExtensionSettings = jsonData;

            //FiddlerApplication.Prefs.RemovePref("Enabled");
            //FiddlerApplication.Prefs.RemovePref("ExecutionCount");
            //FiddlerApplication.Prefs.RemovePref("ManualCheckForUpdate");
            //FiddlerApplication.Prefs.RemovePref("NeverWebCall");
            //FiddlerApplication.Prefs.RemovePref("UpdateMessage");
        }

        public void CreateExtensionURLFiddlerSetting()
        {
            // If the Extension URLs Json already exists, none of this needs to run.
            if (Preferences.ExtensionURLs != null || Preferences.ExtensionURLs == "")
            {
                return;
            }

            // REVIEW THIS. URLs needs to move to master once it's a valid URL.

            var URLs = new
            {
                ExtensionVerisonJson = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/Code-Hygiene/Office365FiddlerExtension/ExtensionVersion.json",
                UpdateJson = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/Code-Hygiene/Office365FiddlerExtension/settings.json",
                MasterRuleSet = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/Master/RulesetVersion",
                BetaRuleSet = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/Code-Hygiene/RulesetVersion",
                Installer = "https://github.com/jprknight/EXOFiddlerExtension/releases/latest",
                Wiki = "https://github.com/jprknight/Office365FiddlerExtension/wiki",
                ReportIssues = "https://github.com/jprknight/Office365FiddlerExtension/issues"
            };

            // Transform the object to a Json object.
            string jsonData = JsonConvert.SerializeObject(URLs);

            // Save the new Json to the Fiddler setting.
            Preferences.ExtensionURLs = jsonData;
        }

        public void UpdateExtensionVersionFiddlerSetting()
        {
            Version applicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            var VersionItems = new
            {
                UpdateMessage = "test", // REVIEW THIS. Needs to be pulled from ExtensionVersion.json in Github.
                ExtensionDLL = Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8),
                VersionMajor = applicationVersion.Major,
                VersionMinor = applicationVersion.Minor,
                VersionBuild = applicationVersion.Build,
                RulesetLastUpdated = ""
            };

            // Transform the object to a Json object.
            string jsonData = JsonConvert.SerializeObject(VersionItems);

            // Save the new Json to the Fiddler setting.
            Preferences.ExtensionVersion = jsonData;
        }

        public bool ExtensionEnabled
        {
            get
            {
                var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
                return extensionSettings.ExtensionEnabled;
            }
        }


        public void SetExtensionEnabled(Boolean extensionEnabled)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.ExtensionEnabled = extensionEnabled;
            // Serialize the object back into Json.
            var extensionSettingsJson = JsonConvert.SerializeObject(extensionSettings);
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = extensionSettingsJson;

            // Set the Menu item to reflect change.
            MenuUI.Instance.ExtensionMenu.Text = ExtensionEnabled ? "Office 365 (Enabled)" : "Office 365 (Disabled)";
        }

        public bool SessionAnalysisOnFiddlerLoad
        {
            get
            {
                var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
                return extensionSettings.SessionAnalysisOnFiddlerLoad;
            }
        }

        public void SetSessionAnalysisOnFiddlerLoad(Boolean sessionAnalysisOnFiddlerLoad)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.SessionAnalysisOnFiddlerLoad = sessionAnalysisOnFiddlerLoad;
            // Serialize the object back into Json.
            var extensionSettingsJson = JsonConvert.SerializeObject(extensionSettings);
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = extensionSettingsJson;

            // Set the Menu item to reflect change.
            MenuUI.Instance.MiOnFiddlerLoad.Checked = sessionAnalysisOnFiddlerLoad;
        }

        public bool SessionAnalysisOnLoadSaz
        {
            get
            {
                var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
                return extensionSettings.SessionAnalysisOnLoadSaz;
            }
        }

        public void SetSessionAnalysisOnLoadSaz(Boolean sessionAnalysisOnLoadSaz)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.SessionAnalysisOnLoadSaz = sessionAnalysisOnLoadSaz;
            // Serialize the object back into Json.
            var extensionSettingsJson = JsonConvert.SerializeObject(extensionSettings);
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = extensionSettingsJson;

            // Set the Menu item to reflect change.
            MenuUI.Instance.MiOnLoadSaz.Checked = sessionAnalysisOnLoadSaz;
        }

        public bool SessionAnalysisOnLiveTrace
        {
            get
            {
                var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
                return extensionSettings.SessionAnalysisOnLiveTrace;
            }
        }

        public void SetSessionAnalysisOnLiveTrace(Boolean sessionAnalysisOnLiveTrace)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.SessionAnalysisOnLiveTrace = sessionAnalysisOnLiveTrace;
            // Serialize the object back into Json.
            var extensionSettingsJson = JsonConvert.SerializeObject(extensionSettings);
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = extensionSettingsJson;

            // Set the Menu item to reflect change.
            MenuUI.Instance.MiOnLiveTrace.Checked = sessionAnalysisOnLiveTrace;
        }

        public int WarningSessionTimeThreshold
        {
            get
            {
                var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
                return extensionSettings.WarningSessionTimeThreshold;
            }
        }

        public void UpdateWarningSessionTimeThreshold(int warningSessionTimeThreshold)
        {
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            extensionSettings.WarningSessionTimeThreshold = warningSessionTimeThreshold;

            var extensionSettingsJson = JsonConvert.SerializeObject(extensionSettings);
            Preferences.ExtensionSettings = extensionSettingsJson;
        }

        public int SlowRunningSessionThreshold
        {
            get
            {
                var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
                return extensionSettings.SlowRunningSessionThreshold;
            }
        }

        public void UpdateSlowRunningSessionThreshold(int slowRunningSessionThreshold)
        {
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            extensionSettings.SlowRunningSessionThreshold = slowRunningSessionThreshold;

            var extensionSettingsJson = JsonConvert.SerializeObject(extensionSettings);
            Preferences.ExtensionSettings = extensionSettingsJson;
        }

        public void IncrementExecutionCount()
        {
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            extensionSettings.ExecutionCount++;

            var extensionSettingsJson = JsonConvert.SerializeObject(extensionSettings);
            // Save the new Json to the extension setting.
            Preferences.ExtensionSettings = extensionSettingsJson;
        }

        public ExtensionVersionFlags GetDeserializedExtentionVersion()
        {
            var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return JsonConvert.DeserializeObject<ExtensionVersionFlags>(Preferences.ExtensionVersion, JsonSettings);
        }

        public ExtensionURLs GetDeserializedExtensionURLs()
        {
            var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return JsonConvert.DeserializeObject<ExtensionURLs>(Preferences.ExtensionURLs, JsonSettings);
        }

        public ExtensionSettingsFlags GetDeserializedExtensionSettings()
        {
            var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return JsonConvert.DeserializeObject<ExtensionSettingsFlags>(Preferences.ExtensionSettings, JsonSettings);
        }

        public async void UpdateExtensionURLsFromGithub()
        {
            // Function to try catch connecting to ExtensionSettings.json file from Github repo, pulling the content and writing it into the ExtensionSettings
            // Fiddler setting for recall as needed.
        }

        /* SETTINGS AREN"T BEING UPDATED FROM GITHUB. NONE OF THIS SHOULD BE NEEDED.
        public async void GetSettingsFromGithub()
        {
            var ExtensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettingsFlags();

            // If disable web calls is set, don't look for any URL updates.
            if (ExtensionSettings.NeverWebCall)
            {
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: NeverWebCall is enabled, not checking for updates for settings.json.");
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
        }*/

    }

    public class ExtensionURLs {
        public string ExtensionVersionJson { get; set; }

        public string UpdateJson { get; set; }

        public string MasterRuleSet { get; set; }

        public string BetaRuleSet { get; set; }

        public string Installer { get; set; }

        public string Wiki { get; set; }

        public string ReportIssues { get; set; }
    }

    public class ExtensionVersionFlags
    {
        public string UpdateMessage { get; set; }

        public int VersionMajor { get; set; }

        public int VersionMinor { get; set; }

        public int VersionBuild { get; set; }

        public DateTime ExtensonVersionLastUpdated { get; set; }
    }

    public class ExtensionSettingsFlags
    {
        public string ExtensionVersionURL { get; set; }

        public bool ExtensionEnabled { get; set; }

        public int ExecutionCount { get; set; }

        public bool NeverWebCall { get; set; }

        public string UpdateMessage { get; set; }

        public bool SessionAnalysisOnFiddlerLoad { get; set; }

        public bool SessionAnalysisOnLoadSaz { get; set; }

        public bool SessionAnalysisOnLiveTrace { get; set; }

        public int WarningSessionTimeThreshold { get; set; }

        public int SlowRunningSessionThreshold { get; set; }

        public string ExtensionDLL { get; set; }

        public string LastLoadedSazFile { get; set; }

        public bool UseBetaRuleSet { get; set; }

        public DateTime LocalMasterRulesetLastUpdated { get; set; }

        public DateTime LocalBetaRulesetLastUpdated { get; set; }
    }
}
