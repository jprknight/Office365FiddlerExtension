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
    class HTTP_505
    {
        internal Session session { get; set; }

        private static HTTP_505 _instance;

        public static HTTP_505 Instance => _instance ?? (_instance = new HTTP_505());

        public void HTTP_505_HTTP_Version_Not_Supported(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 505 HTTP Version Not Supported.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_505s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "505 HTTP Version Not Supported",
                ResponseCodeDescription = "505 HTTP Version Not Supported",
                ResponseAlert = "HTTP 505 HTTP Version Not Supported.",
                ResponseComments = SessionFlagService.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}