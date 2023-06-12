﻿using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Handlers;
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

        public bool ExtensionEnabled
        {
            get
            {
                var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
                return extensionSettings.ExtensionSessionProcessingEnabled;
            }
        }

        public void SetExtensionEnabled(Boolean extensionEnabled)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.ExtensionSessionProcessingEnabled = extensionEnabled;
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
        }

        public bool SessionAnalysisOnLoadSaz
        {
            get
            {
                var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
                return extensionSettings.SessionAnalysisOnLoadSaz;
            }
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string AssemblyName
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Name + ".dll";
            }
        }

        public void SetExtensionPath()
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.ExtensionPath = AssemblyDirectory;
            // Serialize the object back into Json.
            var extensionSettingsJson = JsonConvert.SerializeObject(extensionSettings);
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = extensionSettingsJson;
        }

        public string ExtensionDLL
        {
            get
            {
                var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
                return extensionSettings.ExtensionDLL;
            }
        }

        public void SetExtensionDLL()
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.ExtensionDLL = AssemblyName;
            // Serialize the object back into Json.
            var extensionSettingsJson = JsonConvert.SerializeObject(extensionSettings);
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = extensionSettingsJson;
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
        }

        public int WarningSessionTimeThreshold
        {
            get
            {
                var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
                return extensionSettings.WarningSessionTimeThreshold;
            }
        }

        public void UpdateWarningSessionTimeThreshold(string warningSessionTimeThreshold)
        {
            // Validate input is int and only act if it is.
            int iWarningSessionTimeThreshold;

            var isNumberic = int.TryParse(warningSessionTimeThreshold, out iWarningSessionTimeThreshold);

            if (isNumberic)
            {
                var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
                extensionSettings.WarningSessionTimeThreshold = iWarningSessionTimeThreshold;

                var extensionSettingsJson = JsonConvert.SerializeObject(extensionSettings);
                Preferences.ExtensionSettings = extensionSettingsJson;
            }
        }

        public int SlowRunningSessionThreshold
        {
            get
            {
                var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
                return extensionSettings.SlowRunningSessionThreshold;
            }
        }

        public void UpdateSlowRunningSessionThreshold(string slowRunningSessionThreshold)
        {
            // Validate input is int and only act if it is.
            int iSlowRunningSessionThreshold;

            var isNumberic = int.TryParse(slowRunningSessionThreshold, out iSlowRunningSessionThreshold);

            if (isNumberic)
            {
                var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
                extensionSettings.SlowRunningSessionThreshold = iSlowRunningSessionThreshold;

                var extensionSettingsJson = JsonConvert.SerializeObject(extensionSettings);
                Preferences.ExtensionSettings = extensionSettingsJson;
            }
        }

        public void IncrementExecutionCount()
        {
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            extensionSettings.ExecutionCount++;

            var extensionSettingsJson = JsonConvert.SerializeObject(extensionSettings);
            // Save the new Json to the extension setting.
            Preferences.ExtensionSettings = extensionSettingsJson;
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

        public String TelemetryInstrumentationKey
        {
            get
            {
                var extensionURLs = URLsHandler.Instance.GetDeserializedExtensionURLs();
                return extensionURLs.TelemetryInstrumentationKey;
            }
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

    public class ExtensionSettingsFlags
    {
        public string ExtensionVersionURL { get; set; }

        public bool ExtensionSessionProcessingEnabled { get; set; }

        public int ExecutionCount { get; set; }

        public bool NeverWebCall { get; set; }

        public string UpdateMessage { get; set; }

        public bool SessionAnalysisOnFiddlerLoad { get; set; }

        public bool SessionAnalysisOnLoadSaz { get; set; }

        public bool SessionAnalysisOnLiveTrace { get; set; }

        public int WarningSessionTimeThreshold { get; set; }

        public int SlowRunningSessionThreshold { get; set; }

        public string ExtensionPath { get; set; }

        public string ExtensionDLL { get; set; }

        public string LastLoadedSazFile { get; set; }

        public bool UseBetaRuleSet { get; set; }

        public DateTime LocalMasterRulesetLastUpdated { get; set; }

        public DateTime LocalBetaRulesetLastUpdated { get; set; }
    }
}