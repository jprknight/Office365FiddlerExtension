using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_401 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        public void HTTP_401_Exchange_Online_AutoDiscover(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            // 401.1. Exchange Online Autodiscover
            //
            // Make sure this session is an Exchange Online Autodiscover request.
            // Non-ClickToRun clients redirect to https://autodiscover-s.outlook.com/Autodiscover/AutoDiscover.xml
            // ClickToRun clients use to https://outlook.office365.com/Autodiscover/AutoDiscover.xml.
            if ((this.session.hostname == "autodiscover-s.outlook.com")
                || (this.session.hostname == "outlook.office365.com")
                && (this.session.uriContains("autodiscover.xml")))
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 401 Auth Challenge.");

                getSetSessionFlags.SetUIBackColour(this.session, "Orange");
                getSetSessionFlags.SetUITextColour(this.session, "Black");

                getSetSessionFlags.SetResponseCodeDescription(this.session, "401 Unauthorized (RFC 7235)");

                getSetSessionFlags.SetXAuthentication(this.session, "Autodiscover Microsoft365 Auth Challenge");
                getSetSessionFlags.SetXResponseAlert(this.session, "<b><span style='color:orange'>Autodiscover Authentication Challenge</span></b>");
                getSetSessionFlags.SetXResponseComments(this.session, "Autodiscover Authentication Challenge. If the host for this session is autodiscover-s.outlook.com this is likely Outlook "
                    + "(MSI / perpetual license) being redirected from Exchange On-Premise."
                    + "<p><b>These are expected</b> and are not an issue as long as a subsequent "
                    + "HTTP 200 is seen for authentication to the server which issued the HTTP 401 unauthorized security challenge. "
                    + "<p>If you do not see HTTP 200's following HTTP 401's look for a wider authentication issue.</p>");

                // Set confidence level for Session Authentication, Session Type, and Session Response Server.
                getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "10");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
                getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
        }

        public void HTTP_401_Exchange_OnPremise_AutoDiscover(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            // 401.2 Exchange AutoDiscover
            if (this.session.uriContains("/Autodiscover/Autodiscover.xml"))
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 401 Auth Challenge.");

                getSetSessionFlags.SetUIBackColour(this.session, "Orange");
                getSetSessionFlags.SetUITextColour(this.session, "Black");

                getSetSessionFlags.SetResponseCodeDescription(this.session, "401 Unauthorized (RFC 7235)");

                getSetSessionFlags.SetXAuthentication(this.session, "Autodiscover OnPremise Auth Challenge");
                getSetSessionFlags.SetSessionType(this.session, "Exchange Autodiscover");
                getSetSessionFlags.SetProcess(this.session);

                getSetSessionFlags.SetXResponseAlert(this.session, "<b><span style='color:orange'>Autodiscover Authentication Challenge</span></b>");
                getSetSessionFlags.SetXResponseComments(this.session, "Autodiscover Authentication Challenge."
                    + "<p><b>These are expected</b> and are not an issue as long as a subsequent "
                    + "HTTP 200 is seen for authentication to the server which issued the HTTP 401 unauthorized security challenge. "
                    + "<p>If you do not see HTTP 200's following HTTP 401's look for a wider authentication issue.</p>");

                // Set confidence level for Session Authentication, Session Type, and Session Response Server.
                getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "10");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
                getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
        }

        public void HTTP_401_EWS(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            // 401.3 Any Exchange Web Services
            if (session.uriContains("/EWS/Exchange.asmx"))
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 401 EWS call.");

                getSetSessionFlags.SetUIBackColour(this.session, "Orange");
                getSetSessionFlags.SetUITextColour(this.session, "Black");

                getSetSessionFlags.SetResponseCodeDescription(this.session, "Exchange Web Services (EWS) call.");

                getSetSessionFlags.SetSessionType(this.session, "Exchange Web Services");
                getSetSessionFlags.SetXAuthentication(this.session, "Auth Challenge");
                getSetSessionFlags.SetXResponseAlert(this.session, "Exchange Web Services (EWS) call.");
                getSetSessionFlags.SetXResponseComments(this.session, "Exchange Web Services (EWS) call.");                

                // Absolute certainly we don't want to do anything further for Session Type with this session.
                getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "10");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
                getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
        }

        public void HTTP_401_Everything_Else(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            // 401.99 Everything else.

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 401 Auth Challenge.");

            getSetSessionFlags.SetUIBackColour(this.session, "Orange");
            getSetSessionFlags.SetUITextColour(this.session, "Black");

            getSetSessionFlags.SetResponseCodeDescription(this.session, "401 Unauthorized (RFC 7235)");

            getSetSessionFlags.SetXAuthentication(this.session, "Auth Challenge");
            getSetSessionFlags.SetXResponseAlert(this.session, "<b><span style='color:orange'>Authentication Challenge</span></b>");
            getSetSessionFlags.SetXResponseComments(this.session, "Authentication Challenge. <b>These are expected</b> and are not an issue as long as a subsequent "
                + "HTTP 200 is seen for authentication to the server which issued the HTTP 401 unauthorized security challenge. "
                + "<p>If you do not see HTTP 200's following HTTP 401's look for a wider authentication issue.</p>");

            // Nothing meaningful here, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "5");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "5");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "5");
        }
    }
}