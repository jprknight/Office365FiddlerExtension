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
    class HTTP_418 : ActivationService
    {
        private static HTTP_418 _instance;

        public static HTTP_418 Instance => _instance ?? (_instance = new HTTP_418());

        public void HTTP_418_Im_A_Teapot(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 418 I'm a teapot (RFC 2324, RFC 7168).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "418 I'm a teapot (RFC 2324, RFC 7168)",
                ResponseCodeDescription = "418 I'm a teapot (RFC 2324, RFC 7168)",
                ResponseAlert = "HTTP 418 I'm a teapot (RFC 2324, RFC 7168).",
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