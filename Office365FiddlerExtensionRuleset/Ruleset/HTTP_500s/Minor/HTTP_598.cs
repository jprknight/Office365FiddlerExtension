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
    class HTTP_598
    {
        internal Session session { get; set; }

        private static HTTP_598 _instance;

        public static HTTP_598 Instance => _instance ?? (_instance = new HTTP_598());

        public void HTTP_598_Network_Read_Timeout_Error(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 598 (Informal convention) Network read timeout error.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_598s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "598 (Informal convention) Network read timeout error",
                ResponseCodeDescription = "598 (Informal convention) Network read timeout error",
                ResponseAlert = "HTTP 598 (Informal convention) Network read timeout error.",
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