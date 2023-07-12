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
    class HTTP_520
    {
        internal Session session { get; set; }

        private static HTTP_520 _instance;

        public static HTTP_520 Instance => _instance ?? (_instance = new HTTP_520());

        public void HTTP_520_Web_Server_Returned_an_Unknown_Error(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 520 Cloudflare Web Server Returned an Unknown Error.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_520s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "520 Cloudflare Web Server Returned an Unknown Error",
                ResponseCodeDescription = "520 Cloudflare Web Server Returned an Unknown Error",
                ResponseAlert = "HTTP 520 Cloudflare Web Server Returned an Unknown Error.",
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