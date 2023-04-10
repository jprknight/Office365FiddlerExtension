using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_431 : ActivationService
    {

        public void HTTP_431_Request_Header_Fields_Too_Large(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 431 Request Header Fields Too Large (RFC 6585).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            session["X-ResponseCodeDescription"] = "431 Request Header Fields Too Large (RFC 6585)";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 431 Request Header Fields Too Large (RFC 6585).");

            // Nothing meaningful here, let further processing try to pick up something.
            Preferences.SetSACL(session, "0");
            Preferences.SetSTCL(session, "0");
            Preferences.SetSRSCL(session, "0");
        }
    }
}
