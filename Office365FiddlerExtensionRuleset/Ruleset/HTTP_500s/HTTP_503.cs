using System;
using Office365FiddlerExtensionRuleset.Services;
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

        /// <summary>
        /// Set session analysis values for a HTTP 503 response code.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }
            HTTP_503_Service_Unavailable_OWA_CreateAttachment(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }
            HTTP_503_Service_Unavailable_Everything_Else(this.session);
        }

        private void HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable(Session session)
        {
            this.session = session;

            // 3/19/2024 - SearchForWord works here, SearchForPhrase does not. Going with the easy route.
            if (RulesetUtilities.Instance.SearchForWord(this.session, "FederatedSTSUnreachable") == 0)
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 503 Service Unavailable. FederatedStsUnreachable in response body!");

            int sessionAuthenticationConfidenceLevel = 0;
            int sessionTypeConfidenceLevel = 0;
            int sessionResponseServerConfidenceLevel = 0;
            int sessionSeverity = 0;

            int sessionAuthenticationConfidenceLevelFallback = 5;
            int sessionTypeConfidenceLevelFallback = 10;
            int sessionResponseServerConfidenceLevelFallback = 5;
            int sessionSeverityFallback = 60;

            try
            {
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_503s|HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable");
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

            string RealmURL = "https://login.microsoftonline.com/GetUserRealm.srf?Login=" + this.session.oRequest["X-User-Identity"] + "&xml=1";

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_503s",

                SessionType = RulesetLangHelper.GetString("HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable_ResponseCommentsStart")
                + $"<a href='{RealmURL}' target='_blank'>{RealmURL}</a>"
                + RulesetLangHelper.GetString("HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable_ResponseCommentsEnd"),

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

        private void HTTP_503_Service_Unavailable_OWA_CreateAttachment(Session session)
        {
            this.session = session;

            // If this HTTP 503 session isn't from an OWA CreateAttachment action, return.
            if (!this.session.uriContains("outlook.office.com/owa/service.svc/CreateAttachment"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 503 Service Unavailable. OWA CreatAttachment!");

            int sessionAuthenticationConfidenceLevel = 0;
            int sessionTypeConfidenceLevel = 0;
            int sessionResponseServerConfidenceLevel = 0;
            int sessionSeverity = 0;

            int sessionAuthenticationConfidenceLevelFallback = 5;
            int sessionTypeConfidenceLevelFallback = 10;
            int sessionResponseServerConfidenceLevelFallback = 5;
            int sessionSeverityFallback = 60;

            try
            {
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_503s|HTTP_503_Service_Unavailable_OWA_CreateAttachment");
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

            string RealmURL = "https://login.microsoftonline.com/GetUserRealm.srf?Login=" + this.session.oRequest["X-User-Identity"] + "&xml=1";

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_503s",

                SessionType = RulesetLangHelper.GetString("HTTP_503_Service_Unavailable_OWA_CreateAttachment_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_503_Service_Unavailable_OWA_CreateAttachment_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_503_Service_Unavailable_OWA_CreateAttachment_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_503_Service_Unavailable_OWA_CreateAttachment_ResponseComments"),

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

        private void HTTP_503_Service_Unavailable_Everything_Else(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 503 Service Unavailable.");

            int sessionAuthenticationConfidenceLevel = 0;
            int sessionTypeConfidenceLevel = 0;
            int sessionResponseServerConfidenceLevel = 0;
            int sessionSeverity = 0;

            int sessionAuthenticationConfidenceLevelFallback = 5;
            int sessionTypeConfidenceLevelFallback = 10;
            int sessionResponseServerConfidenceLevelFallback = 5;
            int sessionSeverityFallback = 60;

            try
            {
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_503s|HTTP_503_Service_Unavailable_Everything_Else");
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
                SectionTitle = "HTTP_503s",

                SessionType = RulesetLangHelper.GetString("HTTP_503_Service_Unavailable_Everything_Else_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_503_Service_Unavailable_Everything_Else_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_503_Service_Unavailable_Everything_Else_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_503_Service_Unavailable_Everything_Else_ResponseComments"),

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
