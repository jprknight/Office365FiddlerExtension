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
    class HTTP_521 : ActivationService
    {
        private static HTTP_521 _instance;

        public static HTTP_521 Instance => _instance ?? (_instance = new HTTP_521());

        public void HTTP_521_Web_Server_Is_Down(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 521 Cloudflare Web Server Is Down.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_521s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "521 Cloudflare Web Server Is Down",
                ResponseCodeDescription = "521 Cloudflare Web Server Is Down",
                ResponseAlert = "HTTP 521 Cloudflare Web Server Is Down.",
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