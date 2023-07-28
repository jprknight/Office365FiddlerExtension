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

            var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP200s|HTTP_200_ClientAccessRule");

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Connection blocked by Client Access Rules.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_Client_Access_Rule",

                SessionType = "!CLIENT ACCESS RULE!",
                ResponseCodeDescription = "200 OK Client Access Rule",
                ResponseAlert = "<b><span style='color:red'>CLIENT ACCESS RULE</span></b>",
                ResponseComments = "<b><span style='color:red'>A client access rule has blocked MAPI connectivity to the mailbox</span></b>. "
                + "<p>Check if the <b><span style='color:red'>client access rule includes OutlookAnywhere</span></b>.</p>"
                + "<p>Per <a href='https://docs.microsoft.com/en-us/exchange/clients-and-mobile-in-exchange-online/client-access-rules/client-access-rules' target='_blank'>"
                + "https://docs.microsoft.com/en-us/exchange/clients-and-mobile-in-exchange-online/client-access-rules/client-access-rules </a>, <br />"
                + "OutlookAnywhere includes MAPI over HTTP.<p>"
                + "<p>Remove OutlookAnywhere from the client access rule, wait 1 hour, then test again.</p>",

                SessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel,
                SessionSeverity = sessionClassificationJson.SessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);

        }
    }
}
