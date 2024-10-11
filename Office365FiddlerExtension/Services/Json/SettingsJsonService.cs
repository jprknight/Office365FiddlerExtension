using Fiddler;
using Newtonsoft.Json;
using System;
using System.Diagnostics.Eventing.Reader;
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
                SessionAnalysisOnImport = "True",
                WarningSessionTimeThreshold = "2500",
                SlowRunningSessionThreshold = "5000",
                ExtensionPath = AssemblyDirectory,
                ExtensionDLL = AssemblyName,
                SessionClassificationJsonFileName = "SessionClassification.json",
                UpdateCheckFrequencyHours = 72,
                InspectorScoreForSession = 100,
                PreferredLanguage = "EN",
                DebugMode = "False",
                CaptureTraffic = "",
                WarnBeforeAnalysing = "250",
                Whois = "True",
                ElapsedTimeColumnEnabled = "True",
                SeverityColumnEnabled = "False",
                SessionTypeColumnEnabled = "True",
                AuthenticationColumnEnabled = "True",
                ResponseServerColumnEnabled = "True",
                HostIPColumnEnabled = "True"
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
        /// Determine if the extension session processing enabled is true/false.
        /// Use this when wanting to determine if compute intensive operations should be performed or not.
        /// </summary>
        /// <returns>bool</returns>
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
                    $"({this.GetType().Name}): " +
                    $"PreferredLanguage cannot be determined");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): " +
                    $"{ex}");
            }
            return false;
        }

        public void SetWhois(bool whois)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            extensionSettings.Whois = whois;

            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        /// <summary>
        /// Set next update timestamp.
        /// </summary>
        public void SetNextUpdateTimestamp()
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            // If the NextUpdateCheck value is already more than DateTime right now and also less than DateTime plus the
            // UpdateCheckFrequencyHours, return.
            if (extensionSettings.NextUpdateCheck > DateTime.Now
                && extensionSettings.NextUpdateCheck < DateTime.Now.AddHours(extensionSettings.UpdateCheckFrequencyHours))
            {
                return;
            }

            // Set the next update check timestamp to x hours in the future.
            extensionSettings.NextUpdateCheck = DateTime.Now.AddHours(extensionSettings.UpdateCheckFrequencyHours);

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): " +
                $"Attempting to set NextUpdateCheck to {extensionSettings.NextUpdateCheck}");

            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        public void SetNeverWebCall(bool enabled)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            extensionSettings.NeverWebCall = enabled;

            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        public void SetDebugMode(bool enabled)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            extensionSettings.DebugMode = enabled;

            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        public void SetCaptureOnStartup(bool enabled)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            extensionSettings.CaptureTraffic = enabled;

            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        /// <summary>
        /// Set the update check frequency hours.
        /// </summary>
        /// <param name="hours"></param>
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
                    $"({this.GetType().Name}): " +
                    $"UpdateCheckFreqencyHours set to {ihours}.");
            }
            else
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): UpdateCheckFreqencyHours only accepts a numerical value.");
            }
        }

        /// <summary>
        /// Set extension session processing enabled.
        /// </summary>
        /// <param name="extensionSessionProcessingEnabled"></param>
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

            // MenuUI.Instance.ExtensionMenu.Text = ExtensionSessionProcessingEnabled ? "Office 365 (Enabled)" : "Office 365 (Disabled)";

            MenuUI.Instance.ExtensionMenu.Text = ExtensionSessionProcessingEnabled ? $"{LangHelper.GetString("Office 365")} " +
                $"({LangHelper.GetString("Enabled")})" : $"{LangHelper.GetString("Office 365")} ({LangHelper.GetString("Disabled")})";

            //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
            //    $"({this.GetType().Name}): ExtensionSessionProcessingEnabled set to {extensionSessionProcessingEnabled}.");
        }

        /// <summary>
        /// Determine if session analysis on load Saz is enabled.
        /// </summary>
        /// <returns>bool</returns>
        public bool SessionAnalysisOnLoadSaz
        {
            get
            {
                return SettingsJsonService.Instance.GetDeserializedExtensionSettings().SessionAnalysisOnLoadSaz;
            }
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
        /// Sets extension path in extension settings Json preference.
        /// </summary>
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

        /// <summary>
        /// Sets extension DLL in extension settings Json preference.
        /// </summary>
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

        /// <summary>
        /// Set session analysis on load saz in extension settings Json preference.
        /// </summary>
        /// <param name="sessionAnalysisOnLoadSaz"></param>
        public void SetSessionAnalysisOnLoadSaz(Boolean sessionAnalysisOnLoadSaz)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.SessionAnalysisOnLoadSaz = sessionAnalysisOnLoadSaz;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);

            //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
            //    $"({this.GetType().Name}): SessionAnalysisOnLoadSaz set to {sessionAnalysisOnLoadSaz}.");
        }

        /// <summary>
        /// Get Session analysis on live trace from extension settings Json application preference.
        /// </summary>
        /// <returns>bool</returns>
        public bool SessionAnalysisOnLiveTrace
        {
            get
            {
                return SettingsJsonService.Instance.GetDeserializedExtensionSettings().SessionAnalysisOnLiveTrace;
            }
        }

        /// <summary>
        /// Set session analysis on live trace in extension settings Json application preference.
        /// </summary>
        /// <param name="sessionAnalysisOnLiveTrace"></param>
        public void SetSessionAnalysisOnLiveTrace(Boolean sessionAnalysisOnLiveTrace)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.SessionAnalysisOnLiveTrace = sessionAnalysisOnLiveTrace;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);

            //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
            //    $"({this.GetType().Name}): SessionAnalysisOnLiveTrace set to {sessionAnalysisOnLiveTrace}.");
        }

        /// <summary>
        /// Get Session analysis on import from extension settings Json application preference.
        /// </summary>
        public bool SessionAnalysisOnImport
        {
            get
            {
                return SettingsJsonService.Instance.GetDeserializedExtensionSettings().SessionAnalysisOnImport;
            }
        }

        /// <summary>
        /// Set session analysis on import in extension settings Json application preference.
        /// </summary>
        /// <param name="sessionAnalysisOnImport"></param>
        public void SetSessionAnlysisOnImport(Boolean sessionAnalysisOnImport)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.SessionAnalysisOnImport = sessionAnalysisOnImport;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        /// <summary>
        /// Get WarnBeforeAnalysing from extension settings Json application preference.
        /// </summary>
        public int WarnBeforeAnalysing
        {
            get
            {
                return SettingsJsonService.Instance.GetDeserializedExtensionSettings().WarnBeforeAnalysing;
            }
        }

        public void SetWarnBeforeAnalysing(int warnBeforeAnalysing)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.WarnBeforeAnalysing = warnBeforeAnalysing;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
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
        /// Set warning session time threshold in extension settings Json application preference.
        /// </summary>
        /// <param name="warningSessionTimeThreshold"></param>
        public void SetWarningSessionTimeThreshold(string warningSessionTimeThreshold)
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
        /// Set slow running session threshold in extension settings Json application preference.
        /// </summary>
        /// <param name="slowRunningSessionThreshold"></param>
        public void SetSlowRunningSessionThreshold(string slowRunningSessionThreshold)
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

        public void SetElapsedColumnEnabled(bool enabled)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.ElapsedTimeColumnEnabled = enabled;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        public void SetSeverityColumnEnabled(bool enabled)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.SeverityColumnEnabled = enabled;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        public void SetSessionTypeColumnEnabled(bool enabled)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.SessionTypeColumnEnabled = enabled;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        public void SetResponseServerColumnEnabled(bool enabled)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.ResponseServerColumnEnabled = enabled;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        public void SetAuthenticationColumnEnabled(bool enabled)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.AuthenticationColumnEnabled = enabled;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        public void SetHostIPColumnEnabled(bool enabled)
        {
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            // Set the attribute.
            extensionSettings.HostIPColumnEnabled = enabled;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
        }

        /// <summary>
        /// Increment execution count in extension settings Json application preference.
        /// </summary>
        public void IncrementExecutionCount()
        {
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            extensionSettings.ExecutionCount++;

            // Save the new Json to the extension setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): Incremented ExecutionCount to {extensionSettings.ExecutionCount}.");
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

        public bool SessionAnalysisOnImport { get; set; }

        public bool SessionAnalysisOnLiveTrace { get; set; }

        public int WarningSessionTimeThreshold { get; set; }

        public int SlowRunningSessionThreshold { get; set; }

        public int InspectorScoreForSession { get; set; }

        public string ExtensionPath { get; set; }

        public string ExtensionDLL { get; set; }

        public string SessionClassificationJsonFileName { get; set; }

        public string PreferredLanguage { get; set; }

        public bool DebugMode { get; set; }

        public bool CaptureTraffic { get; set; }

        public int WarnBeforeAnalysing { get; set; }

        public bool Whois { get; set; }

        public bool ElapsedTimeColumnEnabled { get; set; }

        public bool SeverityColumnEnabled { get; set; }

        public bool SessionTypeColumnEnabled { get; set; }

        public bool AuthenticationColumnEnabled { get; set; }

        public bool ResponseServerColumnEnabled { get; set; }

        public bool HostIPColumnEnabled { get; set; }
    }
}
