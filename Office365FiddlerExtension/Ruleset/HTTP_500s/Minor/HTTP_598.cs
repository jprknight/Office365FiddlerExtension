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
    class HTTP_598 : ActivationService
    {
        private static HTTP_598 _instance;

        public static HTTP_598 Instance => _instance ?? (_instance = new HTTP_598());

        public void HTTP_598_Network_Read_Timeout_Error(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 598 (Informal convention) Network read timeout error.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_598s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "598 (Informal convention) Network read timeout error",
                ResponseCodeDescription = "598 (Informal convention) Network read timeout error",
                ResponseAlert = "HTTP 598 (Informal convention) Network read timeout error.",
                ResponseComments = SessionFlagHandler.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }
    }
}