using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_414 : ActivationService
    {

        public void HTTP_414_URI_Too_Long(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 414 URI Too Long (RFC 7231).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 414 URI Too Long (RFC 7231).");

            session["X-ResponseCodeDescription"] = "414 URI Too Long (RFC 7231)";

            // Nothing meaningful here, let further processing try to pick up something.
            Preferences.SetSACL(session, "0");
            Preferences.SetSTCL(session, "0");
            Preferences.SetSRSCL(session, "0");
        }
    }
}
