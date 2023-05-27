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
    class HTTP_527 : ActivationService
    {
        private static HTTP_527 _instance;

        public static HTTP_527 Instance => _instance ?? (_instance = new HTTP_527());


        public void HTTP_527_Railgun_Error(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 527 Cloudflare Railgun Error.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_527s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "527 Cloudflare Railgun Error",
                ResponseCodeDescription = "527 Cloudflare Railgun Error",
                ResponseAlert = "HTTP 527 Cloudflare Railgun Error.",
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