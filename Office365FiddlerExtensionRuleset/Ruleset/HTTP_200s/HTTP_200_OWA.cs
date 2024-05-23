using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset.HTTP_200s
{
    class HTTP_200_OWA
    {
        internal Session session { get; set; }

        private static HTTP_200_OWA _instance;

        public static HTTP_200_OWA Instance => _instance ?? (_instance = new HTTP_200_OWA());

        /// <summary>
        /// Outlook Web App.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            // If the session isn't OWA traffic, return.
            if (!this.session.uriContains("/owa/"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Outlook Web App.");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Outlook_Web_App");
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
                sessionSeverity = 30;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s",

                SessionType = LangHelper.GetString("HTTP_200_Outlook_Web_App_SessionType"),
                ResponseCodeDescription = LangHelper.GetString("HTTP_200_Outlook_Web_App_ResponseCodeDescription"),
                ResponseAlert = LangHelper.GetString("HTTP_200_Outlook_Web_App_ResponseAlert"),
                ResponseComments = LangHelper.GetString("HTTP_200_Outlook_Web_App_ResponseComments"),

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
