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
    class HTTP_200_Outlook_MAPI_Exchange_OnPremise
    {
        internal Session session { get; set; }

        private static HTTP_200_Outlook_MAPI_Exchange_OnPremise _instance;

        public static HTTP_200_Outlook_MAPI_Exchange_OnPremise Instance => _instance ?? (_instance = new HTTP_200_Outlook_MAPI_Exchange_OnPremise());

        /// <summary>
        /// Exchange On-Premise Mailbox MAPI
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            // If the session isn't MAPI traffic, return.
            if (!this.session.uriContains("/mapi/emsmdb/?MailboxId="))
            {
                return;
            }

            var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP200s|HTTP_200_Outlook_Exchange_OnPremise_Mapi");

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Outlook Exchange OnPremise MAPI traffic.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_Exchange_OnPremise_Mapi",

                SessionType = "Outlook MAPI",
                ResponseCodeDescription = "200 OK Exchange MAPI",
                ResponseAlert = "Outlook for Windows MAPI traffic",
                ResponseComments = "This is normal Outlook MAPI over HTTP traffic to an Exchange OnPremise mailbox.",

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
