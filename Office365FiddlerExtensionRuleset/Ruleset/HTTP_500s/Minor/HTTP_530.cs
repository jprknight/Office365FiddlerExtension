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
    class HTTP_530
    {
        internal Session session { get; set; }

        private static HTTP_530 _instance;

        public static HTTP_530 Instance => _instance ?? (_instance = new HTTP_530());

        public void HTTP_530_Site_Is_Frozen(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 530 Site is frozen or Cloudflare Error returned with 1xxx error.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_530s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "530 Site is frozen or Cloudflare Error returned with 1xxx error",
                ResponseCodeDescription = "530 Site is frozen or Cloudflare Error returned with 1xxx error",
                ResponseAlert = "HTTP 530 Site is frozen or Cloudflare Error returned with 1xxx error.",
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