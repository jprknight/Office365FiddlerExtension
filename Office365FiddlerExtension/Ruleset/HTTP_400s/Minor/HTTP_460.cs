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
    class HTTP_460 : ActivationService
    {
        private static HTTP_460 _instance;

        public static HTTP_460 Instance => _instance ?? (_instance = new HTTP_460());

        public void HTTP_460_Load_Balancer_Timeout(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 460 AWS Load balancer Timeout.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_460s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "460 AWS Load balancer Timeout",
                ResponseCodeDescription = "460 AWS Load balancer Timeout",
                ResponseAlert = "HTTP 460 AWS Load balancer Timeout.",
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