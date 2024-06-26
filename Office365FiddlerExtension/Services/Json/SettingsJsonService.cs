﻿using Fiddler;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Functions to ensure ExtensionSettings Json is created and populated.
    /// </summary>

    public class SettingsJsonService
    {
        private static SettingsJsonService _instance;
        public static SettingsJsonService Instance => _instance ?? (_instance = new SettingsJsonService());

        /// <summary>
        /// Create settings if they don't exist. 
        /// </summary>
        public void CreateExtensionSettingsFiddlerSetting()
        {
            if (Preferences.ExtensionSettings != null)
            {
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
                ExtensionSessionProcessingEnabled = upgradeExtensionEnabled,
                ExecutionCount = upgradeExecutionCount,
                NeverWebCall = upgradeNeverWebCall,
                SessionAnalysisOnFiddlerLoad = "True",
                SessionAnalysisOnLoadSaz = "True",
                SessionAnalysisOnLiveTrace = "True",
                WarningSessionTimeThreshold = "2500",
                SlowRunningSessionThreshold = "5000",
                ExtensionPath = AssemblyDirectory,
                ExtensionDLL = AssemblyName,
                SessionClassificationJsonFileName = "SessionClassification.json",
                UpdateCheckFrequencyHours = 72,
                InspectorScoreForSession = 100,
                PreferredLanguage = "en-GB"
            };

            // Transform the object to a Json object.
            string jsonData = JsonConvert.SerializeObject(ExtensionSettings);

            // Save the new Json to the Fiddler setting.
            Preferences.ExtensionSettings = jsonData;
            
            // Remove legacy Fiddler settings only if Json can be read from.
            if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().ExtensionSessionProcessingEnabled)
            {
                FiddlerApplication.Prefs.RemovePref("Enabled");
                FiddlerApplication.Prefs.RemovePref("ManualCheckForUpdate");
                FiddlerApplication.Prefs.RemovePref("UpdateMessage");
                FiddlerApplication.Prefs.RemovePref("ExecutionCount");
                FiddlerApplication.Prefs.RemovePref("NeverWebCall");
            }
        }

        public bool ExtensionSessionProcessingEnabled
        {
            get
            {
                try
                {
                    return SettingsJsonService.Instance.GetDeserializedExtensionSettings().ExtensionSessionProcessingEnabled;
                } 
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                        $"({this.GetType().Name}): ExtensionSessionProcessingEnabled cannot be determined");
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                        $"({this.GetType().Name}): {ex}");
                }
                return false;
            }
        }

        public bool GetPreferredLanguageBool(string language )
        {
            try
            {
                if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().PreferredLanguage == language)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): PreferredLanguage cannot be determined");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {ex}");
            }
            return false;
        }

        public void SetNextUpdateTimestamp()
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            // Set the next update check timestamp to x hours in the future.
            extensionSettings.NextUpdateCheck = DateTime.Now.AddHours(extensionSettings.UpdateCheckFrequencyHours);
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        public void SetUpdateCheckFrequencyHours(string hours)
        {
            // Validate input is int and only act if it is.
            var isNumberic = int.TryParse(hours, out int ihours);

            if (isNumberic)
            {
                var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
                extensionSettings.UpdateCheckFrequencyHours = ihours;

                Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): UpdateCheckFreqencyHours set to {ihours}.");
            }
            else
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): UpdateCheckFreqencyHours only accepts a numerical value.");
            }
        }

        public String TelemetryInstrumentationKey
        {
            get
            {
                return URLsJsonService.Instance.GetDeserializedExtensionURLs().TelemetryInstrumentationKey;
            }
        }

        public void SetExtensionSessionProcessingEnabled(Boolean extensionSessionProcessingEnabled)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.ExtensionSessionProcessingEnabled = extensionSessionProcessingEnabled;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);

            // Set the Menu item to reflect change.
            // REVIEW THIS - Checked March 2024 - Change needed if enabling Multi-Language support.
            MenuUI.Instance.ExtensionMenu.Text = ExtensionSessionProcessingEnabled ? "Office 365 (Enabled)" : "Office 365 (Disabled)";

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): ExtensionSessionProcessingEnabled set to {extensionSessionProcessingEnabled}.");
        }

        public bool SessionAnalysisOnLoadSaz
        {
            get
            {
                return SettingsJsonService.Instance.GetDeserializedExtensionSettings().SessionAnalysisOnLoadSaz;
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
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
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
                return SettingsJsonService.Instance.GetDeserializedExtensionSettings().ExtensionDLL;
            }
        }

        public void SetExtensionDLL()
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.ExtensionDLL = AssemblyName;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        public void SetSessionAnalysisOnLoadSaz(Boolean sessionAnalysisOnLoadSaz)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.SessionAnalysisOnLoadSaz = sessionAnalysisOnLoadSaz;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): SessionAnalysisOnLoadSaz set to {sessionAnalysisOnLoadSaz}.");
        }

        public bool SessionAnalysisOnLiveTrace
        {
            get
            {
                return SettingsJsonService.Instance.GetDeserializedExtensionSettings().SessionAnalysisOnLiveTrace;
            }
        }

        public void SetSessionAnalysisOnLiveTrace(Boolean sessionAnalysisOnLiveTrace)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.SessionAnalysisOnLiveTrace = sessionAnalysisOnLiveTrace;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): SessionAnalysisOnLiveTrace set to {sessionAnalysisOnLiveTrace}.");
        }

        public int WarningSessionTimeThreshold
        {
            get
            {
                return SettingsJsonService.Instance.GetDeserializedExtensionSettings().WarningSessionTimeThreshold;
            }
        }

        public void UpdateWarningSessionTimeThreshold(string warningSessionTimeThreshold)
        {
            // Validate input is int and only act if it is.
            var isNumberic = int.TryParse(warningSessionTimeThreshold, out int iWarningSessionTimeThreshold);

            if (isNumberic)
            {
                var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
                extensionSettings.WarningSessionTimeThreshold = iWarningSessionTimeThreshold;

                Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): WarningSessionTimeThreshold set to {iWarningSessionTimeThreshold}.");
            }
            else
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): WarningSessionTimeThreshold only accepts a numerical value.");
            }
        }

        public int SlowRunningSessionThreshold
        {
            get
            {
                return SettingsJsonService.Instance.GetDeserializedExtensionSettings().SlowRunningSessionThreshold;
            }
        }

        public void UpdateSlowRunningSessionThreshold(string slowRunningSessionThreshold)
        {
            // Validate input is int and only act if it is.
            var isNumberic = int.TryParse(slowRunningSessionThreshold, out int iSlowRunningSessionThreshold);

            if (isNumberic)
            {
                var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
                extensionSettings.SlowRunningSessionThreshold = iSlowRunningSessionThreshold;

                Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): SlowRunningSessionThreshold set to {iSlowRunningSessionThreshold}.");
            }
            else
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): SlowRunningSessionThreshold only accepts a numerical value.");
            }
        }

        public void IncrementExecutionCount()
        {
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            extensionSettings.ExecutionCount++;

            // Save the new Json to the extension setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): Incremented ExecutionCount to {extensionSettings.ExecutionCount}.");
        }

        public ExtensionSettingsJson GetDeserializedExtensionSettings()
        {
            var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            try
            {
                return JsonConvert.DeserializeObject<ExtensionSettingsJson>(Preferences.ExtensionSettings, JsonSettings);
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): Error running GetDeserializedExtensionSettings.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {ex}");

            }
            return null;
        }
    }

    public class ExtensionSettingsJson
    {
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

        public int InspectorScoreForSession { get; set; }

        public string ExtensionPath { get; set; }

        public string ExtensionDLL { get; set; }

        public string SessionClassificationJsonFileName { get; set; }

        public string PreferredLanguage { get; set; }
    }
}
