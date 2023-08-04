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
    class HTTP_509
    {
        internal Session session { get; set; }

        private static HTTP_509 _instance;

        public static HTTP_509 Instance => _instance ?? (_instance = new HTTP_509());

        public void HTTP_509_Bandwidth_Limit_Exceeeded(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 509 Bandwidth Limit Exceeded (Apache Web Server/cPanel).");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_509s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "509 Bandwidth Limit Exceeded (Apache Web Server/cPanel)",
                ResponseCodeDescription = "509 Bandwidth Limit Exceeded (Apache Web Server/cPanel)",
                ResponseAlert = "HTTP 509 Bandwidth Limit Exceeded (Apache Web Server/cPanel).",
                ResponseComments = SessionFlagService.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}