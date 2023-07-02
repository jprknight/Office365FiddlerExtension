using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtensionRuleset.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_497
    {
        internal Session session { get; set; }

        private static HTTP_497 _instance;

        public static HTTP_497 Instance => _instance ?? (_instance = new HTTP_497());

        public void HTTP_497_Request_Sent_To_HTTPS_Port(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 497 nginx HTTP Request Sent to HTTPS Port.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_497s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "497 nginx HTTP Request Sent to HTTPS Port",
                ResponseCodeDescription = "497 nginx HTTP Request Sent to HTTPS Port",
                ResponseAlert = "HTTP 497 nginx HTTP Request Sent to HTTPS Port.",
                ResponseComments = SessionFlagHandler.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}