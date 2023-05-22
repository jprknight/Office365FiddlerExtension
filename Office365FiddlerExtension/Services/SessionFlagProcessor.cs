using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fiddler;
using Newtonsoft.Json;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using Office365FiddlerExtension.Ruleset;

namespace Office365FiddlerExtension.Services
{
    class SessionFlagProcessor : ActivationService
    {
        private static SessionFlagProcessor _instance;
        public static SessionFlagProcessor Instance => _instance ?? (_instance = new SessionFlagProcessor());

        public string GetSessionJsonData(Session session)
        {
            this.session = session;
            return this.session["Microsoft365FiddlerExtensionJson"];
        }

        public Boolean GetAnySessionConfidenceLevelTen(Session session)
        {
            var ExtensionSessionFlags = JsonConvert.DeserializeObject<SessionFlagProcessor.ExtensionSessionFlags>(GetSessionJsonData(this.session));

            if (ExtensionSessionFlags.SessionAuthenticationConfidenceLevel == 10 ||
                ExtensionSessionFlags.SessionTypeConfidenceLevel == 10 ||
                ExtensionSessionFlags.SessionResponseServerConfidenceLevel == 10)
            {
                return true;
            }
            return false;
        }

        public void SetProcess(Session session)
        {
            this.session = session;

            string ProcessName = "";
            // Set process name, split and exclude port used.
            if (session.LocalProcess != String.Empty)
            {
                ProcessName = session.LocalProcess.Split(':')[0];
            }
            // No local process to split.
            else
            {
                ProcessName = "Remote Capture";
            }

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "SetProcess",
                ProcessName = ProcessName
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        public void CreateExtensionSessionFlag(Session session)
        {
            this.session = session;

            if (this.session["Microsoft365FiddlerExtensionJson"] == null)
            {
                var SessionFlagsData = new
                {
                    SectionTitle = "Initial / Not detected",
                    UIBackColour = "Initial / Not detected",
                    UITextColour = "Initial / Not detected",
                    SessionType = "Initial / Not detected",
                    ResponseCodeDescription = "Initial / Not detected",
                    ResponseServer = "Initial / Not detected",
                    ResponseAlert = "Initial / Not detected",
                    ResponseComments = "Initial / Not detected",
                    DataAge = "Initial / Not detected",
                    CalculatedSessionAge = "Initial / Not detected",
                    DateDataCollected = "Initial / Not detected",
                    SessionTimersDescription = "Initial / Not detected",
                    ServerThinkTime = "Initial / Not detected",
                    TransitTime = "Initial / Not detected",
                    ElapsedTime = "Initial / Not detected",
                    InspectorElapsedTime = "Initial / Not detected",
                    Authentication = "Initial / Not detected",
                    AuthenticationType = "Initial / Not detected",
                    AuthenticationDescription = "Initial / Not detected",
                    SamlTokenIssuer = "Initial / Not detected",
                    SamlTokenSigningCertificate = "Initial / Not detected",
                    SamlTokenAttributeNameUPN = "Initial / Not detected",
                    SamlTokenNameIdentifierFormat = "Initial / Not detected",
                    SamlTokenAttributeNameImmutibleID = "Initial / Not detected",
                    ProcessName = "Initial / Not detected",
                    SessionAuthenticationConfidenceLevel = "0",
                    SessionTypeConfidenceLevel = "0",
                    SessionResponseServerConfidenceLevel = "0"
                };

                //Tranform the object to a Json object.
                string jsonData = JsonConvert.SerializeObject(SessionFlagsData, Formatting.Indented);

                // Save the new Json to the session flag.
                this.session["Microsoft365FiddlerExtensionJson"] = jsonData;
            }            
        }

        // Take any updates to session flags and save them into the session Json.
        public void UpdateSessionFlagJson(Session session, String JsonData)
        {
            this.session = session;

            /*var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };*/

            if (this.session["Microsoft365FiddlerExtensionJson"] == null)
            {
                CreateExtensionSessionFlag(this.session);
            }

            var existingSessionFlags = this.session["Microsoft365FiddlerExtensionJson"];
            var existingSessionFlagsJson = JsonConvert.DeserializeObject<ExtensionSessionFlags>(existingSessionFlags);


            // Pull Json for new session flags passed into function.
            var updatedSessionFlagsJson = JsonConvert.DeserializeObject<ExtensionSessionFlags>(JsonData);

            // Add the new SectionTitle to any existing value.
            string SectionTitle;

            if (existingSessionFlagsJson.SectionTitle == null || existingSessionFlagsJson.SectionTitle.Length == 0)
            {
                SectionTitle = updatedSessionFlagsJson.SectionTitle;
            }
            else
            {
                SectionTitle = updatedSessionFlagsJson.SectionTitle + ", " + existingSessionFlagsJson.SectionTitle;
            }

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

            if (updatedSessionFlagsJson.SessionTimersDescription == null)
            {
                updatedSessionFlagsJson.SessionTimersDescription = existingSessionFlagsJson.SessionTimersDescription;
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

            // Elapsed Time
            if (updatedSessionFlagsJson.ElapsedTime == null)
            {
                updatedSessionFlagsJson.ElapsedTime = existingSessionFlagsJson.ElapsedTime;
            }

            // Inspector Elapsed Time
            if (updatedSessionFlagsJson.InspectorElapsedTime == null)
            {
                updatedSessionFlagsJson.InspectorElapsedTime = existingSessionFlagsJson.InspectorElapsedTime;
            }

            // Authentication
            if (updatedSessionFlagsJson.Authentication == null)
            {
                updatedSessionFlagsJson.Authentication = existingSessionFlagsJson.Authentication;
            }

            // Authentication Type
            if (updatedSessionFlagsJson.AuthenticationType == null)
            {
                updatedSessionFlagsJson.AuthenticationType = existingSessionFlagsJson.AuthenticationType;
            }

            // Authentication Description
            if (updatedSessionFlagsJson.AuthenticationDescription == null)
            {
                updatedSessionFlagsJson.AuthenticationDescription = existingSessionFlagsJson.AuthenticationDescription;
            }

            // SamlTokenIssuer
            if (updatedSessionFlagsJson.SamlTokenIssuer == null)
            {
                updatedSessionFlagsJson.SamlTokenIssuer = existingSessionFlagsJson.SamlTokenIssuer;
            }

            // SamlTokenSigningCertificate
            if (updatedSessionFlagsJson.SamlTokenSigningCertificate == null)
            {
                updatedSessionFlagsJson.SamlTokenSigningCertificate = existingSessionFlagsJson.SamlTokenSigningCertificate;
            }

            // SamlTokenAttributeNameUPN
            if (updatedSessionFlagsJson.SamlTokenAttributeNameUPN == null)
            {
                updatedSessionFlagsJson.SamlTokenAttributeNameUPN = existingSessionFlagsJson.SamlTokenAttributeNameUPN;
            }

            // SamlTokenNameIdentifierFormat
            if (updatedSessionFlagsJson.SamlTokenNameIdentifierFormat == null)
            {
                updatedSessionFlagsJson.SamlTokenNameIdentifierFormat = existingSessionFlagsJson.SamlTokenNameIdentifierFormat;
            }

            // SamlTokenAttributeNameImmutibleID
            if (updatedSessionFlagsJson.SamlTokenAttributeNameImmutibleID == null)
            {
                updatedSessionFlagsJson.SamlTokenAttributeNameImmutibleID = existingSessionFlagsJson.SamlTokenAttributeNameImmutibleID;
            }

            // Process Name
            if (updatedSessionFlagsJson.ProcessName == null)
            {
                updatedSessionFlagsJson.ProcessName = existingSessionFlagsJson.ProcessName;
            }

            // Session Confidence Levels

            // If the updated Session Confidence Levels are lower than the existing Session Confidence Levels, use the 
            // existing Session Confidence Levels instead.
            if (updatedSessionFlagsJson.SessionAuthenticationConfidenceLevel < existingSessionFlagsJson.SessionAuthenticationConfidenceLevel)
            {
                updatedSessionFlagsJson.SessionAuthenticationConfidenceLevel = existingSessionFlagsJson.SessionAuthenticationConfidenceLevel;
            }

            if (updatedSessionFlagsJson.SessionTypeConfidenceLevel < existingSessionFlagsJson.SessionTypeConfidenceLevel)
            {
                updatedSessionFlagsJson.SessionTypeConfidenceLevel = existingSessionFlagsJson.SessionTypeConfidenceLevel;
            }

            if (updatedSessionFlagsJson.SessionResponseServerConfidenceLevel < existingSessionFlagsJson.SessionResponseServerConfidenceLevel)
            {
                updatedSessionFlagsJson.SessionResponseServerConfidenceLevel = existingSessionFlagsJson.SessionResponseServerConfidenceLevel;
            }

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

            public string ElapsedTime { get; set; }

            public string InspectorElapsedTime { get; set; }

            public string Authentication { get; set; }

            public string AuthenticationType { get; set; }

            public string AuthenticationDescription { get; set; }

            public string SamlTokenIssuer { get; set; }

            public string SamlTokenSigningCertificate { get; set; }

            public string SamlTokenAttributeNameUPN { get; set; }

            public string SamlTokenNameIdentifierFormat { get; set; }

            public string SamlTokenAttributeNameImmutibleID { get; set; }

            public string ProcessName { get; set; }

            public int SessionAuthenticationConfidenceLevel { get; set; }

            public int SessionTypeConfidenceLevel { get; set; }

            public int SessionResponseServerConfidenceLevel { get; set; }
        }
    }
}
