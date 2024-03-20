using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_200_Outlook_MAPI_Protocol_Disabled
    {
        internal Session session { get; set; }

        private static HTTP_200_Outlook_MAPI_Protocol_Disabled _instance;

        public static HTTP_200_Outlook_MAPI_Protocol_Disabled Instance => _instance ?? (_instance = new HTTP_200_Outlook_MAPI_Protocol_Disabled());

        /// <summary>
        /// Microsoft365 Outlook MAPI traffic, protocol disabled.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            // If this isn't Office 365 MAPI traffic, return.
            if (!this.session.HostnameIs("outlook.office365.com") && (!this.session.uriContains("/mapi/emsmdb/?MailboxId=")))
            {
                return;
            }

            // If we don't find "ProtocolDisabled" in the response body, return.
            if (!(this.session.utilFindInResponse("ProtocolDisabled", false) > 1))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Store Error Protocol Disabled.");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Outlook_Mapi_Microsoft365_Protocol_Disabled");
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

                SessionType = LangHelper.GetString("HTTP_200_Outlook_Mapi_Microsoft365_Protocol_Disabled_SessionType"),
                ResponseCodeDescription = LangHelper.GetString("HTTP_200_Outlook_Mapi_Microsoft365_Protocol_Disabled_ResponseCodeDescription"),
                ResponseAlert = LangHelper.GetString("HTTP_200_Outlook_Mapi_Microsoft365_Protocol_Disabled_ResponseAlert"),
                ResponseComments = LangHelper.GetString("HTTP_200_Outlook_Mapi_Microsoft365_Protocol_Disabled_ResponseComments"),

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
