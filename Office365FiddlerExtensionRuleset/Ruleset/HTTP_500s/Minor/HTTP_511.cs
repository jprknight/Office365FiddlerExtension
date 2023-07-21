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
    class HTTP_511
    {
        internal Session session { get; set; }

        private static HTTP_511 _instance;

        public static HTTP_511 Instance => _instance ?? (_instance = new HTTP_511());

        public void HTTP_511_Network_Authentication_Required(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 511 Network Authentication Required (RFC 6585).");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_511s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "511 Network Authentication Required (RFC 6585)",
                ResponseCodeDescription = "511 Network Authentication Required (RFC 6585)",
                ResponseAlert = "HTTP 511 Network Authentication Required (RFC 6585).",
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