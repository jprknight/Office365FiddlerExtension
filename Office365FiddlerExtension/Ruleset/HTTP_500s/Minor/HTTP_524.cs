using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_524 : ActivationService
    {
        private static HTTP_524 _instance;

        public static HTTP_524 Instance => _instance ?? (_instance = new HTTP_524());

        public void HTTP_524_A_Timeout_Occurred(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} HTTP 524 Cloudflare A Timeout Occurred.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_524s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "524 Cloudflare A Timeout Occurred",
                ResponseCodeDescription = "524 Cloudflare A Timeout Occurred",
                ResponseAlert = "HTTP 524 Cloudflare A Timeout Occurred.",
                ResponseComments = SessionFlagHandler.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }
    }
}