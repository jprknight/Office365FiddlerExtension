using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_522
    {
        internal Session session { get; set; }
        public void HTTP_522_Connection_Timed_Out(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 522 Cloudflare Connection Timed Out.";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 522 Cloudflare Connection Timed Out");

            session["X-ResponseCodeDescription"] = "522 Cloudflare Connection Timed Out";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
