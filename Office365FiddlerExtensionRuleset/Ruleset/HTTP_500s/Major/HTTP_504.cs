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
    class HTTP_504
    {
        internal Session session { get; set; }

        private static HTTP_504 _instance;

        public static HTTP_504 Instance => _instance ?? (_instance = new HTTP_504());

        public void HTTP_504_Gateway_Timeout_Internet_Access_Blocked(Session session)
        {
            // HTTP 504 Bad Gateway 'internet has been blocked'

            this.session = session;

            if (!(this.session.utilFindInResponse("internet", false) > 1))
            {
                return;
            }

            if (!(this.session.utilFindInResponse("access", false) > 1))
            {
                return;
            }

            if(!(this.session.utilFindInResponse("blocked", false) > 1))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 504 Gateway Timeout -- Internet Access Blocked.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_504s",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "***INTERNET BLOCKED***",
                ResponseCodeDescription = "504 Gateway Timeout - Internet Access Blocked",
                ResponseAlert = "<b><span style='color:red'>HTTP 504 Gateway Timeout -- Internet Access Blocked</span></b>",
                ResponseComments = "Detected the keywords 'internet' and 'access' and 'blocked'. Potentially the computer this trace was collected "
                + "from has been <b><span style='color:red'>quaratined for internet access by a LAN based network security device</span></b>."
                + "<p>Validate this by checking the webview and raw tabs for more information.</p>",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);            
        }

        public void HTTP_504_Gateway_Timeout_Anything_Else(Session session)
        {
            // Pick up any other 504 Gateway Timeout and write data into the comments box.

            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 504 Gateway Timeout.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_504s",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "Gateway Timeout",
                ResponseCodeDescription = "504 Gateway Timeout",
                ResponseAlert = "<b><span style='color:red'>HTTP 504 Gateway Timeout</span></b>",
                ResponseComments = "The quantity of these types of server errors need to be considered in context with what you are troubleshooting "
                + "and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could be cause for concern.",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}