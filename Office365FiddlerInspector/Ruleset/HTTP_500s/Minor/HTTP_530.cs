using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_530
    {
        internal Session session { get; set; }
        public void HTTP_530_Site_Is_Frozen(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 530 Site is frozen or Cloudflare Error returned with 1xxx error.";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 530 Site is frozen or Cloudflare Error returned with 1xxx error.");

            session["X-ResponseCodeDescription"] = "530 Site is frozen or Cloudflare Error returned with 1xxx error.";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
