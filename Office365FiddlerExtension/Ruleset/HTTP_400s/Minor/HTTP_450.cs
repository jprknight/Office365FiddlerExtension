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
    class HTTP_450 : ActivationService
    {
        private static HTTP_450 _instance;

        public static HTTP_450 Instance => _instance ?? (_instance = new HTTP_450());

        public void HTTP_450_Blocked_by_Windows_Parental_Controls(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 450 Blocked by Windows Parental Controls (Microsoft).");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_450s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "450 Blocked by Windows Parental Controls (Microsoft)",
                ResponseCodeDescription = "450 Blocked by Windows Parental Controls (Microsoft)",
                ResponseAlert = "HTTP 450 Blocked by Windows Parental Controls (Microsoft).",
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