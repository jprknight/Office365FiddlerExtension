﻿using Fiddler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Process the session classification Json the extension uses for sessions per response code.
    /// </summary>
    public class SessionClassificationService
    {
        internal Session session { get; set; }

        private static SessionClassificationService _instance;
        public static SessionClassificationService Instance => _instance ?? (_instance = new SessionClassificationService());

        /// <summary>
        /// Expecting a "Section|Section" to be passed into function.
        /// Function allows multiple depths to be passed in. Expecting 2 or 3 is the most likely use case.
        /// </summary>
        /// <param name="section"></param>

        /*
        Commenting out this function as it has moved to the Ruleset DLL. Want to force code checks move all references over to
        RulesetSessionClassificationService.cs
         
        public SessionClassificationJsonSection GetSessionClassificationJsonSection(string section)
        {
            string sectionPiece0 = "";
            string sectionPiece1 = "";

            var jsonSection = "";

            var parsedObject = JObject.Parse(Preferences.SessionClassification);

            if (section.Contains('|')) {
                string[] sectionPieces = section.Split('|');

                sectionPiece0 = sectionPieces[0];
                sectionPiece1 = sectionPieces[1];
                jsonSection = parsedObject[sectionPiece0][sectionPiece1].ToString();
            }
            else
            {
                jsonSection = parsedObject[section].ToString();
            }

            return JsonConvert.DeserializeObject<SessionClassificationJsonSection>(jsonSection);
        }*/

        /// <summary>
        /// SessionClassification.json is delivered to the output directory, for any users who have 'NeverWebCall' true.
        /// Create the application preference to store the Json in to be read from.
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
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Failed to create Session Classification Fiddler Setting {ex}");
            }
        }
    }

    public class SessionClassificationJsonSection
    {
        public string SectionTitle { get; set; }

        public string SessionType { get; set; }

        public string SessionResponseCodeDescription { get; set; }

        public string SessionResponseComments { get; set; }

        public string SessionResponseServer { get; set; }

        public string SessionResponseAlert { get; set; }

        public string SessionAuthentication { get; set; }

        public int SessionAuthenticationConfidenceLevel { get; set; }

        public int SessionTypeConfidenceLevel { get; set; }

        public int SessionResponseServerConfidenceLevel { get; set; }

        public int SessionSeverity { get; set; }
    }
}
