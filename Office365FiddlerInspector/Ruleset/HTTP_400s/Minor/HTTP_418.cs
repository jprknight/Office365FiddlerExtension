using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_418
    {
        internal Session session { get; set; }
        public void HTTP_418_Im_A_Teapot(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 418 I'm a teapot (RFC 2324, RFC 7168).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 418 I'm a teapot (RFC 2324, RFC 7168).");

            session["X-ResponseCodeDescription"] = "418 I'm a teapot (RFC 2324, RFC 7168)";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");

        }
    }
}
