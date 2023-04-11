using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_412
    {
        internal Session session { get; set; }
        public void HTTP_412_Precondition_Failed(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 412 Precondition Failed (RFC 7232).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 412 Precondition Failed (RFC 7232).");

            session["X-ResponseCodeDescription"] = "412 Precondition Failed (RFC 7232)";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
