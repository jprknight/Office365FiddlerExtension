using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_200_ClientAccessRule
    {
        internal Session session { get; set; }

        private static HTTP_200_ClientAccessRule _instance;

        public static HTTP_200_ClientAccessRule Instance => _instance ?? (_instance = new HTTP_200_ClientAccessRule());

        /// <summary>
        /// Connection blocked by Client Access Rules.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            // If the session content doesn't match the intended rule, return.
            if (!this.session.fullUrl.Contains("outlook.office365.com/mapi"))
            {
                return;
            }

            if (!(this.session.utilFindInResponse("Connection blocked by Client Access Rules", false) > 1))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Connection blocked by Client Access Rules.");

            string sessionSectionTitle;
            string sessionType;
            string sessionResponseCodeDescription;
            string sessionResponseAlert;
            string sessionResponseComments;

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_ClientAccessRule");

                sessionSectionTitle = sessionClassificationJson.SectionTitle;
                sessionType = sessionClassificationJson.SessionType;
                sessionResponseCodeDescription = sessionClassificationJson.SessionResponseCodeDescription;
                sessionResponseAlert = sessionClassificationJson.SessionResponseAlert;
                sessionResponseComments = sessionClassificationJson.SessionResponseComments;

                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                sessionSectionTitle = "HTTP_200s";
                sessionType = "!CLIENT ACCESS RULE!";
                sessionResponseCodeDescription = "200 OK Client Access Rule";
                sessionResponseAlert = "<b><span style='color:red'>CLIENT ACCESS RULE</span></b>";
                sessionResponseComments = "The quantity of these types of server errors need to be considered in context with what you are "
                + "troubleshooting and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could "
                + "be cause for concern."
                + "<p>If you are not seeing expected client traffic, consider if network traces should be collected. Review if there is an underlying "
                + "network issue such as congestion on routers, which could be causing issues. The Network Connection Status Indicator (NCSI) on the "
                + "client computer might also be an area to investigate.</p>";

                sessionAuthenticationConfidenceLevel = 5;
                sessionTypeConfidenceLevel = 10;
                sessionResponseServerConfidenceLevel = 5;
                sessionSeverity = 60;
            }
            
            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = sessionSectionTitle,

                SessionType = sessionType,
                ResponseCodeDescription = sessionResponseCodeDescription,
                ResponseAlert = sessionResponseAlert,
                ResponseComments = sessionResponseComments,

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
