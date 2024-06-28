using Fiddler;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using Office365FiddlerExtension.Services;

namespace Office365FiddlerExtensionRuleset.Services
{
    /// <summary>
    /// Functions to ensure ExtensionSettings Json is created and populated.
    /// </summary>

    public class RulesetSettingsJsonService
    {
        private static RulesetSettingsJsonService _instance;
        public static RulesetSettingsJsonService Instance => _instance ?? (_instance = new RulesetSettingsJsonService());

        /// <summary>
        /// Create settings if they don't exist. 
        /// </summary>
        public void CreateExtensionSettingsFiddlerApplicationPreference()
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

        /// <summary>
        /// Returns bool on whether the Preferred language is the current language in use.
        /// </summary>
        /// <param name="language"></param>
        /// <returns>bool</returns>
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

        /// <summary>
        /// Gets assembly directory.
        /// </summary>
        /// <returns>string assembly directory.</returns>
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

        /// <summary>
        /// Gets assembly name.
        /// </summary>
        /// <returns>string assembly name.</returns>
        public static string AssemblyName
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Name + ".dll";
            }
        }

        /// <summary>
        /// Get Warning session time threshold from extension settings Json application preference.
        /// </summary>
        /// <return>int</return>
        public int WarningSessionTimeThreshold
        {
            get
            {
                return SettingsJsonService.Instance.GetDeserializedExtensionSettings().WarningSessionTimeThreshold;
            }
        }

        /// <summary>
        /// Get slow running session threshold from extension settings Json application preference.
        /// </summary>
        /// <return>int</return>
        public int SlowRunningSessionThreshold
        {
            get
            {
                return SettingsJsonService.Instance.GetDeserializedExtensionSettings().SlowRunningSessionThreshold;
            }
        }

        /// <summary>
        /// Get Json deserialised extension settings from application preference.
        /// </summary>
        /// <returns></returns>
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
