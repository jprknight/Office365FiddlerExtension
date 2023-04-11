using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_449
    {
        internal Session session { get; set; }
        public void HTTP_449_IIS_Retry_With(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 449 IIS Retry With.";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            session["X-ResponseCodeDescription"] = "449 IIS Retry With";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 449 IIS Retry With");

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
