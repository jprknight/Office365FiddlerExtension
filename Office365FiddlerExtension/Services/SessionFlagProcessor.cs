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
        public void SetExtensionSessionFlagJson(Session session, String Json)
        {
            this.session = session;
            this.session["Microsoft365FiddlerExtensionJson"] = Json;
        }

        // Take any updates to session flags and save them into the session Json.
        public void UpdateSessionFlagJson(Session session, String Json)
        {
            this.session = session;

            // pull Json for any session flags already set.
            var existingSessionFlags = this.session["Microsoft365FiddlerExtensionJson"];
            var existingSessionFlagsJson = JsonConvert.DeserializeObject<ExtensionSessionFlags>(existingSessionFlags);

            // Pull Json for new session flags passed into function.
            var newSessionFlagsJson = JsonConvert.DeserializeObject<ExtensionSessionFlags>(Json);

            // Add the new SectionTitle to any existing value.
            newSessionFlagsJson.SectionTitle += existingSessionFlagsJson.SectionTitle;

            // Replace all other values with new values.
            newSessionFlagsJson.UIBackColour = existingSessionFlagsJson.UIBackColour;
            newSessionFlagsJson.UITextColour = existingSessionFlagsJson.UITextColour;
            
            newSessionFlagsJson.SessionType = existingSessionFlagsJson.SessionType;
            
            newSessionFlagsJson.ResponseCodeDescription = existingSessionFlagsJson.ResponseCodeDescription;
            newSessionFlagsJson.ResponseServer = existingSessionFlagsJson.ResponseServer;
            newSessionFlagsJson.ResponseAlert = existingSessionFlagsJson.ResponseAlert;
            newSessionFlagsJson.ResponseComments = existingSessionFlagsJson.ResponseComments;

            newSessionFlagsJson.Authentication = existingSessionFlagsJson.Authentication;

            newSessionFlagsJson.SessionAuthenticationConfidenceLevel = existingSessionFlagsJson.SessionAuthenticationConfidenceLevel;
            newSessionFlagsJson.SessionResponseServerConfidenceLevel = existingSessionFlagsJson.SessionResponseServerConfidenceLevel;
            newSessionFlagsJson.SessionTypeConfidenceLevel = existingSessionFlagsJson.SessionTypeConfidenceLevel;

            var newJson = JsonConvert.SerializeObject(newSessionFlagsJson, Formatting.Indented);

            // Save the new Json to the session flag.
            this.session["Microsoft365FiddlerExtensionJson"] = newJson;           
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

            public string Authentication { get; set; }

            public int SessionAuthenticationConfidenceLevel { get; set; }

            public int SessionTypeConfidenceLevel { get; set; }

            public int SessionResponseServerConfidenceLevel { get; set; }
        }
    }
}
