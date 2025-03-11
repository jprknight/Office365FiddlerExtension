using Fiddler;
using System;
using System.IO;
using System.Reflection;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Create the Json Session Classification application Preference.
    /// See RulesetSessionClassificationService in the rule set where data is pulled from the Json for session processing.
    /// </summary>
    public class SessionClassificationService
    {
        internal Session session { get; set; }

        private static SessionClassificationService _instance;
        public static SessionClassificationService Instance => _instance ?? (_instance = new SessionClassificationService());

        /// <summary>
        /// SessionClassification.json is delivered to the output directory, for any users who have 'NeverWebCall' true.
        /// Create the Session Classification application preference to store the Json.
        /// </summary>
        public void CreateSessionClassificationFiddlerApplicationPreference()
        {
            if (Preferences.SessionClassification != null)
            {
                return;
            }

            try
            {
                var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

                string JsonFilename = $"{extensionSettings.ExtensionPath}\\{extensionSettings.SessionClassificationJsonFileName}";

                string json = File.ReadAllText(JsonFilename);

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Creating SessionClassification Json Fiddler setting from {JsonFilename}.");

                Preferences.SessionClassification = json;
            }
            catch (Exception ex)
            {
                TelemetryService.CustomTrackException(ex);
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Failed to create Session Classification Fiddler Setting {ex}");
            }
        }
    }
}
