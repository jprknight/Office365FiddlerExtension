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
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        public void HTTP_302_Redirect(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 302 Found / Redirect.");

            getSetSessionFlags.SetUIBackColour(this.session, "Green");
            getSetSessionFlags.SetUITextColour(this.session, "Black");

            getSetSessionFlags.SetResponseCodeDescription(this.session, "302 Redirect / Found");

            // Exchange Autodiscover redirects.
            if (this.session.uriContains("autodiscover"))
            {
                getSetSessionFlags.SetSessionType(this.session, "Autodiscover Redirect");
                getSetSessionFlags.SetXResponseAlert(this.session, "<b><span style='color:green'>Exchange Autodiscover redirect.</span></b>");
                getSetSessionFlags.SetXResponseComments(this.session, "This type of traffic is typically an Autodiscover redirect response from "
                    + "Exchange On-Premise sending the Outlook client to connect to Exchange Online.");
            }
            // All other HTTP 302 Redirects.
            else
            {
                getSetSessionFlags.SetSessionType(this.session, "Redirect");
                getSetSessionFlags.SetXResponseAlert(this.session, "<b><span style='color:green'>Redirect.</span></b>");
                getSetSessionFlags.SetXResponseComments(this.session, "Redirects within Office 365 client applications or servers are not unusual. "
                    + "The only potential downfall is too many of them. However if this happens you would normally see a too many "
                    + "redirects exception thrown as a server response.");
            }

            // Possible something more to be found, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "5");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "5");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "5");
        }
    }
}