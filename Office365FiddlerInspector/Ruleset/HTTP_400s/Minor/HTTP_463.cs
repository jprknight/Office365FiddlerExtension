using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_463
    {
        internal Session session { get; set; }
        public void HTTP_463_X_Forwarded_For_Header(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 463 AWS X-Forwarded-For Header > 30 IP addresses.";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 463 AWS X-Forwarded-For Header > 30 IP addresses");

            session["X-ResponseCodeDescription"] = "463 AWS X-Forwarded-For Header > 30 IP addresses";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
