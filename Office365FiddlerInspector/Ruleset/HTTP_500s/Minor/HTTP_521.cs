using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_521
    {
        internal Session session { get; set; }
        public void HTTP_521_Web_Server_Is_Down(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 521 Cloudflare Web Server Is Down.";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 521 Cloudflare Web Server Is Down");

            session["X-ResponseCodeDescription"] = "521 Cloudflare Web Server Is Down";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
