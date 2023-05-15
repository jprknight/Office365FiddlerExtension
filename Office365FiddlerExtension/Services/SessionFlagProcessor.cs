using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;
using Newtonsoft.Json;

namespace Office365FiddlerExtension.Services
{
    class SessionFlagProcessor : ActivationService
    {
        /*
        public void SetExtensionSessionFlagJson(Session session, String Json)
        {
            this.session = session;
            this.session["Microsoft365FiddlerExtensionJson"] = Json;
        }*/

        private static SessionFlagProcessor _instance;
        public static SessionFlagProcessor Instance => _instance ?? (_instance = new SessionFlagProcessor());

        // Take any updates to session flags and save them into the session Json.
        public void UpdateSessionFlagJson(Session session, String JsonData)
        {
            this.session = session;
            /*
            var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };*/

            // Make sure all session flags have something to start out with, even if all values are null.
            var existingSessionFlags = "{\"SectionTitle\":null,\"UIBackColour\":null,\"UITextColour\":null,\"SessionType\":null,\"ResponseCodeDescription\":null,\"ResponseServer\":null,\"ResponseAlert\":null,\"ResponseComments\":null,\"Authentication\":null,\"SessionAuthenticationConfidenceLevel\":0,\"SessionTypeConfidenceLevel\":0,\"SessionResponseServerConfidenceLevel\":0}";
            var existingSessionFlagsJson = JsonConvert.DeserializeObject<ExtensionSessionFlags>(existingSessionFlags);

            // pull Json for any session flags already set.
            if (this.session["Microsoft365FiddlerExtensionJson"] != null)
            {
                existingSessionFlags = this.session["Microsoft365FiddlerExtensionJson"];
                existingSessionFlagsJson = JsonConvert.DeserializeObject<ExtensionSessionFlags>(existingSessionFlags);
            }

            // Pull Json for new session flags passed into function.
            var updatedSessionFlagsJson = JsonConvert.DeserializeObject<ExtensionSessionFlags>(JsonData);

            // Add the new SectionTitle to any existing value.
            string SectionTitle = existingSessionFlagsJson.SectionTitle + " " + updatedSessionFlagsJson.SectionTitle;
            updatedSessionFlagsJson.SectionTitle = SectionTitle;

            // Replace all other values with new values as long as we don't pass in a null value.
            
            // UIBackColour
            if (updatedSessionFlagsJson.UIBackColour == null)
            {
                updatedSessionFlagsJson.UIBackColour = existingSessionFlagsJson.UIBackColour;
            }
            
            // UITextColour
            if (updatedSessionFlagsJson.UITextColour == null)
            {
                updatedSessionFlagsJson.UITextColour = existingSessionFlagsJson.UITextColour;
            }
            
            // Session Type
            if (updatedSessionFlagsJson.SessionType == null)
            {
                updatedSessionFlagsJson.SessionType = existingSessionFlagsJson.SessionType;
            }
            
            // Response Code Description
            if (updatedSessionFlagsJson.ResponseCodeDescription == null)
            {
                updatedSessionFlagsJson.ResponseCodeDescription = existingSessionFlagsJson.ResponseCodeDescription;
            }
            
            // Response Server
            if (updatedSessionFlagsJson.ResponseServer == null)
            {
                updatedSessionFlagsJson.ResponseServer = existingSessionFlagsJson.ResponseServer;
            }
            
            // Response Alert
            if (updatedSessionFlagsJson.ResponseAlert == null)
            {
                updatedSessionFlagsJson.ResponseAlert = existingSessionFlagsJson.ResponseAlert;
            }
            
            // Response Comments
            if (updatedSessionFlagsJson.ResponseComments == null)
            {
                updatedSessionFlagsJson.ResponseComments = existingSessionFlagsJson.ResponseComments;
            }

            // Data Age
            if (updatedSessionFlagsJson.DataAge == null)
            {
                updatedSessionFlagsJson.DataAge = existingSessionFlagsJson.DataAge;
            }
            
            // Calculated Session Age
            if (updatedSessionFlagsJson.CalculatedSessionAge == null)
            {
                updatedSessionFlagsJson.CalculatedSessionAge = existingSessionFlagsJson.CalculatedSessionAge;
            }
            
            // Date Data Collected
            if (updatedSessionFlagsJson.DateDataCollected == null)
            {
                updatedSessionFlagsJson.DateDataCollected = existingSessionFlagsJson.DateDataCollected;
            }

            // Server Think Time
            if (updatedSessionFlagsJson.ServerThinkTime == null)
            {
                updatedSessionFlagsJson.ServerThinkTime = existingSessionFlagsJson.ServerThinkTime;
            }

            // Transit Time
            if (updatedSessionFlagsJson.TransitTime == null)
            {
                updatedSessionFlagsJson.TransitTime = existingSessionFlagsJson.TransitTime;
            }

            // Authentication
            if (updatedSessionFlagsJson.Authentication == null)
            {
                updatedSessionFlagsJson.Authentication = existingSessionFlagsJson.Authentication;
            }

            // Session Confidence Levels
            // REVIEW THIS. These are ints, so they'll never be null. Does it make sense here to check updated value is greater than
            // existing value and only update if that's the case?
            // For now just updating these with passed values as these should always be set on logic from the ruleset.
            updatedSessionFlagsJson.SessionAuthenticationConfidenceLevel = existingSessionFlagsJson.SessionAuthenticationConfidenceLevel;
            updatedSessionFlagsJson.SessionResponseServerConfidenceLevel = existingSessionFlagsJson.SessionResponseServerConfidenceLevel;
            updatedSessionFlagsJson.SessionTypeConfidenceLevel = existingSessionFlagsJson.SessionTypeConfidenceLevel;

            var newJsonData = JsonConvert.SerializeObject(updatedSessionFlagsJson, Formatting.Indented);

            // Save the new Json to the session flag.
            this.session["Microsoft365FiddlerExtensionJson"] = newJsonData;           
        }

        public class ExtensionSessionFlags
        {
            public string SectionTitle { get; set; }

            public string UIBackColour { get; set; }

            public string UITextColour { get; set; }

            public string SessionType { get; set; }

            public string ResponseCodeDescription { get; set; }

            public string ResponseServer { get; set; }

            public string ResponseAlert { get; set; }

            public string ResponseComments { get; set; }

            public string DataAge { get; set; }

            public string CalculatedSessionAge { get; set; }

            public string DateDataCollected { get; set; }

            public string SessionTimersDescription { get; set; }

            public string ServerThinkTime { get; set; }

            public string TransitTime { get; set; }

            public string Authentication { get; set; }

            public int SessionAuthenticationConfidenceLevel { get; set; }

            public int SessionTypeConfidenceLevel { get; set; }

            public int SessionResponseServerConfidenceLevel { get; set; }
        }
    }
}
