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
    class HTTP_308
    {
        internal Session session { get; set; }

        private static HTTP_308 _instance;

        public static HTTP_308 Instance => _instance ?? (_instance = new HTTP_308());

        public void HTTP_308_Permenant_Redirect(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 308 Permanent Redirect (RFC 7538).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_308s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "HTTP 308 Permanent Redirect (RFC 7538)",
                ResponseCodeDescription = "HTTP 308 Permanent Redirect (RFC 7538)",
                ResponseAlert = "HTTP 308 Permanent Redirect (RFC 7538).",
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