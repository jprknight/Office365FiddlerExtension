using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_525
    {
        internal Session session { get; set; }
        public void HTTP_525_SSL_Handshake_Failed(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 525 Cloudflare SSL Handshake Failed.";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 525 Cloudflare SSL Handshake Failed");

            session["X-ResponseCodeDescription"] = "525 Cloudflare SSL Handshake Failed";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
