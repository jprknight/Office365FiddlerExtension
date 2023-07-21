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
    class HTTP_304
    {
        internal Session session { get; set; }

        private static HTTP_304 _instance;

        public static HTTP_304 Instance => _instance ?? (_instance = new HTTP_304());

        public void HTTP_304_Not_Modified(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 304 Not modified.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_304s",
                UIBackColour = "Green",
                UITextColour = "Black",

                SessionType = "304 Not Modified (RFC 7232)",
                ResponseCodeDescription = "304 Not Modified (RFC 7232)",
                ResponseAlert = "304 Not Modified (RFC 7232).",
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