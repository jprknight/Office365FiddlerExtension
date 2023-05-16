using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_424 : ActivationService
    {
        private static HTTP_424 _instance;

        public static HTTP_424 Instance => _instance ?? (_instance = new HTTP_424());

        public void HTTP_424_Failed_Dependency(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 424 Failed Dependency (WebDAV; RFC 4918).");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_424s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "424 Failed Dependency (WebDAV; RFC 4918)",
                ResponseCodeDescription = "424 Failed Dependency (WebDAV; RFC 4918)",
                ResponseAlert = "HTTP 424 Failed Dependency (WebDAV; RFC 4918).",
                ResponseComments = SessionProcessor.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}