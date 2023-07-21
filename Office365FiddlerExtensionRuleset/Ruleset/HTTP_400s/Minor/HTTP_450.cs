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
    class HTTP_450
    {
        internal Session session { get; set; }

        private static HTTP_450 _instance;

        public static HTTP_450 Instance => _instance ?? (_instance = new HTTP_450());

        public void HTTP_450_Blocked_by_Windows_Parental_Controls(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 450 Blocked by Windows Parental Controls (Microsoft).");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_450s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "450 Blocked by Windows Parental Controls (Microsoft)",
                ResponseCodeDescription = "450 Blocked by Windows Parental Controls (Microsoft)",
                ResponseAlert = "HTTP 450 Blocked by Windows Parental Controls (Microsoft).",
                ResponseComments = SessionFlagService.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}