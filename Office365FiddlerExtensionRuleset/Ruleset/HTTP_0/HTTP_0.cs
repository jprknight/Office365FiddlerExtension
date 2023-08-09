using Fiddler;
using Newtonsoft.Json;
using System.Reflection;
using Office365FiddlerExtension.Services;
using System;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_0
    {
        internal Session session { get; set; }

        private static HTTP_0 _instance;

        public static HTTP_0 Instance => _instance ?? (_instance = new HTTP_0());

        public void HTTP_0_NoSessionResponse(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 0 No response.");

            string sessionSectionTitle;
            string sessionType;
            string sessionResponseCodeDescription;
            string sessionResonseServer;
            string sessionResponseAlert;
            string sessionResponseComments;
            string sessionAuthentication;

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP0s");

                sessionSectionTitle = sessionClassificationJson.SectionTitle;
                sessionType = sessionClassificationJson.SessionType;
                sessionResponseCodeDescription = sessionClassificationJson.SessionResponseCodeDescription;
                sessionResonseServer = sessionClassificationJson.SessionResponseServer;
                sessionResponseAlert = sessionClassificationJson.SessionResponseAlert;
                sessionResponseComments = sessionClassificationJson.SessionResponseComments;
                sessionAuthentication = sessionClassificationJson.SessionAuthentication;

                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                sessionSectionTitle = "HTTP_0s";
                sessionType = "!NO RESPONSE!";
                sessionResponseCodeDescription = "0 No Response";
                sessionResonseServer = "!NO RESPONSE!";
                sessionResponseAlert = "<b><span style='color:red'>HTTP 0 - No Response</span></b>";
                sessionResponseComments = "The quantity of these types of server errors need to be considered in context with what you are "
                + "troubleshooting and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could "
                + "be cause for concern."
                + "<p>If you are not seeing expected client traffic, consider if network traces should be collected. Review if there is an underlying "
                + "network issue such as congestion on routers, which could be causing issues. The Network Connection Status Indicator (NCSI) on the "
                + "client computer might also be an area to investigate.</p>";
                sessionAuthentication = "!NO RESPONSE!";

                sessionAuthenticationConfidenceLevel = 10;
                sessionTypeConfidenceLevel = 10;
                sessionResponseServerConfidenceLevel = 10;
                sessionSeverity = 60;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = sessionSectionTitle,

                SessionType = sessionType,
                ResponseServer = sessionResonseServer,
                ResponseAlert = sessionResponseAlert,
                ResponseCodeDescription = sessionResponseCodeDescription,
                ResponseComments = sessionResponseComments,
                Authentication = sessionAuthentication,

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