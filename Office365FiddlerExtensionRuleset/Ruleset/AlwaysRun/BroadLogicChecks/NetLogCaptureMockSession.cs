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

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

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
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} {ex}");

                sessionAuthenticationConfidenceLevel = 5;
                sessionTypeConfidenceLevel = 10;
                sessionResponseServerConfidenceLevel = 5;
                sessionSeverity = 30;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = RulesetLangHelper.GetString("Broad Logic Checks"),

                SessionType = RulesetLangHelper.GetString("BroadLogicChecks_NetLogCaptureMockSession_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("BroadLogicChecks_NetLogCaptureMockSession_ResponseCodeDescription"),
                ResponseServer = RulesetLangHelper.GetString("BroadLogicChecks_NetLogCaptureMockSession_ResponseServer"),
                ResponseAlert = RulesetLangHelper.GetString("BroadLogicChecks_NetLogCaptureMockSession_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("BroadLogicChecks_NetLogCaptureMockSession_ResponseComments"),                   

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
