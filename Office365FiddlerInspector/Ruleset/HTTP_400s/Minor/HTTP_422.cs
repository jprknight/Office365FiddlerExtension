using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_422
    {
        internal Session session { get; set; }
        public void HTTP_422_Unprocessable_Entry(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 422 Unprocessable Entity (WebDAV; RFC 4918).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 422 Unprocessable Entity (WebDAV; RFC 4918).");

            session["X-ResponseCodeDescription"] = "422 Unprocessable Entity (WebDAV; RFC 4918)";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
