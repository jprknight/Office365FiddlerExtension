using System;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_504
    {
        internal Session session { get; set; }

        private static HTTP_504 _instance;

        public static HTTP_504 Instance => _instance ?? (_instance = new HTTP_504());

        public void Run(Session session)
        {
            this.session = session;

            HTTP_504_Gateway_Timeout_Internet_Access_Blocked(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }
            HTTP_504_Gateway_Timeout_Anything_Else(this.session);
        }

        private void HTTP_504_Gateway_Timeout_Internet_Access_Blocked(Session session)
        {
            // HTTP 504 Bad Gateway 'internet has been blocked'

            this.session = session;

            if (!(this.session.utilFindInResponse("internet", false) > 1))
            {
                return;
            }

            if (!(this.session.utilFindInResponse("access", false) > 1))
            {
                return;
            }

            if(!(this.session.utilFindInResponse("blocked", false) > 1))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 504 Gateway Timeout -- Internet Access Blocked.");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_504s|HTTP_504_Gateway_Timeout_Internet_Access_Blocked");
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
                SectionTitle = "HTTP_504s",

                SessionType = LangHelper.GetString("HTTP_504_Gateway_Timeout_Internet_Access_Blocked_SessionType"),
                ResponseCodeDescription = LangHelper.GetString("HTTP_504_Gateway_Timeout_Internet_Access_Blocked_ResponseCodeDescription"),
                ResponseAlert = LangHelper.GetString("HTTP_504_Gateway_Timeout_Internet_Access_Blocked_ResponseAlert"),
                ResponseComments = LangHelper.GetString("HTTP_504_Gateway_Timeout_Internet_Access_Blocked_ResponseComments"),

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);            
        }

        private void HTTP_504_Gateway_Timeout_Anything_Else(Session session)
        {
            // Pick up any other 504 Gateway Timeout and write data into the comments box.

            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 504 Gateway Timeout.");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_504s|HTTP_504_Gateway_Timeout_Anything_Else");
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
                SectionTitle = "HTTP_504s",

                SessionType = LangHelper.GetString("HTTP_504_Gateway_Timeout_Anything_Else_SessionType"),
                ResponseCodeDescription = LangHelper.GetString("HTTP_504_Gateway_Timeout_Anything_Else_ResponseCodeDescription"),
                ResponseAlert = LangHelper.GetString("HTTP_504_Gateway_Timeout_Anything_Else_ResponseAlert"),
                ResponseComments = LangHelper.GetString("HTTP_504_Gateway_Timeout_Anything_Else_ResponseComments"),

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
