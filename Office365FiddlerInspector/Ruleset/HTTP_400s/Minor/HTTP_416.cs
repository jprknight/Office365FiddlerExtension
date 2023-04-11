using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_416
    {
        internal Session session { get; set; }
        public void HTTP_416_Range_Not_Satisfiable(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 416 Range Not Satisfiable (RFC 7233).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            session["X-ResponseCodeDescription"] = "416 Range Not Satisfiable (RFC 7233)";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 416 Range Not Satisfiable (RFC 7233).");

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
