using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Fiddler.WebFormats;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_503
    {
        internal Session session { get; set; }

        private static HTTP_503 _instance;

        public static HTTP_503 Instance => _instance ?? (_instance = new HTTP_503());

        public void HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable(Session session)
        {
            this.session = session;

            if (SessionWordSearch.Instance.Search(this.session, "FederatedSTSUnreachable") == 0)
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 503 Service Unavailable. FederatedStsUnreachable in response body!");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_503s|HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable");
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
                sessionSeverity = 60;
            }
            
            string RealmURL = "https://login.microsoftonline.com/GetUserRealm.srf?Login=" + this.session.oRequest["X-User-Identity"] + "&xml=1";

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_503s",

                SessionType = "***FederatedSTSUnreachable***",
                ResponseCodeDescription = "503 Federation Service Unavailable",
                ResponseAlert = "<b><span style='color:red'>FederatedSTSUnreachable</span></b>",
                ResponseComments = "<b><span style='color:red'>HTTP 503: FederatedSTSUnreachable</span></b>."
                + "<b><span style='color:red'>The fedeation service is unreachable or unavailable</span></b>."
                + "<p><b><span style='color:red'>Troubleshoot this issue first before doing anything else.</span></b></p>"
                + "<p>Check the Raw tab for additional details.</p>"
                + "<p>Check the realm page for the authenticating domain. Check the below links from the Realm page to see if the IDP gives the "
                + "expected responses.</p>"
                + $"<a href='{RealmURL}' target='_blank'>{RealmURL}</a>"
                + "<p><b>Expected responses for ADFS</b> (other federation services such as Ping, OKTA may vary)</p>"
                + "<b>AuthURL</b>: Normally expected to show federation service logon page.<br />"
                + "<b>STSAuthURL</b>: Normally expected to show HTTP 400.<br />"
                + "<b>MEXURL</b>: Normally expected to show long stream of XML data.<br />"
                + "<p>If any of these show the HTTP 503 Service Unavailable this <b>confirms some kind of failure on the federation service</b>.</p>"
                + "<p>If however you get the expected responses, this <b>does not neccessarily mean the federation service / everything authentication is healthy</b>. "
                + "Further investigation is advised. You could try hitting these endpoints a few times and see if you get an intermittent failure.</p>",

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);           
        }

        public void HTTP_503_Service_Unavailable_Everything_Else(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 503 Service Unavailable.");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_503s|HTTP_503_Service_Unavailable_Everything_Else");
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
                sessionSeverity = 60;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_503s",

                SessionType = "!Service Unavailable!",
                ResponseCodeDescription = "503 Service Unavailable",
                ResponseAlert = "<b><span style='color:red'>HTTP 503 Service Unavailable</span></b>",
                ResponseComments = "<b><span style='color:red'>Server that was contacted in this session reports "
                + "it is unavailable</span></b>. Look at the server that issued this response, it is healthy? Contactable? "
                + "Contactable consistently or intermittently? Consider other session server responses in the 500's (500, 502 or 503) in conjunction with this session.",

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