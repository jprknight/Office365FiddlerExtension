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
    class HTTP_522 : ActivationService
    {
        private static HTTP_522 _instance;

        public static HTTP_522 Instance => _instance ?? (_instance = new HTTP_522());

        public void HTTP_522_Connection_Timed_Out(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 522 Cloudflare Connection Timed Out.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_522s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "522 Cloudflare Connection Timed Out",
                ResponseCodeDescription = "522 Cloudflare Connection Timed Out",
                ResponseAlert = "HTTP 522 Cloudflare Connection Timed Out.",
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