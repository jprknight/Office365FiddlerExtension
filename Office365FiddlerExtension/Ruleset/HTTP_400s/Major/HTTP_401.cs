using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_401 : ActivationService
    {
        private static HTTP_401 _instance;

        public static HTTP_401 Instance => _instance ?? (_instance = new HTTP_401());

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

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Orange");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");
                
                GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "401 Unauthorized (RFC 7235)");

                GetSetSessionFlags.Instance.SetSessionType(this.session, "Microsoft365 AutoDiscover");
                GetSetSessionFlags.Instance.SetXAuthentication(this.session, "Autodiscover Microsoft365 Auth Challenge");
                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:orange'>Autodiscover Authentication Challenge</span></b>");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "Autodiscover Authentication Challenge. If the host for this session is autodiscover-s.outlook.com this is likely Outlook "
                    + "(MSI / perpetual license) being redirected from Exchange On-Premise."
                    + "<p><b>These are expected</b> and are not an issue as long as a subsequent "
                    + "HTTP 200 is seen for authentication to the server which issued the HTTP 401 unauthorized security challenge. "
                    + "<p>If you do not see HTTP 200's following HTTP 401's look for a wider authentication issue.</p>");

                // Set confidence level for Session Authentication, Session Type, and Session Response Server.
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
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

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Orange");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "401 Unauthorized (RFC 7235)");

                GetSetSessionFlags.Instance.SetSessionType(this.session, "Exchange OnPremise AutoDiscover");
                GetSetSessionFlags.Instance.SetXAuthentication(this.session, "Autodiscover OnPremise Auth Challenge");
                GetSetSessionFlags.Instance.SetSessionType(this.session, "Exchange Autodiscover");
                GetSetSessionFlags.Instance.SetProcess(this.session);

                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:orange'>Autodiscover Authentication Challenge</span></b>");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "Autodiscover Authentication Challenge."
                    + "<p><b>These are expected</b> and are not an issue as long as a subsequent "
                    + "HTTP 200 is seen for authentication to the server which issued the HTTP 401 unauthorized security challenge. "
                    + "<p>If you do not see HTTP 200's following HTTP 401's look for a wider authentication issue.</p>");

                // Set confidence level for Session Authentication, Session Type, and Session Response Server.
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
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

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Orange");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "Exchange Web Services (EWS) call.");

                GetSetSessionFlags.Instance.SetSessionType(this.session, "Exchange Web Services");
                GetSetSessionFlags.Instance.SetXAuthentication(this.session, "Auth Challenge");
                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "Exchange Web Services (EWS) call.");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "Exchange Web Services (EWS) call.");                

                // Absolute certainly we don't want to do anything further for Session Type with this session.
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
        }

        public void HTTP_401_Everything_Else(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            // 401.99 Everything else.

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 401 Auth Challenge.");

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Orange");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "401 Unauthorized (RFC 7235)");
            GetSetSessionFlags.Instance.SetSessionType(this.session, "401 Unauthorized");
            GetSetSessionFlags.Instance.SetXAuthentication(this.session, "Auth Challenge");
            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:orange'>Authentication Challenge</span></b>");
            GetSetSessionFlags.Instance.SetXResponseComments(this.session, "Authentication Challenge. <b>These are expected</b> and are not an issue as long as a subsequent "
                + "HTTP 200 is seen for authentication to the server which issued the HTTP 401 unauthorized security challenge. "
                + "<p>If you do not see HTTP 200's following HTTP 401's look for a wider authentication issue.</p>");

            // Nothing meaningful here, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
        }
    }
}