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
    class HTTP_413 : ActivationService
    {
        private static HTTP_413 _instance;

        public static HTTP_413 Instance => _instance ?? (_instance = new HTTP_413());

        public void HTTP_413_Payload_Too_Large(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} HTTP 413 Payload Too Large (RFC 7231).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_413s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "413 Payload Too Large (RFC 7231)",
                ResponseCodeDescription = "413 Payload Too Large (RFC 7231)",
                ResponseAlert = "HTTP 413 Payload Too Large (RFC 7231).",
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