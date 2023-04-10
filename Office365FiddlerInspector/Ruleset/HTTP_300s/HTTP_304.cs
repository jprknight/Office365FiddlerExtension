using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_304 : ActivationService
    {
        
        public void HTTP_304_Not_Modified(Session session)
        {
            /////////////////////////////
            //
            //  HTTP 304: Not modified.
            //
            session["ui-backcolor"] = Preferences.HTMLColourGreen;
            session["ui-color"] = "black";

            session["X-ResponseAlert"] = "HTTP 304 Not Modified";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 304 Not modified.");

            session["X-ResponseCodeDescription"] = "304 Not Modified (RFC 7232)";

            // Nothing meaningful here, let further processing try to pick up something.
            Preferences.SetSACL(session, "0");
            Preferences.SetSTCL(session, "0");
            Preferences.SetSRSCL(session, "0");
        }
    }
}
