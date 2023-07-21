using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_525
    {
        internal Session session { get; set; }

        private static HTTP_525 _instance;

        public static HTTP_525 Instance => _instance ?? (_instance = new HTTP_525());

        public void HTTP_525_SSL_Handshake_Failed(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 525 Cloudflare SSL Handshake Failed.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_525s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "525 Cloudflare SSL Handshake Failed",
                ResponseCodeDescription = "525 Cloudflare SSL Handshake Failed",
                ResponseAlert = "HTTP 525 Cloudflare SSL Handshake Failed.",
                ResponseComments = SessionFlagService.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}