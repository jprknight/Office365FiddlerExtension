using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset.HTTP_200s
{
    class HTTP_200_ClientAccessRule
    {
        internal Session session { get; set; }

        private static HTTP_200_ClientAccessRule _instance;

        public static HTTP_200_ClientAccessRule Instance => _instance ?? (_instance = new HTTP_200_ClientAccessRule());

        public void Run(Session session)
        {
            this.session = session;

            if (!this.session.fullUrl.Contains("outlook.office365.com/mapi"))
            {
                return;
            }

            if (!(RulesetUtilities.Instance.SearchForPhrase(this.session, "Connection blocked by Client Access Rules")))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Connection blocked by Client Access Rules.");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_ClientAccessRule");

                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                sessionAuthenticationConfidenceLevel = 5;
                sessionTypeConfidenceLevel = 10;
                sessionResponseServerConfidenceLevel = 5;
                sessionSeverity = 60;
            }
            
            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s",

                SessionType = LangHelper.GetString("HTTP_200_Client Access Rule"),
                ResponseCodeDescription = LangHelper.GetString("HTTP_200_Client Access Rule ResponseCodeDescription"),
                ResponseAlert = LangHelper.GetString("HTTP_200_Client Access Rule ResponseAlert"),
                ResponseComments = LangHelper.GetString("HTTP_200_Client Access Rule ResponseComments"),

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
