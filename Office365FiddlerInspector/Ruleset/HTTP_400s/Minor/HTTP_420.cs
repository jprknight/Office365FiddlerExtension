using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_420
    {
        internal Session session { get; set; }
        public void HTTP_420_Method_Failure_or_Enchance_Your_Calm(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 420 Method Failure (Spring Framework) or Enhance Your Calm (Twitter).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 420 Method Failure (Spring Framework) or Enhance Your Calm (Twitter).");

            session["X-ResponseCodeDescription"] = "420 Method Failure (Spring Framework) or Enhance Your Calm (Twitter)";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
