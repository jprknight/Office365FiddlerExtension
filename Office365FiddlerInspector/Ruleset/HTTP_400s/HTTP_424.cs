using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_424 : ActivationService
    {

        public void HTTP_424_Failed_Dependency(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 424 Failed Dependency (WebDAV; RFC 4918).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 424 Failed Dependency (WebDAV; RFC 4918).");

            session["X-ResponseCodeDescription"] = "424 Failed Dependency (WebDAV; RFC 4918)";

            // Nothing meaningful here, let further processing try to pick up something.
            Preferences.SetSACL(session, "0");
            Preferences.SetSTCL(session, "0");
            Preferences.SetSRSCL(session, "0");
        }
    }
}
