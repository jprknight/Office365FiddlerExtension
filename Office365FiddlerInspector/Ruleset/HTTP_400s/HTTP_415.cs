using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_415
    {

        public void HTTP_415_UnSupported_Media_Type(Session session)
        {

            session["X-ResponseAlert"] = "HTTP 415 Unsupported Media Type (RFC 7231).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 415 Unsupported Media Type (RFC 7231).");

            session["X-ResponseCodeDescription"] = "415 Unsupported Media Type (RFC 7231)";

            // Nothing meaningful here, let further processing try to pick up something.
            Preferences.SetSACL(session, "0");
            Preferences.SetSTCL(session, "0");
            Preferences.SetSRSCL(session, "0");
        }
    }
}
