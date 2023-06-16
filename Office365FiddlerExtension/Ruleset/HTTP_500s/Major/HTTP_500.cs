using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_500 : ActivationService
    {
        private static HTTP_500 _instance;

        public static HTTP_500 Instance => _instance ?? (_instance = new HTTP_500());

        public void HTTP_500_Internal_Server_Error_Repeating_Redirects(Session session)
        {
            // Repeating Redirects Detected.

            this.Session = session;

            if (!(this.Session.utilFindInResponse("Repeating redirects detected", false) > 1))
            {
                return;
            }

            if (!this.Session.HostnameIs("outlook.office365.com"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} HTTP 500 Internal Server Error.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_500s",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "***REPEATING REDIRECTS DETECTED***",
                ResponseCodeDescription = "500 Internal Server Error",
                ResponseAlert = "<b><span style='color:red'>HTTP 500 Internal Server Error - Repeating redirects detected</span></b>",
                ResponseComments = "<b><span style='color:red'>Repeating redirects detected</span></b> found in this session response. "
                + "This response has been seen with OWA and federated domains. Is this issue seen with non-federated user accounts? "
                + "If not this might suggest an issue with a federation service. "
                + "<p>Alternatively does the impacted account have too many roles assigned? Too many roles on an account have been seen as a cause of this type of issue.</p>"
                + "<p>Otherwise this might be an issue which needs to be raised to Microsoft support.</p>",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson); 
        }

        public void HTTP_500_Internal_Server_Error_Impersonate_User_Denied(Session session)
        {
            // EWS ErrorImpersonateUserDenied.

            this.Session = session;

            if (!this.Session.HostnameIs("outlook.office365.com"))
            {
                return;
            }

            if (!this.Session.uriContains("/EWS/Exchange.asmx"))
            {
                return;
            }

            if (!(this.Session.utilFindInResponse("ErrorImpersonateUserDenied", false) > 1))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} HTTP 500 EWS Impersonate User Denied.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_500s",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "***EWS Impersonate User Denied***",
                ResponseCodeDescription = "500 EWS Impersonate User Denied",
                ResponseAlert = "<b><span style='color:red'>HTTP 500 Internal Server Error - EWS Impersonate User Denied</span></b>",
                ResponseComments = "<b><span style='color:red'>EWS Impersonate User Denied</span></b> found in this session response. "
                + "Check the service account in use has impersonation rights on the mailbox you are trying to work with."
                + "Are the impersonation permissions given directly on the service account or via a security group?</p>",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }

        public void HTTP_500_Internal_Server_Error_OWA_Something_Went_Wrong(Session session)
        {
            // Microsoft365 OWA - Something went wrong.

            this.Session = session;

            if (!this.Session.HostnameIs("outlook.office365.com"))
            {
                return;
            }

            if (!(this.Session.utilFindInResponse("Something went wrong", false) > 1))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} HTTP 500 Internal Server Error - OWA Something went wrong.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_500s",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "***OWA SOMETHING WENT WRONG***",
                ResponseCodeDescription = "500 OWA Something went wrong",
                ResponseAlert = "<b><span style='color:red'>HTTP 500 Internal Server Error - OWA Something went wrong.</span></b>",
                ResponseComments = "<b><span style='color:red'>OWA - Something went wrong</span></b> found in this session response. "
                + "<p>Check the response Raw and Webview tabs to see what further details can be pulled on the issue.</p>"
                + "<p>Does the issue reproduce with federated and non-federated (managed) domains?</p>"
                + "<p>Does the issue reproduce in different browsers?</p>"
                + "<p>Otherwise this might be an issue which needs to be raised to Microsoft support.</p>",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }

        public void HTTP_500_Internal_Server_Error_All_Others(Session session)
        {
            // Everything else.

            this.Session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} HTTP 500 Internal Server Error.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "!HTTP 500 Internal Server Error!",
                ResponseCodeDescription = "500 Internal Server Error",
                ResponseAlert = "<b><span style='color:red'>HTTP 500 Internal Server Error</span></b>",
                ResponseComments = "Consider the server that issued this response, look at the IP address in the 'Host IP' "
                + "column and lookup where it is hosted to know who should be looking at the issue.",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }
    }
}