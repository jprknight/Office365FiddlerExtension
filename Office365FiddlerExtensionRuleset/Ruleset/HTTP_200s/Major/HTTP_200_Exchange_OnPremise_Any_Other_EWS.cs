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
    class HTTP_200_Exchange_OnPremise_Any_Other_EWS
    {
        internal Session session { get; set; }

        private static HTTP_200_Exchange_OnPremise_Any_Other_EWS _instance;

        public static HTTP_200_Exchange_OnPremise_Any_Other_EWS Instance => _instance ?? (_instance = new HTTP_200_Exchange_OnPremise_Any_Other_EWS());

        /// <summary>
        /// Exchange OnPremise Any Other Exchange Web Services.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            // Any other EWS call.
            // Note: There are some organizations who have vanity domains for Office 365. They are the outliers for this scenario.

            this.session = session;

            // If this isn't an EWS call, return.
            if (!this.session.uriContains("ews/exchange.asmx"))
            {
                return;
            }

            if (this.session.HostnameIs("outlook.office365.com"))
            {
                return;
            }

            var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP200s|HTTP_200_OnPremise_Any_Other_EWS");

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " + this.session.id + " HTTP 200 OnPremise EWS call.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_OnPremise_Exchange_EWS",

                SessionType = "Exchange OnPremise Web Services",
                ResponseCodeDescription = "200 OK Exchange Web Services / EWS",
                ResponseAlert = "Exchange OnPremise Web Services (EWS) call.",
                ResponseComments = "Exchange OnPremise Web Services (EWS) call.",

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
