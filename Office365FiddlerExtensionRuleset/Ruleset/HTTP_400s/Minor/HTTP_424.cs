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
    class HTTP_424
    {
        internal Session session { get; set; }

        private static HTTP_424 _instance;

        public static HTTP_424 Instance => _instance ?? (_instance = new HTTP_424());

        public void HTTP_424_Failed_Dependency(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 424 Failed Dependency (WebDAV; RFC 4918).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_424s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "424 Failed Dependency (WebDAV; RFC 4918)",
                ResponseCodeDescription = "424 Failed Dependency (WebDAV; RFC 4918)",
                ResponseAlert = "HTTP 424 Failed Dependency (WebDAV; RFC 4918).",
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