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
    class HTTP_418
    {
        internal Session session { get; set; }

        private static HTTP_418 _instance;

        public static HTTP_418 Instance => _instance ?? (_instance = new HTTP_418());

        public void HTTP_418_Im_A_Teapot(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 418 I'm a teapot (RFC 2324, RFC 7168).");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "418 I'm a teapot (RFC 2324, RFC 7168)",
                ResponseCodeDescription = "418 I'm a teapot (RFC 2324, RFC 7168)",
                ResponseAlert = "HTTP 418 I'm a teapot (RFC 2324, RFC 7168).",
                ResponseComments = SessionFlagService.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}