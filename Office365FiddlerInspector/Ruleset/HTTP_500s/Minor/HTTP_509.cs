using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_509
    {
        internal Session session { get; set; }
        public void HTTP_509_Bandwidth_Limit_Exceeeded(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 509 Bandwidth Limit Exceeded (Apache Web Server/cPanel).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 509 Bandwidth Limit Exceeded (Apache Web Server/cPanel).");

            session["X-ResponseCodeDescription"] = "509 Bandwidth Limit Exceeded (Apache Web Server/cPanel)";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
