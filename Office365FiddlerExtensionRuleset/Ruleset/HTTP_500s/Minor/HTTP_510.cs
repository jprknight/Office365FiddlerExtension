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
    class HTTP_510
    {
        internal Session session { get; set; }

        private static HTTP_510 _instance;

        public static HTTP_510 Instance => _instance ?? (_instance = new HTTP_510());

        public void HTTP_510_Not_Extended(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 510 Not Extended (RFC 2774).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_510s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "510 Not Extended (RFC 2774)",
                ResponseCodeDescription = "510 Not Extended (RFC 2774)",
                ResponseAlert = "HTTP 510 Not Extended (RFC 2774).",
                ResponseComments = SessionFlagHandler.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}