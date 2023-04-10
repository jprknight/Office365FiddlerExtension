using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_418 : ActivationService
    {

        public void HTTP_418_Im_A_Teapot(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 418 I'm a teapot (RFC 2324, RFC 7168).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 418 I'm a teapot (RFC 2324, RFC 7168).");

            session["X-ResponseCodeDescription"] = "418 I'm a teapot (RFC 2324, RFC 7168)";

            // Nothing meaningful here, let further processing try to pick up something.
            Preferences.SetSACL(session, "0");
            Preferences.SetSTCL(session, "0");
            Preferences.SetSRSCL(session, "0");

        }
    }
}
