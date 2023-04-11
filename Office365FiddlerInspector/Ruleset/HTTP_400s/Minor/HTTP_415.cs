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
        internal Session session { get; set; }
        public void HTTP_415_UnSupported_Media_Type(Session session)
        {

            session["X-ResponseAlert"] = "HTTP 415 Unsupported Media Type (RFC 7231).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 415 Unsupported Media Type (RFC 7231).");

            session["X-ResponseCodeDescription"] = "415 Unsupported Media Type (RFC 7231)";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
