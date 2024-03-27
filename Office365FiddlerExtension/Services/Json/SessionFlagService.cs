using System;
using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.UI;
using System.Reflection;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Function to stamp all session flags the extension uses.
    /// </summary>
    public class SessionFlagService
    {
        internal Session session { get; set; }

        private static SessionFlagService _instance;
        public static SessionFlagService Instance => _instance ?? (_instance = new SessionFlagService());

        /// <summary>
        /// Return deserialised Json session flags stored in each session in the Fiddler UI.
        /// </summary>
        /// <param name="Session"></param>
        /// <returns></returns>
        public ExtensionSessionFlags GetDeserializedSessionFlags(Session Session)
        {
            this.session = Session;

            try
            {
                return JsonConvert.DeserializeObject<SessionFlagService.ExtensionSessionFlags>(SessionFlagService.Instance.GetSessionJsonData(this.session));
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
            this.session = Session;

            // Make sure the extension session flag is created if it doesn't exist.
            CreateExtensionSessionFlag(this.session);

            return this.session["Microsoft365FiddlerExtensionJson"];
        }

        /// <summary>
        /// Creates the session flags on each session. Avoid null exceptions.
        /// </summary>
        /// <param name="Session"></param>
        public void CreateExtensionSessionFlag(Session Session)
        {
            this.session = Session;

            if (this.session["Microsoft365FiddlerExtensionJson"] != null)
            {
                return;
            }

            //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} Json extension session flag not found. Creating.");

            var SessionFlagsData = new
            {
                SectionTitle = "",
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
                HostIP = "",
                SessionAuthenticationConfidenceLevel = "0",
                SessionTypeConfidenceLevel = "0",
                SessionResponseServerConfidenceLevel = "0",
                SessionSeverity = "0",
                TLSVersion = ""
            };

            // Transform the object to a Json object.
            string jsonData = JsonConvert.SerializeObject(SessionFlagsData, Formatting.Indented);

            // Save the new Json to the session flag.
            this.session["Microsoft365FiddlerExtensionJson"] = jsonData;
        }

        /// <summary>
        /// Analyse the selected sessions in Fiddler. Called from the MenuUI and ContextMenuUI.
        /// </summary>
        public void AnalyseSelectedSessions()
        {
            var Sessions = FiddlerApplication.UI.GetSelectedSessions();

            foreach (var Session in Sessions)
            {
                this.session = Session;

                // If the session already has the Microsoft365FiddlerExtensionJson flag set with high confidence session classifications set,
                // enhance the session based on prior / stored analysis.
                if (GetDeserializedSessionFlags(this.session).SessionAuthenticationConfidenceLevel == 10
                    && GetDeserializedSessionFlags(this.session).SessionResponseServerConfidenceLevel == 10
                    && GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                        $"Enhancing {this.session.id} based on existing session flags ({GetDeserializedSessionFlags(this.session).SessionType}).");

                    EnhanceSessionUX.Instance.EnhanceSession(this.session);
                }
                else
                {
                    SessionService.Instance.OnPeekAtResponseHeaders(this.session);
                }
            }
        }

        /// <summary>
        /// Analyse all sessions loaded in Fiddler. Called from the MenuUI and ContextMenuUI.
        /// </summary>
        /*public void AnalyseAllSessions()
        {
            var Sessions = FiddlerApplication.UI.GetAllSessions();

            foreach (var Session in Sessions)
            {
                this.session = Session;

                // If the session already has the Microsoft365FiddlerExtensionJson flag set with high confidence session classifications set,
                // enhance the session based on prior / stored analysis.
                if (GetDeserializedSessionFlags(this.session).SessionAuthenticationConfidenceLevel == 10
                    && GetDeserializedSessionFlags(this.session).SessionResponseServerConfidenceLevel == 10
                    && GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                        $"Enhancing {this.session.id} based on existing session flags ({GetDeserializedSessionFlags(this.session).SessionType}).");

                    EnhanceSessionUX.Instance.EnhanceSession(this.session);
                }
                else
                {
                    SessionService.Instance.OnPeekAtResponseHeaders(this.session);
                }
            }
        }
        */

        /// <summary>
        /// Clear colourisation and column data fill on selected sessions. Called from the MenuUI and ContextMenuUI.
        /// </summary>
        public void ClearAnalysisSelectedSessions()
        {
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Clearing session procesing from selected sessions.");

            var Sessions = FiddlerApplication.UI.GetSelectedSessions();
            foreach (var Session in Sessions)
            {
                this.session = Session;

                EnhanceSessionUX.Instance.NormaliseSession(this.session);

                this.session["Microsoft365FiddlerExtensionJson"] = null;

                this.session.RefreshUI();
            }
        }

        /// <summary>
        /// Recalculate the Microsoft365FiddlerExtensionJson session flag on selected sessions.
        /// </summary>
        public void CmiRecalculateAnalysisSelectedSessions()
        {
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Recalculating session analysis on selected sessions.");

            var Sessions = FiddlerApplication.UI.GetSelectedSessions();
            foreach (var Session in Sessions)
            {
                this.session = Session;

                EnhanceSessionUX.Instance.NormaliseSession(this.session);
                this.session.RefreshUI();

                this.session["Microsoft365FiddlerExtensionJson"] = null;

                SessionService.Instance.OnPeekAtResponseHeaders(this.session);
            }
        }

        /// <summary>
        /// Take any updates to session flags and save them into the session Json.
        /// Conditional - Use the condition for Session Severity.
        /// Unconditional - Update Session Severity. Used when calling this function fron the context menu and when a lower session severity needs to be set.
        /// </summary>
        /// <param name="Session"></param>
        /// <param name="JsonData"></param>
        /// <param name="unconditional"></param>        
        public void UpdateSessionFlagJson(Session Session, String JsonData, bool unconditional)
        {
            this.session = Session;

            var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            CreateExtensionSessionFlag(this.session);

            var existingSessionFlags = this.session["Microsoft365FiddlerExtensionJson"];
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

            // Host IP
            if (updatedSessionFlagsJson.HostIP == null)
            {
                updatedSessionFlagsJson.HostIP = existingSessionFlagsJson.HostIP;
            }

            // TLS Version
            if (updatedSessionFlagsJson.TLSVersion == null)
            {
                updatedSessionFlagsJson.TLSVersion = existingSessionFlagsJson.TLSVersion;
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

            // Session Severity.

            // If the severity is being set and unconditional is false peform the logic check before allowing it to be updated.
            if (!unconditional)
            {
                if (updatedSessionFlagsJson.SessionSeverity < existingSessionFlagsJson.SessionSeverity)
                {
                    updatedSessionFlagsJson.SessionSeverity = existingSessionFlagsJson.SessionSeverity;
                }
            }
            /*
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} " +
                $"SessionTypeConfidenceLevel set to {updatedSessionFlagsJson.SessionTypeConfidenceLevel}");

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} " +
                $"SessionAuthenticationConfidenceLevel set to {updatedSessionFlagsJson.SessionAuthenticationConfidenceLevel}");

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} " +
                $"SessionResponseServerConfidenceLevel set to {updatedSessionFlagsJson.SessionResponseServerConfidenceLevel}");

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} " +
                $"Session Severity set to {updatedSessionFlagsJson.SessionSeverity}");
            */
            var newJsonData = JsonConvert.SerializeObject(updatedSessionFlagsJson, Formatting.Indented);

            // Save the new Json to the session flag.
            this.session["Microsoft365FiddlerExtensionJson"] = newJsonData;
        }

        public class ExtensionSessionFlags
        {
            public string SectionTitle { get; set; }

            public bool UITextBold { get; set; }

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

            public string HostIP { get; set; }

            public bool SessionTimesInsufficientData { get; set; }

            public int SessionAuthenticationConfidenceLevel { get; set; }

            public int SessionTypeConfidenceLevel { get; set; }

            public int SessionResponseServerConfidenceLevel { get; set; }

            public int SessionSeverity { get; set; }

            public string TLSVersion { get; set; }
        }
    }
}
