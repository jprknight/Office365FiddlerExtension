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
    class HTTP_422
    {
        internal Session session { get; set; }

        private static HTTP_422 _instance;

        public static HTTP_422 Instance => _instance ?? (_instance = new HTTP_422());

        public void HTTP_422_Unprocessable_Entry(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 422 Unprocessable Entity (WebDAV; RFC 4918).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_422s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "422 Unprocessable Entity (WebDAV; RFC 4918)",
                ResponseCodeDescription = "422 Unprocessable Entity (WebDAV; RFC 4918)",
                ResponseAlert = "HTTP 422 Unprocessable Entity (WebDAV; RFC 4918).",
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