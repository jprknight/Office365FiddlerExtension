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
    class HTTP_200_Exchange_Online_Any_Other_EWS
    {
        internal Session session { get; set; }

        private static HTTP_200_Exchange_Online_Any_Other_EWS _instance;

        public static HTTP_200_Exchange_Online_Any_Other_EWS Instance => _instance ?? (_instance = new HTTP_200_Exchange_Online_Any_Other_EWS());

        /// <summary>
        /// Exchange Online / Microsoft 365 Any Other Exchange Web Services.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            // Any other (Microsoft365 / EXO) EWS call.

            this.session = session;

            // If this isn't an EWS call, return.
            if (!this.session.uriContains("ews/exchange.asmx"))
            {
                return;
            }

            if (!this.session.HostnameIs("outlook.office365.com"))
            {
                return;
            }

            var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP200s|HTTP_200_Microsoft365_Any_Other_EWS");

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 EXO / M365 EWS call.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_Microsoft365_Any_Other_EWS",

                SessionType = "Exchange Online / Microsoft365 Web Services",
                ResponseCodeDescription = "200 OK Microsoft365 Other EWS",
                ResponseAlert = "Exchange Online / Microsoft365 Web Services (EWS) call.",
                ResponseComments = "Exchange Online / Microsoft365 Web Services (EWS) call.",

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
