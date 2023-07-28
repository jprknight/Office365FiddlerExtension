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
    class HTTP_415
    {
        internal Session session { get; set; }

        private static HTTP_415 _instance;

        public static HTTP_415 Instance => _instance ?? (_instance = new HTTP_415());

        public void HTTP_415_UnSupported_Media_Type(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 415 Unsupported Media Type (RFC 7231).");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "415 Unsupported Media Type (RFC 7231)",
                ResponseCodeDescription = "415 Unsupported Media Type (RFC 7231)",
                ResponseAlert = "HTTP 415 Unsupported Media Type (RFC 7231).",
                ResponseComments = SessionFlagService.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}