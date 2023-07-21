using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_431
    {
        internal Session session { get; set; }

        private static HTTP_431 _instance;

        public static HTTP_431 Instance => _instance ?? (_instance = new HTTP_431());

        public void HTTP_431_Request_Header_Fields_Too_Large(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 431 Request Header Fields Too Large (RFC 6585).");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_431s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "431 Request Header Fields Too Large (RFC 6585)",
                ResponseCodeDescription = "431 Request Header Fields Too Large (RFC 6585)",
                ResponseAlert = "HTTP 431 Request Header Fields Too Large (RFC 6585).",
                ResponseComments = SessionFlagService.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}