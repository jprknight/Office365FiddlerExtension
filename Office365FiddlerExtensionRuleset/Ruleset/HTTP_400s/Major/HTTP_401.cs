using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_401
    {
        internal Session session { get; set; }

        private static HTTP_401 _instance;

        public static HTTP_401 Instance => _instance ?? (_instance = new HTTP_401());

        public void HTTP_401_Exchange_Online_AutoDiscover(Session session)
        {
            this.session = session;

            // Make sure this session is an Exchange Online Autodiscover request.
            // Non-ClickToRun clients redirect to https://autodiscover-s.outlook.com/Autodiscover/AutoDiscover.xml
            // ClickToRun clients use to https://outlook.office365.com/Autodiscover/AutoDiscover.xml.
            if ((this.session.hostname == "autodiscover-s.outlook.com")
                || (this.session.hostname == "outlook.office365.com")
                && (this.session.uriContains("autodiscover.xml")))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 401 Auth Challenge.");

                int sessionAuthenticationConfidenceLevel;
                int sessionTypeConfidenceLevel;
                int sessionResponseServerConfidenceLevel;
                int sessionSeverity;

                try
                {
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_401s|HTTP_401_Exchange_Online_AutoDiscover");
                    sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                    sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                    sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                    sessionSeverity = sessionClassificationJson.SessionSeverity;
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                    sessionAuthenticationConfidenceLevel = 5;
                    sessionTypeConfidenceLevel = 10;
                    sessionResponseServerConfidenceLevel = 5;
                    sessionSeverity = 40;
                }

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_401s",

                    SessionType = "Microsoft365 AutoDiscover",
                    ResponseCodeDescription = "401 Unauthorized (RFC 7235)",
                    ResponseAlert = "<b><span style='color:orange'>Autodiscover Authentication Challenge</span></b>",
                    ResponseComments = "Autodiscover Authentication Challenge. If the host for this session is autodiscover-s.outlook.com this is likely Outlook "
                    + "(MSI / perpetual license) being redirected from Exchange On-Premise."
                    + "<p><b>These are expected</b> and are not an issue as long as a subsequent "
                    + "HTTP 200 is seen for authentication to the server which issued the HTTP 401 unauthorized security challenge. "
                    + "<p>If you do not see HTTP 200's following HTTP 401's look for a wider authentication issue.</p>",
                    Authentication = "Autodiscover Microsoft365 Auth Challenge",

                    SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                    SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                    SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                    SessionSeverity = sessionSeverity
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
        }

        public void HTTP_401_Exchange_OnPremise_AutoDiscover(Session session)
        {
            this.session = session;

            if (this.session.uriContains("/Autodiscover/Autodiscover.xml"))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 401 Auth Challenge.");

                int sessionAuthenticationConfidenceLevel;
                int sessionTypeConfidenceLevel;
                int sessionResponseServerConfidenceLevel;
                int sessionSeverity;

                try
                {
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_401s|HTTP_401_Exchange_OnPremise_AutoDiscover");
                    sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                    sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                    sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                    sessionSeverity = sessionClassificationJson.SessionSeverity;
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                    sessionAuthenticationConfidenceLevel = 5;
                    sessionTypeConfidenceLevel = 10;
                    sessionResponseServerConfidenceLevel = 5;
                    sessionSeverity = 40;
                }

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_401s",

                    SessionType = "Exchange OnPremise AutoDiscover",
                    ResponseCodeDescription = "401 Unauthorized (RFC 7235)",
                    ResponseAlert = "<b><span style='color:orange'>Autodiscover Authentication Challenge</span></b>",
                    ResponseComments = "Autodiscover Authentication Challenge."
                    + "<p><b>These are expected</b> and are not an issue as long as a subsequent "
                    + "HTTP 200 is seen for authentication to the server which issued the HTTP 401 unauthorized security challenge. "
                    + "<p>If you do not see HTTP 200's following HTTP 401's look for a wider authentication issue.</p>",
                    Authentication = "Autodiscover OnPremise Auth Challenge",

                    SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                    SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                    SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                    SessionSeverity = sessionSeverity
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
        }

        public void HTTP_401_EWS(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 401 EWS call.");

            if (!session.uriContains("/EWS/Exchange.asmx"))
            {
                return;
            }
            
            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_401s|HTTP_401_EWS");
                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                sessionAuthenticationConfidenceLevel = 5;
                sessionTypeConfidenceLevel = 10;
                sessionResponseServerConfidenceLevel = 5;
                sessionSeverity = 40;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_401s",

                SessionType = "Exchange Web Services",
                ResponseCodeDescription = "401 Exchange Web Services (EWS) call.",
                ResponseAlert = "Exchange Web Services (EWS) call.",
                ResponseComments = "Exchange Web Services (EWS) call.",
                Authentication = "Auth Challenge",

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        public void HTTP_401_Everything_Else(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 401 Auth Challenge.");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_401s|HTTP_401_Everything_Else");
                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                sessionAuthenticationConfidenceLevel = 5;
                sessionTypeConfidenceLevel = 10;
                sessionResponseServerConfidenceLevel = 5;
                sessionSeverity = 40;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_401s",

                SessionType = "401 Unauthorized",
                ResponseCodeDescription = "401 Unauthorized (RFC 7235)",
                ResponseAlert = "<b><span style='color:orange'>Authentication Challenge</span></b>",
                ResponseComments = "Authentication Challenge. <b>These are expected</b> and are not an issue as long as a subsequent "
                + "HTTP 200 is seen for authentication to the server which issued the HTTP 401 unauthorized security challenge. "
                + "<p>If you do not see HTTP 200's following HTTP 401's look for a wider authentication issue.</p>",
                Authentication = "Auth Challenge",

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}