using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.UI;
using System.Reflection;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Function to stamp all session flags the extension uses.
    /// Needs to be public so the CodeDom Compiler can access ExtensionSessionFlags.
    /// </summary>
    public class SessionFlagHandler : ActivationService
    {
        private static SessionFlagHandler _instance;
        public static SessionFlagHandler Instance => _instance ?? (_instance = new SessionFlagHandler());

        /// <summary>
        /// Return deserialised Json session flags stored in each session in the Fiddler UI.
        /// </summary>
        /// <param name="Session"></param>
        /// <returns></returns>
        public ExtensionSessionFlags GetDeserializedSessionFlags(Session Session)
        {
            this.Session = Session;

            try
            {
                return JsonConvert.DeserializeObject<SessionFlagHandler.ExtensionSessionFlags>(SessionFlagHandler.Instance.GetSessionJsonData(this.Session));
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error deserializing session flags.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
            return null;
        }

        /// <summary>
        /// Return the raw Json data from the session flag.
        /// </summary>
        /// <param name="Session"></param>
        /// <returns></returns>
        public string GetSessionJsonData(Session Session)
        {
            this.Session = Session;

            // Make sure the extension session flag is created if it doesn't exist.
            CreateExtensionSessionFlag(this.Session);

            return this.Session["Microsoft365FiddlerExtensionJson"];
        }

        /// <summary>
        /// Return string for sessions where no known issue is needed.
        /// Used across response code logic.
        /// </summary>
        /// <returns></returns>
        public string ResponseCommentsNoKnownIssue()
        {
            return "<p>No known issue with Microsoft365 and this type of session. If you have a suggestion for an improvement, "
                + "create an issue or better yet a pull request in the project Github repository: "
                + "<a href='https://aka.ms/Office365FiddlerExtension' target='_blank'>https://aka.ms/Office365FiddlerExtension</a>.</p>";
        }

        /// <summary>
        /// Returns true if any session confidence level is 10.
        /// </summary>
        /// <param name="Session"></param>
        /// <returns></returns>
        public Boolean GetAnySessionConfidenceLevelTen(Session Session)
        {
            this.Session = Session;

            var ExtensionSessionFlags = JsonConvert.DeserializeObject<SessionFlagHandler.ExtensionSessionFlags>(GetSessionJsonData(this.Session));

            if (ExtensionSessionFlags.SessionAuthenticationConfidenceLevel == 10 ||
                ExtensionSessionFlags.SessionTypeConfidenceLevel == 10 ||
                ExtensionSessionFlags.SessionResponseServerConfidenceLevel == 10)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Creates the session flags on each session. Avoid null exceptions.
        /// </summary>
        /// <param name="Session"></param>
        public void CreateExtensionSessionFlag(Session Session)
        {
            this.Session = Session;

            if (this.Session["Microsoft365FiddlerExtensionJson"] != null)
            {
                return;
            }

            //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} Json extension session flag not found. Creating.");

            var SessionFlagsData = new
            {
                SectionTitle = "",
                UIBackColour = "",
                UITextColour = "",
                SessionType = "",
                ResponseCodeDescription = "",
                ResponseServer = "",
                ResponseAlert = "",
                ResponseComments = "",
                DataAge = "",
                CalculatedSessionAge = "",
                DateDataCollected = "",
                SessionTimersDescription = "",
                ServerThinkTime = "",
                TransitTime = "",
                ElapsedTime = "",
                InspectorElapsedTime = "",
                Authentication = "",
                AuthenticationType = "",
                AuthenticationDescription = "",
                SamlTokenIssuer = "",
                SamlTokenSigningCertificate = "",
                SamlTokenAttributeNameUPN = "",
                SamlTokenNameIdentifierFormat = "",
                SamlTokenAttributeNameImmutibleID = "",
                ProcessName = "",
                SessionAuthenticationConfidenceLevel = "0",
                SessionTypeConfidenceLevel = "0",
                SessionResponseServerConfidenceLevel = "0"
            };

            // Transform the object to a Json object.
            string jsonData = JsonConvert.SerializeObject(SessionFlagsData, Formatting.Indented);

            // Save the new Json to the session flag.
            this.Session["Microsoft365FiddlerExtensionJson"] = jsonData;
        }

        /// <summary>
        /// Processes the selected sessions in Fiddler. Called from the MenuUI and ContextMenuUI.
        /// </summary>
        public void ProcessSelectedSessions()
        {
            var Sessions = FiddlerApplication.UI.GetSelectedSessions();

            foreach (var Session in Sessions)
            {
                this.Session = Session;
                SessionHandler.Instance.OnPeekAtResponseHeaders(this.Session);
            }
        }

        /// <summary>
        /// Processes all sessions loaded in Fiddler. Called from the MenuUI and ContextMenuUI.
        /// </summary>
        public void ProcessAllSessions()
        {
            var Sessions = FiddlerApplication.UI.GetAllSessions();
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Processing all {Sessions.Count()} current sessions.");

            foreach (var Session in Sessions)
            {
                this.Session = Session;
                SessionHandler.Instance.OnPeekAtResponseHeaders(this.Session);
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Processed {Sessions.Count()} sessions.");
        }

        /// <summary>
        /// Clears processing from all sessions loaded in Fiddler. Called from the MenuUI and ContextMenuUI.
        /// </summary>
        public void ClearAllSessionProcessing()
        {
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Clearing all current session procesing.");

            var Sessions = FiddlerApplication.UI.GetAllSessions();
            foreach (var Session in Sessions)
            {
                this.Session = Session;

                EnhanceSessionUX.Instance.NormaliseSession(this.Session);

                this.Session.RefreshUI();
            }
        }

        /// <summary>
        /// Take any updates to session flags and save them into the session Json.
        /// </summary>
        /// <param name="Session"></param>
        /// <param name="JsonData"></param>
        public void UpdateSessionFlagJson(Session Session, String JsonData)
        {
            this.Session = Session;

            var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            CreateExtensionSessionFlag(this.Session);

            var existingSessionFlags = this.Session["Microsoft365FiddlerExtensionJson"];
            var existingSessionFlagsJson = JsonConvert.DeserializeObject<ExtensionSessionFlags>(existingSessionFlags, JsonSettings);


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
            this.Session["Microsoft365FiddlerExtensionJson"] = newJsonData;           
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
