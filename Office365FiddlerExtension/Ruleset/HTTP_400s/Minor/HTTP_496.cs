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
    class HTTP_496 : ActivationService
    {
        private static HTTP_496 _instance;

        public static HTTP_496 Instance => _instance ?? (_instance = new HTTP_496());

        public void HTTP_496_SSL_Certificate_Required(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 496 nginx SSL Certificate Required.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_496s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "496 nginx SSL Certificate Required",
                ResponseCodeDescription = "496 nginx SSL Certificate Required",
                ResponseAlert = "HTTP 496 nginx SSL Certificate Required.",
                ResponseComments = SessionProcessor.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}