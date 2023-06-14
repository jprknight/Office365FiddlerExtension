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
    class HTTP_520 : ActivationService
    {
        private static HTTP_520 _instance;

        public static HTTP_520 Instance => _instance ?? (_instance = new HTTP_520());

        public void HTTP_520_Web_Server_Returned_an_Unknown_Error(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 520 Cloudflare Web Server Returned an Unknown Error.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_520s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "520 Cloudflare Web Server Returned an Unknown Error",
                ResponseCodeDescription = "520 Cloudflare Web Server Returned an Unknown Error",
                ResponseAlert = "HTTP 520 Cloudflare Web Server Returned an Unknown Error.",
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