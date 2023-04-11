using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_497
    {
        internal Session session { get; set; }
        public void HTTP_497_Request_Sent_To_HTTPS_Port(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 497 nginx HTTP Request Sent to HTTPS Port.";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 497 nginx HTTP Request Sent to HTTPS Port");

            session["X-ResponseCodeDescription"] = "497 nginx HTTP Request Sent to HTTPS Port";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
