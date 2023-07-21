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
    class HTTP_451
    {
        internal Session session { get; set; }

        private static HTTP_451 _instance;

        public static HTTP_451 Instance => _instance ?? (_instance = new HTTP_451());

        public void HTTP_451_Unavailable_For_Legal_Reasons_or_IIS_Redirect(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 451 Unavailable For Legal Reasons (RFC 7725) or 451 IIS Redirect.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "451 Unavailable For Legal Reasons (RFC 7725) or 451 IIS Redirect",
                ResponseCodeDescription = "451 Unavailable For Legal Reasons (RFC 7725) or 451 IIS Redirect",
                ResponseAlert = "HTTP 451 Unavailable For Legal Reasons (RFC 7725) or 451 IIS Redirect.",
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