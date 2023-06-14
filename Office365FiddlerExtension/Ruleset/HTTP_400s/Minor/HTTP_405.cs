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
    class HTTP_405 : ActivationService
    {
        private static HTTP_405 _instance;

        public static HTTP_405 Instance => _instance ?? (_instance = new HTTP_405());

        public void HTTP_405_Method_Not_Allowed(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 405 Method not allowed.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_405s",
                UIBackColour = "Orange",
                UITextColour = "Black",

                SessionType = "",
                ResponseCodeDescription = "405 Method Not Allowed",
                ResponseAlert = "<b><span style='color:red'>HTTP 405: Method Not Allowed</span></b>",
                ResponseComments = "Was there a GET when only a POST is allowed or vice-versa, or was HTTP tried when HTTPS is required?",

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }
    }
}