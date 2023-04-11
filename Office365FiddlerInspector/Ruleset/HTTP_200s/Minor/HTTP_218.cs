using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_218
    {
        internal Session session { get; set; }
        public void HTTP_218_This_Is_Fine_Apache_Web_Server(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 218 This is fine (Apache Web Server).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 218 This is fine (Apache Web Server).");

            session["X-ResponseCodeDescription"] = "218 This is fine (Apache Web Server)";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
