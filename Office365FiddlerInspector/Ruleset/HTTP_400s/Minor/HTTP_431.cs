using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_431
    {
        internal Session session { get; set; }
        public void HTTP_431_Request_Header_Fields_Too_Large(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 431 Request Header Fields Too Large (RFC 6585).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            session["X-ResponseCodeDescription"] = "431 Request Header Fields Too Large (RFC 6585)";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 431 Request Header Fields Too Large (RFC 6585).");

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
