using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_421
    {
        internal Session session { get; set; }
        public void HTTP_421_Misdirected_Request(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 421 Misdirected Request (RFC 7540).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 421 Misdirected Request (RFC 7540).");

            session["X-ResponseCodeDescription"] = "421 Misdirected Request (RFC 7540)";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
