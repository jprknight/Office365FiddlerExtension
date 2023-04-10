using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_302 : ActivationService
    {

        public void HTTP_302_Redirect(Session session)
        {
            /////////////////////////////
            //
            //  HTTP 302: Found / Redirect.
            //            

            // Exchange Autodiscover redirects.
            if (session.uriContains("autodiscover"))
            {
                session["ui-backcolor"] = Preferences.HTMLColourGreen;
                session["ui-color"] = "black";
                session["X-SessionType"] = "Autodiscover Redirect";

                session["X-ResponseAlert"] = "<b><span style='color:green'>Exchange Autodiscover redirect.</span></b>";
                session["X-ResponseComments"] = "This type of traffic is typically an Autodiscover redirect response from Exchange On-Premise "
                    + "sending the Outlook client to connect to Exchange Online.";
            }
            // All other HTTP 302 Redirects.
            else
            {
                session["ui-backcolor"] = Preferences.HTMLColourGreen;
                session["ui-color"] = "black";
                session["X-SessionType"] = "Redirect";

                session["X-ResponseAlert"] = "<b><span style='color:green'>Redirect.</span></b>";
                session["X-ResponseComments"] = "Redirects within Office 365 client applications or servers are not unusual. "
                    + "The only potential downfall is too many of them. However if this happens you would normally see a too many "
                    + "redirects exception thrown as a server response.";
            }

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 302 Found / Redirect.");

            session["X-ResponseCodeDescription"] = "302 Found";

            // Possible something more to be found, let further processing try to pick up something.
            Preferences.SetSACL(session, "5");
            Preferences.SetSTCL(session, "5");
            Preferences.SetSRSCL(session, "5");
        }
    }
}
