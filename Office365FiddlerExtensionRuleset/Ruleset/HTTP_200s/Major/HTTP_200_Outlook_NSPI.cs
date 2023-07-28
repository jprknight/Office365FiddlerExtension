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
    class HTTP_200_Outlook_NSPI
    {
        internal Session session { get; set; }

        private static HTTP_200_Outlook_NSPI _instance;

        public static HTTP_200_Outlook_NSPI Instance => _instance ?? (_instance = new HTTP_200_Outlook_NSPI());

        /// <summary>
        /// Outlook Name Service Provider Interface (NSPI) traffic.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            // If this isn't NSPI traffic, return.
            if (!this.session.uriContains("/mapi/nspi/"))
            {
                return;
            }

            var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP200s|HTTP_200_Outlook_NSPI");

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " + this.session.id + " HTTP 200 Outlook NSPI traffic.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_Outlook_NSPI",

                SessionType = "Outlook NSPI",
                ResponseCodeDescription = "200 OK Outlook NSPI",
                ResponseAlert = "Outlook for Windows NSPI traffic",
                ResponseComments = "This is normal Outlook traffic to an Exchange On-Premise mailbox. Name Service Provider Interface (NSPI).",

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
