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

        /// <summary>
        /// Set session analysis values when no response code is seen.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            HTTP_0_NoSessionResponse(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }
            HTTP_0_OWA_Notification_Channel(this.session);
        }

        private void HTTP_0_NoSessionResponse(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} HTTP 0 No response.");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP0s");

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

                sessionAuthenticationConfidenceLevel = 10;
                sessionTypeConfidenceLevel = 10;
                sessionResponseServerConfidenceLevel = 10;
                sessionSeverity = 60;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_0",

                SessionType = LangHelper.GetString("HTTP_0 SessionType"),
                ResponseServer = LangHelper.GetString("HTTP_0 ResonseServer"),
                ResponseAlert = LangHelper.GetString("HTTP_0 Response Alert"),
                ResponseCodeDescription = LangHelper.GetString("HTTP_0 ResponseCodeDescription"),
                ResponseComments = LangHelper.GetString("HTTP_0 ResponseComments"),
                Authentication = LangHelper.GetString("HTTP_0 Authentication"),

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        private void HTTP_0_OWA_Notification_Channel(Session session)
        {
            this.session = session;

            // If the session isn't for OWA notification channel, return.
            if (!this.session.uriContains("/owa/notificationchannel/"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 0 Outlook Web App Notification Channel.");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_0s|HTTP_0_Outlook_Web_App_Notification_Channel");
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
                SectionTitle = "HTTP_0s",

                SessionType = LangHelper.GetString("HTTP_0_Outlook_Web_App_Notification_Channel_SessionType"),
                ResponseCodeDescription = LangHelper.GetString("HTTP_0_Outlook_Web_App_Notification_Channel_ResponseCodeDescription"),
                ResponseAlert = LangHelper.GetString("HTTP_0_Outlook_Web_App_Notification_Channel_ResponseAlert"),
                ResponseComments = LangHelper.GetString("HTTP_0_Outlook_Web_App_Notification_Channel_ResponseComments"),

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
