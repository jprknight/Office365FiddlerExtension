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
    class HTTP_498 : ActivationService
    {
        private static HTTP_498 _instance;

        public static HTTP_498 Instance => _instance ?? (_instance = new HTTP_498());

        public void HTTP_498_Invalid_Token(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 498 Invalid Token (Esri).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_498s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "498 Invalid Token (Esri)",
                ResponseCodeDescription = "498 Invalid Token (Esri)",
                ResponseAlert = "HTTP 498 Invalid Token (Esri).",
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