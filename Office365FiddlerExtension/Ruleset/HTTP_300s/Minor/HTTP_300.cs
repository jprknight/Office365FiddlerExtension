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
    class HTTP_300 : ActivationService
    {
        private static HTTP_300 _instance;

        public static HTTP_300 Instance => _instance ?? (_instance = new HTTP_300());

        public void HTTP_300_Multiple_Choices(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 300 Multiple Choices.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_300s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "300 Multiple Choices",
                ResponseCodeDescription = "300 Multiple Choices",
                ResponseAlert = "HTTP 300 Multiple Choices.",
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