using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_497 : ActivationService
    {
        private static HTTP_497 _instance;

        public static HTTP_497 Instance => _instance ?? (_instance = new HTTP_497());

        public void HTTP_497_Request_Sent_To_HTTPS_Port(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 497 nginx HTTP Request Sent to HTTPS Port.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_497s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "497 nginx HTTP Request Sent to HTTPS Port",
                ResponseCodeDescription = "497 nginx HTTP Request Sent to HTTPS Port",
                ResponseAlert = "HTTP 497 nginx HTTP Request Sent to HTTPS Port.",
                ResponseComments = SessionProcessor.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}