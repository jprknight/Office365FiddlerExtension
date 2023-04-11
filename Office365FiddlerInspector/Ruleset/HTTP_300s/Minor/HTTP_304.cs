using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_304
    {
        internal Session session { get; set; }
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
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
