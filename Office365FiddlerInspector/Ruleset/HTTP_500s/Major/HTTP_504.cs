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
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        public void HTTP_504_Gateway_Timeout_Internet_Access_Blocked(Session session)
        {
            this.session = session;

            /////////////////////////////
            // 504.1. HTTP 504 Bad Gateway 'internet has been blocked'
            if ((this.session.utilFindInResponse("access", false) > 1) &&
                (this.session.utilFindInResponse("internet", false) > 1) &&
                (this.session.utilFindInResponse("blocked", false) > 1))
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + "  HTTP 504 Gateway Timeout -- Internet Access Blocked.");

                getSetSessionFlags.SetUIBackColour(this.session, "Red");
                getSetSessionFlags.SetUITextColour(this.session, "Black");

                getSetSessionFlags.SetResponseCodeDescription(this.session, "504 Gateway Timeout - Internet Access Blocked");

                getSetSessionFlags.SetSessionType(this.session, "***INTERNET BLOCKED***");

                getSetSessionFlags.SetXResponseAlert(this.session, "<b><span style='color:red'>HTTP 504 Gateway Timeout -- Internet Access Blocked</span></b>");
                getSetSessionFlags.SetXResponseComments(this.session, "Detected the keywords 'internet' and 'access' and 'blocked'. Potentially the computer this trace was collected "
                    + "from has been <b><span style='color:red'>quaratined for internet access by a LAN based network security device</span></b>."
                    + "<p>Validate this by checking the webview and raw tabs for more information.</p>");

                // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
                getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
        }

        public void HTTP_504_Gateway_Timeout_Anything_Else(Session session)
        {
            this.session = session;

            /////////////////////////////
            // 504.99. Pick up any other 504 Gateway Timeout and write data into the comments box.
            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 504 Gateway Timeout (99).");

            getSetSessionFlags.SetUIBackColour(this.session, "Red");
            getSetSessionFlags.SetUITextColour(this.session, "Black");

            getSetSessionFlags.SetResponseCodeDescription(this.session, "504 Gateway Timeout");

            getSetSessionFlags.SetXResponseComments(this.session, "<b><span style='color:red'>HTTP 504 Gateway Timeout</span></b>");
            getSetSessionFlags.SetXResponseComments(this.session, "The quantity of these types of server errors need to be considered in context with what you are troubleshooting "
                + "and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could be cause for concern.");

            getSetSessionFlags.SetSessionType(this.session, "Gateway Timeout");

            // Nothing meaningful here, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}