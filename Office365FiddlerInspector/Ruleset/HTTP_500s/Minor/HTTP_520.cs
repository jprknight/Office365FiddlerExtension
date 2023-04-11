using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_520
    {
        internal Session session { get; set; }
        public void HTTP_520_Web_Server_Returned_an_Unknown_Error(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 520 Cloudflare Web Server Returned an Unknown Error.";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 520 Cloudflare Web Server Returned an Unknown Error");

            session["X-ResponseCodeDescription"] = "520 Cloudflare Web Server Returned an Unknown Error";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
