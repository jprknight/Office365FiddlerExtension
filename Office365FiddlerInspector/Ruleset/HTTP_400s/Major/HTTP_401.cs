using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_401
    {
        internal Session session { get; set; }
        public void HTTP_401_Exchange_Online_AutoDiscover(Session session)
        {
            /////////////////////////////
            //
            //  HTTP 401: UNAUTHORIZED / AUTHENTICATION CHALLENGE.
            //

            /////////////////////////////
            //
            // 401.1. Exchange Online Autodiscover
            //
            // Make sure this session is an Exchange Online Autodiscover request.
            // Non-ClickToRun clients redirect to https://autodiscover-s.outlook.com/Autodiscover/AutoDiscover.xml
            // ClickToRun clients use to https://outlook.office365.com/Autodiscover/AutoDiscover.xml.
            if ((session.hostname == "autodiscover-s.outlook.com")
                || (session.hostname == "outlook.office365.com")
                && (session.uriContains("autodiscover.xml")))
            {
                session["ui-backcolor"] = Preferences.HTMLColourOrange;
                session["ui-color"] = "black";
                session["X-Authentication"] = "Autodiscover Auth Challenge";

                session["X-ResponseAlert"] = "<b><span style='color:orange'>Autodiscover Authentication Challenge</span></b>";
                session["X-ResponseComments"] = "Autodiscover Authentication Challenge. If the host for this session is autodiscover-s.outlook.com this is likely Outlook "
                    + "(MSI / perpetual license) being redirected from Exchange On-Premise."
                    + "<p><b>These are expected</b> and are not an issue as long as a subsequent "
                    + "HTTP 200 is seen for authentication to the server which issued the HTTP 401 unauthorized security challenge. "
                    + "<p>If you do not see HTTP 200's following HTTP 401's look for a wider authentication issue.</p>";

                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 401 Auth Challenge.");

                session["X-ResponseCodeDescription"] = "401 Unauthorized (RFC 7235)";

                // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                SessionProcessor.Instance.SetSACL(this.session, "10");
                SessionProcessor.Instance.SetSTCL(this.session, "10");
                SessionProcessor.Instance.SetSRSCL(this.session, "5");
            }
        }

        public void HTTP_401_Exchange_OnPremise_AutoDiscover(Session session)
        {
            /////////////////////////////
            //
            // 401.2 Exchange AutoDiscover
            if (session.uriContains("/Autodiscover/Autodiscover.xml"))
            {
                session["ui-backcolor"] = Preferences.HTMLColourOrange;
                session["ui-color"] = "black";

                session["X-Authentication"] = "Autodiscover Auth Challenge";
                session["X-SessionType"] = "Exchange Autodiscover";

                session["X-ResponseAlert"] = "<b><span style='color:orange'>Autodiscover Authentication Challenge</span></b>";
                session["X-ResponseComments"] = "Autodiscover Authentication Challenge."
                    + "<p><b>These are expected</b> and are not an issue as long as a subsequent "
                    + "HTTP 200 is seen for authentication to the server which issued the HTTP 401 unauthorized security challenge. "
                    + "<p>If you do not see HTTP 200's following HTTP 401's look for a wider authentication issue.</p>";

                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 401 Auth Challenge.");

                session["X-ResponseCodeDescription"] = "401 Unauthorized (RFC 7235)";

                // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                SessionProcessor.Instance.SetSACL(this.session, "10");
                SessionProcessor.Instance.SetSTCL(this.session, "10");
                SessionProcessor.Instance.SetSRSCL(this.session, "5");
            }
        }

        public void HTTP_401_EWS(Session session)
        {
            /////////////////////////////
            //
            // 401.3 Any Exchange Web Services
            if (session.uriContains("/EWS/Exchange.asmx"))
            {
                session["ui-backcolor"] = Preferences.HTMLColourOrange;
                session["ui-color"] = "black";
                session["X-SessionType"] = "Exchange Web Services";
                session["X-Authentication"] = "Auth Challenge";

                session["X-ResponseAlert"] = "Exchange Web Services (EWS) call.";
                session["X-ResponseComments"] = "Exchange Web Services (EWS) call.";

                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 200 EWS call.");

                // Absolute certainly we don't want to do anything further for Session Type with this session.
                SessionProcessor.Instance.SetSACL(this.session, "10");
                SessionProcessor.Instance.SetSTCL(this.session, "10");
                SessionProcessor.Instance.SetSRSCL(this.session, "5");
            }
        }

        public void HTTP_401_Everything_Else(Session session)
        {
            /////////////////////////////
            //
            // 401.99 Everything else.

            session["ui-backcolor"] = Preferences.HTMLColourOrange;
            session["ui-color"] = "black";
            session["X-Authentication"] = "Auth Challenge";

            session["X-ResponseAlert"] = "<b><span style='color:orange'>Authentication Challenge</span></b>";
            session["X-ResponseComments"] = "Authentication Challenge. <b>These are expected</b> and are not an issue as long as a subsequent "
                + "HTTP 200 is seen for authentication to the server which issued the HTTP 401 unauthorized security challenge. "
                + "<p>If you do not see HTTP 200's following HTTP 401's look for a wider authentication issue.</p>";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 401 Auth Challenge.");

            session["X-ResponseCodeDescription"] = "401 Unauthorized (RFC 7235)";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "5");
            SessionProcessor.Instance.SetSTCL(this.session, "5");
            SessionProcessor.Instance.SetSRSCL(this.session, "5");
        }
    }
}
