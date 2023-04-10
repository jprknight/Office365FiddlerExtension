using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_307 : ActivationService
    {

        public void HTTP_307_Temporary_Redirect(Session session)
        {
            /////////////////////////////
            //
            //  HTTP 307: Temporary Redirect.
            //

            // Specific scenario where a HTTP 307 Temporary Redirect incorrectly send an EXO Autodiscover request to an On-Premise resource, breaking Outlook connectivity.
            if (session.hostname.Contains("autodiscover") &&
                (session.hostname.Contains("mail.onmicrosoft.com") &&
                (session.fullUrl.Contains("autodiscover") &&
                (session.ResponseHeaders["Location"] != "https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml"))))
            {
                // Redirect location has been found to send the Autodiscover connection somewhere else other than'
                // Exchange Online, highlight.
                session["ui-backcolor"] = Preferences.HTMLColourRed;
                session["ui-color"] = "black";

                session["X-Authentication"] = "***UNEXPECTED LOCATION***";
                session["X-SessionType"] = "***UNEXPECTED LOCATION***";
                session["X-ResponseServer"] = "***UNEXPECTED LOCATION***";

                session["X-ResponseAlert"] = "<b><span style='color:red'>HTTP 307 Temporary Redirect</span></b>";
                session["X-ResponseComments"] = "<b>Temporary Redirects have been seen to redirect Exchange Online Autodiscover "
                    + "calls back to On-Premise resources, breaking Outlook connectivity</b>. Likely cause is a local networking device. Test outside of the LAN to confirm."
                    + "<p>This session is an Autodiscover request for Exchange Online which has not been sent to "
                    + "<a href='https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml' target='_blank'>https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml</a> as expected.</p>"
                    + "<p>Check the Headers or Raw tab and the Location to ensure the Autodiscover call is going to the correct place.</p>";

                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 307 On-Prem Temp Redirect - Unexpected location!");

                // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                Preferences.SetSACL(session, "10");
                Preferences.SetSTCL(session, "10");
                Preferences.SetSRSCL(session, "10");

                return;
            }
            else
            {
                // The above scenario is not seem, however Temporary Redirects are not normally expected to be seen.
                // Highlight as a warning.
                session["ui-backcolor"] = Preferences.HTMLColourOrange;
                session["ui-color"] = "black";

                session["X-ResponseAlert"] = "HTTP 307 Temporary Redirect";
                session["X-ResponseComments"] = "Temporary Redirects have been seen to redirect Exchange Online Autodiscover calls " +
                    "back to On-Premise resources, breaking Outlook connectivity. " +
                    "<p>Check the Headers or Raw tab and the Location to ensure the Autodiscover call is going to the correct place. </p>" +
                    "<p>If this session is not for an Outlook process then the information above may not be relevant to the issue under investigation.</p>";

                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 307 Temp Redirect.");

                session["X-ResponseCodeDescription"] = "307 Temporary Redirect";

                // Nothing meaningful here, let further processing try to pick up something.
                Preferences.SetSACL(session, "0");
                Preferences.SetSTCL(session, "0");
                Preferences.SetSRSCL(session, "0");
            }
        }
    }
}
