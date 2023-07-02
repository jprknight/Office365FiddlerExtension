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
    class HTTP_203
    {
        internal Session session { get; set; }

        private static HTTP_203 _instance;

        public static HTTP_203 Instance => _instance ?? (_instance = new HTTP_203());

        public void HTTP_203_NonAuthoritive_Answer(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 203 Non-Authoritative Information.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_203s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "HTTP_203s",
                ResponseCodeDescription = "203 Non-Authoritative Information",
                ResponseAlert = "203 Non-Authoritative Information.",
                ResponseComments = "203 Non-Authoritative Information.",

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}