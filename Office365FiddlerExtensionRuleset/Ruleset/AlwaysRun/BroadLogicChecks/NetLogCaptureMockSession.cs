using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using Office365FiddlerExtensionRuleset.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class NetLogCaptureMockSession
    {
        internal Session session { get; set; }

        private static NetLogCaptureMockSession _instance;

        public static NetLogCaptureMockSession Instance => _instance ?? (_instance = new NetLogCaptureMockSession());

        /// <summary>
        /// Determine if the current session is a NetLog Capture "Mock" session.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            if (!this.session.host.Equals("NETLOG"))
            {
                return;
            }

            int sessionAuthenticationConfidenceLevel = 0;
            int sessionTypeConfidenceLevel = 0;
            int sessionResponseServerConfidenceLevel = 0;
            int sessionSeverity = 0;

            int sessionAuthenticationConfidenceLevelFallback = 5;
            int sessionTypeConfidenceLevelFallback = 10;
            int sessionResponseServerConfidenceLevelFallback = 5;
            int sessionSeverityFallback = 30;

            try
            {
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("BroadLogicChecks|NetLogCaptureMockSession");

                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"{this.session.id} SESSION CLASSIFICATION EXTERNAL JSON FILE EXCEPTION: {ex}");
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = RulesetLangHelper.GetString("Broad Logic Checks"),

                SessionType = RulesetLangHelper.GetString("BroadLogicChecks_NetLogCaptureMockSession_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("BroadLogicChecks_NetLogCaptureMockSession_ResponseCodeDescription"),
                ResponseServer = RulesetLangHelper.GetString("BroadLogicChecks_NetLogCaptureMockSession_ResponseServer"),
                ResponseAlert = RulesetLangHelper.GetString("BroadLogicChecks_NetLogCaptureMockSession_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("BroadLogicChecks_NetLogCaptureMockSession_ResponseComments"),

                SessionAuthenticationConfidenceLevel = RulesetUtilities.Instance.ValidateSessionAuthenticationConfidenceLevel(sessionAuthenticationConfidenceLevel,
                    sessionAuthenticationConfidenceLevelFallback),

                SessionTypeConfidenceLevel = RulesetUtilities.Instance.ValidateSessionTypeConfidenceLevel(sessionTypeConfidenceLevel,
                    sessionTypeConfidenceLevelFallback),

                SessionResponseServerConfidenceLevel = RulesetUtilities.Instance.ValidateSessionResponseServerConfidenceLevel(sessionResponseServerConfidenceLevel,
                    sessionResponseServerConfidenceLevelFallback),

                SessionSeverity = RulesetUtilities.Instance.ValidateSessionSeverity(sessionSeverity,
                    sessionSeverityFallback)
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
