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
                LocalBetaRulesetLastUpdated = "",
                UpdateCheckFrequencyHours = 72
            };

            // Transform the object to a Json object.
            string jsonData = JsonConvert.SerializeObject(ExtensionSettings);

            // Save the new Json to the Fiddler setting.
            Preferences.ExtensionSettings = jsonData;
            
            // Remove legacy Fiddler settings only if Json can be read from.
            if (ExtensionSettings.ExtensionEnabled)
            {
                FiddlerApplication.Prefs.RemovePref("Enabled");
                FiddlerApplication.Prefs.RemovePref("ManualCheckForUpdate");
                FiddlerApplication.Prefs.RemovePref("UpdateMessage");
                FiddlerApplication.Prefs.RemovePref("ExecutionCount");
                FiddlerApplication.Prefs.RemovePref("NeverWebCall");
            }
        }

        public bool ExtensionEnabled
        {
            get
            {
                return SettingsHandler.Instance.GetDeserializedExtensionSettings().ExtensionSessionProcessingEnabled;
            }
        }

        public void SetNextUpdateTimestamp()
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            // Set the next update check timestamp to x hours in the future.
            extensionSettings.NextUpdateCheck = DateTime.Now.AddHours(extensionSettings.UpdateCheckFrequencyHours);
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        public String TelemetryInstrumentationKey
        {
            get
            {
                return URLsHandler.Instance.GetDeserializedExtensionURLs().TelemetryInstrumentationKey;
            }
        }

    public void SetExtensionEnabled(Boolean extensionEnabled)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.ExtensionSessionProcessingEnabled = extensionEnabled;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);

            // Set the Menu item to reflect change.
            MenuUI.Instance.ExtensionMenu.Text = ExtensionEnabled ? "Office 365 (Enabled)" : "Office 365 (Disabled)";
        }

        public bool SessionAnalysisOnLoadSaz
        {
            get
            {
                return SettingsHandler.Instance.GetDeserializedExtensionSettings().SessionAnalysisOnLoadSaz;
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
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        public string ExtensionDLL
        {
            get
            {
                return SettingsHandler.Instance.GetDeserializedExtensionSettings().ExtensionDLL;
            }
        }

        public void SetExtensionDLL()
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.ExtensionDLL = AssemblyName;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        public void SetSessionAnalysisOnLoadSaz(Boolean sessionAnalysisOnLoadSaz)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.SessionAnalysisOnLoadSaz = sessionAnalysisOnLoadSaz;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        public bool SessionAnalysisOnLiveTrace
        {
            get
            {
                return SettingsHandler.Instance.GetDeserializedExtensionSettings().SessionAnalysisOnLiveTrace;
            }
        }

        public void SetSessionAnalysisOnLiveTrace(Boolean sessionAnalysisOnLiveTrace)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.SessionAnalysisOnLiveTrace = sessionAnalysisOnLiveTrace;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        public int WarningSessionTimeThreshold
        {
            get
            {
                return SettingsHandler.Instance.GetDeserializedExtensionSettings().WarningSessionTimeThreshold;
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

                Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
            }
        }

        public int SlowRunningSessionThreshold
        {
            get
            {
                return SettingsHandler.Instance.GetDeserializedExtensionSettings().SlowRunningSessionThreshold;
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

                Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
            }
        }

        public void IncrementExecutionCount()
        {
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            extensionSettings.ExecutionCount++;

            // Save the new Json to the extension setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        public ExtensionSettingsJson GetDeserializedExtensionSettings()
        {
            var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return JsonConvert.DeserializeObject<ExtensionSettingsJson>(Preferences.ExtensionSettings, JsonSettings);
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

    public class ExtensionSettingsJson
    {
        public string ExtensionVersion { get; set; }

        public bool ExtensionSessionProcessingEnabled { get; set; }

        public int ExecutionCount { get; set; }

        public bool NeverWebCall { get; set; }

        public int UpdateCheckFrequencyHours { get; set; }

        public DateTime NextUpdateCheck { get; set; }

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
