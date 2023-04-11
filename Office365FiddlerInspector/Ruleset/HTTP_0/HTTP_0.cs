using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_0
    {
        //private static HTTP_0 _instance;

        //public static HTTP_0 Instance => _instance ?? (_instance = new HTTP_0());

        internal Session session { get; set; }
        public void HTTP_0_NoSessionResponse(Session session)
        {
            /////////////////////////////
            //
            //  HTTP 0: No Response.
            this.session["ui-backcolor"] = Preferences.HTMLColourRed;
            this.session["ui-color"] = "black";
            this.session["X-SessionType"] = "!NO RESPONSE!";

            this.session["X-ResponseAlert"] = "<b><span style='color:red'>HTTP 0 - No Response</span></b>";

            this.session["X-ResponseComments"] = "The quantity of these types of server errors need to be considered in context with what you are "
                + "troubleshooting and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could "
                + "be cause for concern."
                + "<p>If you are not seeing expected client traffic, consider if network traces should be collected. Review if there is an underlying "
                + "network issue such as congestion on routers, which could be causing issues. The Network Connection Status Indicator (NCSI) on the "
                + "client computer might also be an area to investigate.</p>";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 0 No response");

            this.session["X-ResponseCodeDescription"] = "0 No Response";

            // This actually isn't very useful, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "5");
            SessionProcessor.Instance.SetSTCL(this.session, "5");
            SessionProcessor.Instance.SetSRSCL(this.session, "5");
        }   
    }
}
