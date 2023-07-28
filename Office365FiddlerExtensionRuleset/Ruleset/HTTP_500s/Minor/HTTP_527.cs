using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;
using Office365FiddlerExtension.Services;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_527
    {
        internal Session session { get; set; }

        private static HTTP_527 _instance;

        public static HTTP_527 Instance => _instance ?? (_instance = new HTTP_527());

        public void HTTP_527_Railgun_Error(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 527 Cloudflare Railgun Error.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_527s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "527 Cloudflare Railgun Error",
                ResponseCodeDescription = "527 Cloudflare Railgun Error",
                ResponseAlert = "HTTP 527 Cloudflare Railgun Error.",
                ResponseComments = SessionFlagService.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}