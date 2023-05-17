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
    class HTTP_561 : ActivationService
    {
        private static HTTP_561 _instance;

        public static HTTP_561 Instance => _instance ?? (_instance = new HTTP_561());

        public void HTTP_561_Unauthorized(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 561 AWS Unauthorized.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_561s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "561 AWS Unauthorized",
                ResponseCodeDescription = "561 AWS Unauthorized",
                ResponseAlert = "HTTP 561 AWS Unauthorized.",
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