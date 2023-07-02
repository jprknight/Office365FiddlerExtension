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
    class HTTP_208
    {
        internal Session session { get; set; }

        private static HTTP_208 _instance;

        public static HTTP_208 Instance => _instance ?? (_instance = new HTTP_208());

        public void HTTP_208_Already_Reported(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 208 Already Reported (WebDAV; RFC 5842).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_208s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "208 Already Reported (WebDAV; RFC 5842)",
                ResponseCodeDescription = "208 Already Reported (WebDAV; RFC 5842)",
                ResponseAlert = "208 Already Reported (WebDAV; RFC 5842).",
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