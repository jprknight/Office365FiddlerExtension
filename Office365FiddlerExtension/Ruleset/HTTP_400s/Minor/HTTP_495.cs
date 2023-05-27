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
    class HTTP_495 : ActivationService
    {
        private static HTTP_495 _instance;

        public static HTTP_495 Instance => _instance ?? (_instance = new HTTP_495());

        public void HTTP_495_SSL_Certificate_Error(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 495 nginx SSL Certificate Error.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_495s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "495 nginx SSL Certificate Error",
                ResponseCodeDescription = "495 nginx SSL Certificate Error",
                ResponseAlert = "HTTP 495 nginx SSL Certificate Error.",
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