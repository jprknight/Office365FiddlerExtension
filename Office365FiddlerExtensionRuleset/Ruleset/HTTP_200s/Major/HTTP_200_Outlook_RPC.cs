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
    class HTTP_200_Outlook_RPC
    {
        internal Session session { get; set; }

        private static HTTP_200_Outlook_RPC _instance;

        public static HTTP_200_Outlook_RPC Instance => _instance ?? (_instance = new HTTP_200_Outlook_RPC());

        /// <summary>
        /// Outlook RPC.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            // If the session isn't RPC traffic, return.
            if (!this.session.uriContains("/rpc/emsmdb/"))
            {
                return;
            }

            var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP200s|HTTP_200_Outlook_RPC");

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Outlook RPC traffic break.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_Outlook_RPC",

                SessionType = "Outlook RPC",
                ResponseCodeDescription = "200 OK Outlook over RPC",
                ResponseAlert = "Outlook for Windows RPC traffic",
                ResponseComments = "This is normal Outlook RPC over HTTP traffic to an Exchange On-Premise mailbox.",

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
