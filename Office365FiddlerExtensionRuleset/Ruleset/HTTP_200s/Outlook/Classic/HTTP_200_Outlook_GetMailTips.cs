using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using Office365FiddlerExtensionRuleset.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset.HTTP_200s
{
    class HTTP_200_Outlook_GetMailTips
    {
        internal Session session { get; set; }

        private static HTTP_200_Outlook_GetMailTips _instance;

        public static HTTP_200_Outlook_GetMailTips Instance => _instance ?? (_instance = new HTTP_200_Outlook_GetMailTips());

        public void Run(Session session)
        {
            this.session = session;

            if (!this.session.oResponse.headers.ExistsAndContains("x-EwsHandler", "GetMailTips"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " + this.session.id + " HTTP 200 Outlook GetMailTips.");

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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Outlook_GetMailTips");
                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                TelemetryService.CustomTrackException(ex);
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"{this.session.id} SESSION CLASSIFICATION EXTERNAL JSON FILE EXCEPTION: {ex}");
            }

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s",

                SessionType = RulesetLangHelper.GetString("HTTP_200_Outlook_GetMailTips_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_Outlook_GetMailTips_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_200_Outlook_GetMailTips_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_200_Outlook_GetMailTips_ResponseComments"),

                SessionAuthenticationConfidenceLevel = RulesetUtilities.Instance.ValidateSessionAuthenticationConfidenceLevel(sessionAuthenticationConfidenceLevel,
                    sessionAuthenticationConfidenceLevelFallback),

                SessionTypeConfidenceLevel = RulesetUtilities.Instance.ValidateSessionTypeConfidenceLevel(sessionTypeConfidenceLevel,
                    sessionTypeConfidenceLevelFallback),

                SessionResponseServerConfidenceLevel = RulesetUtilities.Instance.ValidateSessionResponseServerConfidenceLevel(sessionResponseServerConfidenceLevel,
                    sessionResponseServerConfidenceLevelFallback),

                SessionSeverity = RulesetUtilities.Instance.ValidateSessionSeverity(sessionSeverity,
                    sessionSeverityFallback)
            };

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Updating session flags.");
            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}
