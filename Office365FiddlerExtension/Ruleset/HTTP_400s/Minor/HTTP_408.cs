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
    class HTTP_408 : ActivationService
    {
        private static HTTP_408 _instance;

        public static HTTP_408 Instance => _instance ?? (_instance = new HTTP_408());

        public void HTTP_408_Request_Timeout(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: " + this.session.id + " HTTP 408 Request Timeout.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_408s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "408 Request Timeout",
                ResponseCodeDescription = "408 Request Timeout",
                ResponseAlert = "HTTP 408 Request Timeout.",
                ResponseComments = "",

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}