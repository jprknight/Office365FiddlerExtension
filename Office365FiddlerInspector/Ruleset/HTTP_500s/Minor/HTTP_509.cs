using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_509 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();
        public void HTTP_509_Bandwidth_Limit_Exceeeded(Session session)
        {
            this.session = session;

            this.session["X-ResponseAlert"] = "HTTP 509 Bandwidth Limit Exceeded (Apache Web Server/cPanel).";
            getSetSessionFlags.SetXResponseCommentsNoKnownIssue(this.session);

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 509 Bandwidth Limit Exceeded (Apache Web Server/cPanel).");

            this.session["X-ResponseCodeDescription"] = "509 Bandwidth Limit Exceeded (Apache Web Server/cPanel)";

            // Nothing meaningful here, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}