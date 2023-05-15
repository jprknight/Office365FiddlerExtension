using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_401 : ActivationService
    {
        private static HTTP_401 _instance;

        public static HTTP_401 Instance => _instance ?? (_instance = new HTTP_401());

        public void HTTP_401_Exchange_Online_AutoDiscover(Session session)
        {
            // 401.1. Exchange Online Autodiscover

            this.session = session;

            // Make sure this session is an Exchange Online Autodiscover request.
            // Non-ClickToRun clients redirect to https://autodiscover-s.outlook.com/Autodiscover/AutoDiscover.xml
            // ClickToRun clients use to https://outlook.office365.com/Autodiscover/AutoDiscover.xml.
            if ((this.session.hostname == "autodiscover-s.outlook.com")
                || (this.session.hostname == "outlook.office365.com")
                && (this.session.uriContains("autodiscover.xml")))
            {
                FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 401 Auth Challenge.");

                var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_401s",
                    UIBackColour = "Orange",
                    UITextColour = "Black",

                    SessionType = "Microsoft365 AutoDiscover",
                    ResponseCodeDescription = "401 Unauthorized (RFC 7235)",
                    ResponseAlert = "<b><span style='color:orange'>Autodiscover Authentication Challenge</span></b>",
                    ResponseComments = "Autodiscover Authentication Challenge. If the host for this session is autodiscover-s.outlook.com this is likely Outlook "
                    + "(MSI / perpetual license) being redirected from Exchange On-Premise."
                    + "<p><b>These are expected</b> and are not an issue as long as a subsequent "
                    + "HTTP 200 is seen for authentication to the server which issued the HTTP 401 unauthorized security challenge. "
                    + "<p>If you do not see HTTP 200's following HTTP 401's look for a wider authentication issue.</p>",
                    Authentication = "Autodiscover Microsoft365 Auth Challenge",

                    SessionAuthenticationConfidenceLevel = 10,
                    SessionTypeConfidenceLevel = 10,
                    SessionResponseServerConfidenceLevel = 5
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
        }

        public void HTTP_401_Exchange_OnPremise_AutoDiscover(Session session)
        {
            // Exchange OnPremise AutoDiscover

            this.session = session;

            var ExtensionSessionFlags = JsonConvert.DeserializeObject<SessionFlagProcessor.ExtensionSessionFlags>(SessionFlagProcessor.Instance.GetSessionJsonData(this.session));

            if (ExtensionSessionFlags.SessionTypeConfidenceLevel == 10 || ExtensionSessionFlags.SessionAuthenticationConfidenceLevel == 10)
            {
                return;
            }

            if (this.session.uriContains("/Autodiscover/Autodiscover.xml"))
            {
                FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 401 Auth Challenge.");

                var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_401s",
                    UIBackColour = "Orange",
                    UITextColour = "Black",

                    SessionType = "Exchange OnPremise AutoDiscover",
                    ResponseCodeDescription = "401 Unauthorized (RFC 7235)",
                    ResponseAlert = "<b><span style='color:orange'>Autodiscover Authentication Challenge</span></b>",
                    ResponseComments = "Autodiscover Authentication Challenge."
                    + "<p><b>These are expected</b> and are not an issue as long as a subsequent "
                    + "HTTP 200 is seen for authentication to the server which issued the HTTP 401 unauthorized security challenge. "
                    + "<p>If you do not see HTTP 200's following HTTP 401's look for a wider authentication issue.</p>",
                    Authentication = "Autodiscover OnPremise Auth Challenge",

                    SessionAuthenticationConfidenceLevel = 10,
                    SessionTypeConfidenceLevel = 10,
                    SessionResponseServerConfidenceLevel = 5
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
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
                FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 401 EWS call.");

                var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
                {
                    SectionTitle = "",
                    UIBackColour = "Orange",
                    UITextColour = "Black",

                    SessionType = "Exchange Web Services",
                    ResponseCodeDescription = "Exchange Web Services (EWS) call.",
                    ResponseAlert = "Exchange Web Services (EWS) call.",
                    ResponseComments = "Exchange Web Services (EWS) call.",
                    Authentication = "Auth Challenge",

                    SessionAuthenticationConfidenceLevel = 10,
                    SessionTypeConfidenceLevel = 10,
                    SessionResponseServerConfidenceLevel = 5
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
        }

        public void HTTP_401_Everything_Else(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 401 Auth Challenge.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "",
                UIBackColour = "Orange",
                UITextColour = "Black",

                SessionType = "401 Unauthorized",
                ResponseCodeDescription = "401 Unauthorized (RFC 7235)",
                ResponseAlert = "<b><span style='color:orange'>Authentication Challenge</span></b>",
                ResponseComments = "Authentication Challenge. <b>These are expected</b> and are not an issue as long as a subsequent "
                + "HTTP 200 is seen for authentication to the server which issued the HTTP 401 unauthorized security challenge. "
                + "<p>If you do not see HTTP 200's following HTTP 401's look for a wider authentication issue.</p>",
                Authentication = "Auth Challenge",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 5,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}