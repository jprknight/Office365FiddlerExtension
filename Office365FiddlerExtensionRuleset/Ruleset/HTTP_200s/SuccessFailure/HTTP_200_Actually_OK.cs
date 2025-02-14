using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtensionRuleset.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset.HTTP_200s
{
    class HTTP_200_Actually_OK
    {
        internal Session session { get; set; }

        private static HTTP_200_Actually_OK _instance;

        public static HTTP_200_Actually_OK Instance => _instance ?? (_instance = new HTTP_200_Actually_OK());

        public void Run(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} HTTP 200 OK");

            if (RulesetUtilities.Instance.SearchForWord(this.session, "Error") == 0 &&
                RulesetUtilities.Instance.SearchForWord(this.session, "failed") == 0 &&
                RulesetUtilities.Instance.SearchForWord(this.session, "exception") == 0)
            {
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
                    var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Actually_OK");
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

                var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s",

                    SessionType = RulesetLangHelper.GetString("HTTP_200_Actually_OK SessionType"),
                    ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_Actually_OK ResponseCodeDescription"),
                    ResponseAlert = RulesetLangHelper.GetString("HTTP_200_Actually_OK ResponseAlert"),
                    ResponseComments = RulesetLangHelper.GetString("HTTP_200_Actually_OK ResponseComments"),

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
                RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
        }
    }
}
