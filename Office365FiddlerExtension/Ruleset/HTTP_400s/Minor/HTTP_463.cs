using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_463 : ActivationService
    {
        private static HTTP_463 _instance;

        public static HTTP_463 Instance => _instance ?? (_instance = new HTTP_463());

        public void HTTP_463_X_Forwarded_For_Header(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} HTTP 463 AWS X-Forwarded-For Header > 30 IP addresses.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_463",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "463 AWS X-Forwarded-For Header > 30 IP addresses",
                ResponseCodeDescription = "463 AWS X-Forwarded-For Header > 30 IP addresses",
                ResponseAlert = "HTTP 463 AWS X-Forwarded-For Header > 30 IP addresses.",
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