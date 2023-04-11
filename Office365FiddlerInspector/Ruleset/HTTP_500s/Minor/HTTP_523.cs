using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_523
    {
        internal Session session { get; set; }
        public void HTTP_523_Origin_Is_Unreachable(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 523 Cloudflare Origin Is Unreachable.";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 523 Cloudflare Origin Is Unreachable");

            session["X-ResponseCodeDescription"] = "523 Cloudflare Origin Is Unreachable";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
