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
    class HTTP_218
    {
        internal Session session { get; set; }

        private static HTTP_218 _instance;

        public static HTTP_218 Instance => _instance ?? (_instance = new HTTP_218());

        public void HTTP_218_This_Is_Fine_Apache_Web_Server(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 218 This is fine (Apache Web Server).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_218s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "218 This is fine (Apache Web Server)",
                ResponseCodeDescription = "218 This is fine (Apache Web Server)",
                ResponseAlert = "HTTP 218 This is fine (Apache Web Server).",
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