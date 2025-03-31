using System;
using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.UI;
using System.Reflection;
using System.Diagnostics;
using System.Linq;

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
                TelemetryService.CustomTrackException(ex);
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
                TLSVersion = "",
                UIColoursSet = false
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

            var sw = Stopwatch.StartNew();

            int SessionsProcessedCount = 0;

            foreach (var Session in Sessions)
            {
                this.session = Session;

                // User interruption of session processing.
                if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().InterruptSessionProcessing)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): LoadSaz processing interrupted by user.");
                    break;
                }

                // If the session already has the Microsoft365FiddlerExtensionJson flag set with high confidence session classifications set,
                // enhance the session based on prior / stored analysis.
                if (GetDeserializedSessionFlags(this.session).SessionAuthenticationConfidenceLevel == 10
                    && GetDeserializedSessionFlags(this.session).SessionResponseServerConfidenceLevel == 10
                    && GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                {
                    EnhanceSessionUX.Instance.EnhanceSession(this.session);
                }
                else
                {
                    SessionService.Instance.OnPeekAtResponseHeaders(this.session);
                }

                SessionsProcessedCount++;

                // Update status bar with load saz progress.
                StatusBar.Instance.UpdateStatusBarOnSessionProgression(SessionsProcessedCount, Sessions.Count());
            }
            
            sw.Stop();

            // Reset the interrupt session processing flag.
            if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().InterruptSessionProcessing)
            {
                SettingsJsonService.Instance.SetInterruptSessionProcessing(false);

            }

            // Update status bar once completed.
            StatusBar.Instance.UpdateStatusBarOnSessionProcessComplete(sw, SessionsProcessedCount);

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                $"Analysed {Sessions.Count()} selected sessions in {sw.ElapsedMilliseconds}ms.");
        }

        /// <summary>
        /// Analyse all sessions loaded in Fiddler. Called from the MenuUI and ContextMenuUI.
        /// </summary>
        public void AnalyseAllSessions()
        {
            var Sessions = FiddlerApplication.UI.GetAllSessions();

            var sw = Stopwatch.StartNew();

            int SessionsProcessedCount = 0;

            foreach (var Session in Sessions)
            {
                this.session = Session;

                // User interruption of session processing.
                if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().InterruptSessionProcessing)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): LoadSaz processing interrupted by user.");
                    break;
                }

                // If the session already has the Microsoft365FiddlerExtensionJson flag set with high confidence session classifications set,
                // enhance the session based on prior / stored analysis.
                if (GetDeserializedSessionFlags(this.session).SessionAuthenticationConfidenceLevel == 10
                    && GetDeserializedSessionFlags(this.session).SessionResponseServerConfidenceLevel == 10
                    && GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                {
                    EnhanceSessionUX.Instance.EnhanceSession(this.session);
                }
                else
                {
                    SessionService.Instance.OnPeekAtResponseHeaders(this.session);
                }

                SessionsProcessedCount++;

                // Update status bar with load saz progress.
                StatusBar.Instance.UpdateStatusBarOnSessionProgression(SessionsProcessedCount, Sessions.Count());
            }

            sw.Stop();

            // Reset the interrupt session processing flag.
            if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().InterruptSessionProcessing)
            {
                SettingsJsonService.Instance.SetInterruptSessionProcessing(false);

            }

            // Update status bar once completed.
            StatusBar.Instance.UpdateStatusBarOnSessionProcessComplete(sw, SessionsProcessedCount);

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                        $"Analysed {Sessions.Count()} all visible sessions in {sw.ElapsedMilliseconds}ms.");
        }

        /// <summary>
        /// Clear colourisation and column data fill on all sessions.
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

                this.session["UI-BACKCOLOR"] = null;
                this.session["UI-COLOR"] = null;

                SetUIColourSet(this.session, false);

                this.session.RefreshUI();
            }
        }

        /// <summary>
        /// Clear colourisation and column data fill on selected sessions.
        /// </summary>
        public void ClearAnalysisAllSessions()
        {
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Clearing session procesing from all sessions.");

            var Sessions = FiddlerApplication.UI.GetAllSessions();
            foreach (var Session in Sessions)
            {
                this.session = Session;

                EnhanceSessionUX.Instance.NormaliseSession(this.session);

                this.session["Microsoft365FiddlerExtensionJson"] = null;

                this.session["UI-BACKCOLOR"] = null;
                this.session["UI-COLOR"] = null;

                SetUIColourSet(this.session, false);

                this.session.RefreshUI();
            }
        }

        public Tuple<bool, int> CheckAllSessionsAreAnalysed()
        {
            int sessionsWithNoAnalysis = 0;

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
                    // Do nothing here.
                }
                else
                {
                    sessionsWithNoAnalysis++;
                }
            }

            if (sessionsWithNoAnalysis == 0)
            {
                return Tuple.Create(true,0);
            }

            return Tuple.Create(false,sessionsWithNoAnalysis);
        }

        /// <summary>
        /// Function to set UIColoursSet session flag true/false. Trying to determine when this.session[ui-color] is null, not null, doesn't
        /// have the right value was too cumbersome. This function is called when the UI colors on sessions are set or cleared.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="_value"></param>
        public void SetUIColourSet(Session session, bool _value)
        {
            this.session = session;

            var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            CreateExtensionSessionFlag(this.session);

            // Pull the existing session flags on the session.
            var sessionFlags = this.session["Microsoft365FiddlerExtensionJson"];
            var sessionFlagsJson = JsonConvert.DeserializeObject<ExtensionSessionFlags>(sessionFlags, JsonSettings);

            // Update the session severity.
            sessionFlagsJson.UIColoursSet = _value;

            var newJsonData = JsonConvert.SerializeObject(sessionFlagsJson, Formatting.Indented);

            // Save the new Json to the session flag.
            this.session["Microsoft365FiddlerExtensionJson"] = newJsonData;
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

        public void SetSessionSeverity(Session session, int _severity)
        {
            this.session = session;

            var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            CreateExtensionSessionFlag(this.session);

            // Pull the existing session flags on the session.
            var sessionFlags = this.session["Microsoft365FiddlerExtensionJson"];
            var sessionFlagsJson = JsonConvert.DeserializeObject<ExtensionSessionFlags>(sessionFlags, JsonSettings);

            // Update the session severity.
            sessionFlagsJson.SessionSeverity = _severity;

            var newJsonData = JsonConvert.SerializeObject(sessionFlagsJson, Formatting.Indented);

            // Save the new Json to the session flag.
            this.session["Microsoft365FiddlerExtensionJson"] = newJsonData;
        }

        public class ExtensionSessionFlags
        {
            public string SectionTitle { get; set; }

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

            public bool UIColoursSet { get; set; }
        }
    }
}
