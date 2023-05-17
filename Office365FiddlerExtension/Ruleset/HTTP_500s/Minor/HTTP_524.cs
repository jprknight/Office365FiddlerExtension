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
    class HTTP_524 : ActivationService
    {
        private static HTTP_524 _instance;

        public static HTTP_524 Instance => _instance ?? (_instance = new HTTP_524());

        public void HTTP_524_A_Timeout_Occurred(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 524 Cloudflare A Timeout Occurred.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_524s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "524 Cloudflare A Timeout Occurred",
                ResponseCodeDescription = "524 Cloudflare A Timeout Occurred",
                ResponseAlert = "HTTP 524 Cloudflare A Timeout Occurred.",
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