using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_301
    {
        internal Session session { get; set; }
        public void HTTP_301_Permanently_Moved(Session session)
        {

            /////////////////////////////
            //
            //  HTTP 301: Moved Permanently.
            //
            session["ui-backcolor"] = Preferences.HTMLColourGreen;
            session["ui-color"] = "black";

            session["X-ResponseAlert"] = "HTTP 301 Moved Permanently";
            session["X-ResponseComments"] = "Nothing of concern here at this time.";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 301 Moved Permanently.");

            session["X-ResponseCodeDescription"] = "301 Moved Permanently";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
