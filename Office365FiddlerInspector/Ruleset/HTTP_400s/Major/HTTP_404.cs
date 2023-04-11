using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_404
    {
        internal Session session { get; set; }
        public void HTTP_404_Not_Found(Session session)
        {
            /////////////////////////////
            //
            //  HTTP 404: Not Found.
            //
            session["ui-backcolor"] = Preferences.HTMLColourOrange;
            session["ui-color"] = "black";
            session["X-SessionType"] = "HTTP 404 Not Found";

            session["X-ResponseAlert"] = "<b><span style='color:red'>HTTP 404 Not Found</span></b>";
            session["X-ResponseComments"] = "The quantity of these types of server errors need to be considered in context with what you are troubleshooting " +
                "and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could be cause for concern.";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 404 Not found.");

            session["X-ResponseCodeDescription"] = "404 Not Found";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
