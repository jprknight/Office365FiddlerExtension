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
    class HTTP_511 : ActivationService
    {
        private static HTTP_511 _instance;

        public static HTTP_511 Instance => _instance ?? (_instance = new HTTP_511());

        public void HTTP_511_Network_Authentication_Required(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} HTTP 511 Network Authentication Required (RFC 6585).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_511s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "511 Network Authentication Required (RFC 6585)",
                ResponseCodeDescription = "511 Network Authentication Required (RFC 6585)",
                ResponseAlert = "HTTP 511 Network Authentication Required (RFC 6585).",
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