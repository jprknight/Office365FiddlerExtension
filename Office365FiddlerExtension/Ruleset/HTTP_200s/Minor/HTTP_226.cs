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
    class HTTP_226 : ActivationService
    {
        private static HTTP_226 _instance;

        public static HTTP_226 Instance => _instance ?? (_instance = new HTTP_226());

        public void HTTP_226_IM_Used(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: " + this.Session.id + " HTTP 226 IM Used (RFC 3229).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_226s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "HTTP_226s",
                ResponseCodeDescription = "226 IM Used (RFC 3229)",
                ResponseAlert = "226 IM Used (RFC 3229).",
                ResponseComments = "226 IM Used (RFC 3229).",

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }
    }
}