using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_450
    {
        internal Session session { get; set; }
        public void HTTP_450_Blocked_by_Windows_Parental_Controls(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 450 Blocked by Windows Parental Controls (Microsoft).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 450 Blocked by Windows Parental Controls (Microsoft).");

            session["X-ResponseCodeDescription"] = "450 Blocked by Windows Parental Controls (Microsoft)";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
