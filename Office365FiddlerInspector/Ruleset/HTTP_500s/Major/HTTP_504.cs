using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_504 : ActivationService
    {
        private static HTTP_504 _instance;

        public static HTTP_504 Instance => _instance ?? (_instance = new HTTP_504());

        public void HTTP_504_Gateway_Timeout_Internet_Access_Blocked(Session session)
        {
            this.session = session;

            /////////////////////////////
            // 504.1. HTTP 504 Bad Gateway 'internet has been blocked'
            if ((this.session.utilFindInResponse("access", false) > 1) &&
                (this.session.utilFindInResponse("internet", false) > 1) &&
                (this.session.utilFindInResponse("blocked", false) > 1))
            {
                GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, " HTTP 504 Gateway Timeout -- Internet Access Blocked.");

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Red");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "504 Gateway Timeout - Internet Access Blocked");

                GetSetSessionFlags.Instance.SetSessionType(this.session, "***INTERNET BLOCKED***");

                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:red'>HTTP 504 Gateway Timeout -- Internet Access Blocked</span></b>");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "Detected the keywords 'internet' and 'access' and 'blocked'. Potentially the computer this trace was collected "
                    + "from has been <b><span style='color:red'>quaratined for internet access by a LAN based network security device</span></b>."
                    + "<p>Validate this by checking the webview and raw tabs for more information.</p>");

                // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
        }

        public void HTTP_504_Gateway_Timeout_Anything_Else(Session session)
        {
            this.session = session;

            /////////////////////////////
            // 504.99. Pick up any other 504 Gateway Timeout and write data into the comments box.
            GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "HTTP 504 Gateway Timeout (99).");

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Red");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "504 Gateway Timeout");

            GetSetSessionFlags.Instance.SetXResponseComments(this.session, "<b><span style='color:red'>HTTP 504 Gateway Timeout</span></b>");
            GetSetSessionFlags.Instance.SetXResponseComments(this.session, "The quantity of these types of server errors need to be considered in context with what you are troubleshooting "
                + "and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could be cause for concern.");

            GetSetSessionFlags.Instance.SetSessionType(this.session, "Gateway Timeout");

            // Nothing meaningful here, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}