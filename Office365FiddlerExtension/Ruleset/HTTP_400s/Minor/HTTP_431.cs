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
    class HTTP_431 : ActivationService
    {
        private static HTTP_431 _instance;

        public static HTTP_431 Instance => _instance ?? (_instance = new HTTP_431());

        public void HTTP_431_Request_Header_Fields_Too_Large(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} HTTP 431 Request Header Fields Too Large (RFC 6585).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_431s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "431 Request Header Fields Too Large (RFC 6585)",
                ResponseCodeDescription = "431 Request Header Fields Too Large (RFC 6585)",
                ResponseAlert = "HTTP 431 Request Header Fields Too Large (RFC 6585).",
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