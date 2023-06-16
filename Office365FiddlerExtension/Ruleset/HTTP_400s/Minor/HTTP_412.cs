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
    class HTTP_412 : ActivationService
    {
        private static HTTP_412 _instance;

        public static HTTP_412 Instance => _instance ?? (_instance = new HTTP_412());

        public void HTTP_412_Precondition_Failed(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} HTTP 412 Precondition Failed (RFC 7232).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_412s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "412 Precondition Failed (RFC 7232)",
                ResponseCodeDescription = "412 Precondition Failed (RFC 7232)",
                ResponseAlert = "HTTP 412 Precondition Failed (RFC 7232).",
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