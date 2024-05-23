using System;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_503
    {
        internal Session session { get; set; }

        private static HTTP_503 _instance;

        public static HTTP_503 Instance => _instance ?? (_instance = new HTTP_503());

        public void Run(Session session)
        {
            this.session = session;

            HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }
            HTTP_503_Service_Unavailable_Everything_Else(this.session);
        }

        private void HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable(Session session)
        {
            this.session = session;

            // 3/19/2024 - SearchForWord works here, SearchForPhrase does not. Going with the easy route.
            if (SessionContentSearch.Instance.SearchForWord(this.session, "FederatedSTSUnreachable") == 0)
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 503 Service Unavailable. FederatedStsUnreachable in response body!");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_503s|HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable");
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
            
            string RealmURL = "https://login.microsoftonline.com/GetUserRealm.srf?Login=" + this.session.oRequest["X-User-Identity"] + "&xml=1";

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_503s",

                SessionType = LangHelper.GetString("HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable_SessionType"),
                ResponseCodeDescription = LangHelper.GetString("HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable_ResponseCodeDescription"),
                ResponseAlert = LangHelper.GetString("HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable_ResponseAlert"),
                ResponseComments = LangHelper.GetString("HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable_ResponseCommentsStart")
                + $"<a href='{RealmURL}' target='_blank'>{RealmURL}</a>"
                + LangHelper.GetString("HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable_ResponseCommentsEnd"),

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);           
        }

        private void HTTP_503_Service_Unavailable_Everything_Else(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 503 Service Unavailable.");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_503s|HTTP_503_Service_Unavailable_Everything_Else");
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
                SectionTitle = "HTTP_503s",

                SessionType = LangHelper.GetString("HTTP_503_Service_Unavailable_Everything_Else_SessionType"),
                ResponseCodeDescription = LangHelper.GetString("HTTP_503_Service_Unavailable_Everything_Else_ResponseCodeDescription"),
                ResponseAlert = LangHelper.GetString("HTTP_503_Service_Unavailable_Everything_Else_ResponseAlert"),
                ResponseComments = LangHelper.GetString("HTTP_503_Service_Unavailable_Everything_Else_ResponseComments"),

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
