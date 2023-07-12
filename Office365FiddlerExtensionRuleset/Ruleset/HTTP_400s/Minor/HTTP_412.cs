using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtensionRuleset.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_412
    {
        internal Session session { get; set; }

        private static HTTP_412 _instance;

        public static HTTP_412 Instance => _instance ?? (_instance = new HTTP_412());

        public void HTTP_412_Precondition_Failed(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 412 Precondition Failed (RFC 7232).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_412s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "412 Precondition Failed (RFC 7232)",
                ResponseCodeDescription = "412 Precondition Failed (RFC 7232)",
                ResponseAlert = "HTTP 412 Precondition Failed (RFC 7232).",
                ResponseComments = SessionFlagHandler.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}