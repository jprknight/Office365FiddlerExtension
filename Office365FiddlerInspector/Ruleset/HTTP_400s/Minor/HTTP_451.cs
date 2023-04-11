using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_451
    {
        internal Session session { get; set; }
        public void HTTP_451_Unavailable_For_Legal_Reasons_or_IIS_Redirect(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 451 Unavailable For Legal Reasons (RFC 7725) or 451 IIS Redirect.";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 451 Unavailable For Legal Reasons (RFC 7725) or 451 IIS Redirect.");

            session["X-ResponseCodeDescription"] = "451 Unavailable For Legal Reasons (RFC 7725) or 451 IIS Redirect";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
