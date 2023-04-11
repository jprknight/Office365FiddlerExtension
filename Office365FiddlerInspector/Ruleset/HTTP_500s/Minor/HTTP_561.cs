using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_561
    {
        internal Session session { get; set; }

        public void HTTP_561_Unauthorized(Session session)
        {
            this.session = session;

            this.session["X-ResponseAlert"] = "HTTP 561 AWS Unauthorized.";
            this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 561 AWS Unauthorized");

            this.session["X-ResponseCodeDescription"] = "561 AWS Unauthorized";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
