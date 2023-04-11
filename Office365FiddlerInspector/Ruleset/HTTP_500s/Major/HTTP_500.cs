using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_500
    {
        internal Session session { get; set; }
        public void HTTP_500_Internal_Server_Error_Repeating_Redirects(Session session)
        {
            /////////////////////////////
            //
            //  HTTP 500: Internal Server Error.
            //

            /////////////////////////////
            //
            // 500.1. Repeating Redirects Detected.
            //

            if (session.utilFindInResponse("Repeating redirects detected", false) > 1)
            {
                if (session.HostnameIs("outlook.office365.com"))
                {
                    session["ui-backcolor"] = Preferences.HTMLColourRed;
                    session["ui-color"] = "black";

                    session["X-SessionType"] = "***REPEATING REDIRECTS DETECTED***";

                    session["X-ResponseAlert"] = "<b><span style='color:red'>HTTP 500 Internal Server Error - Repeating redirects detected</span></b>";
                    session["X-ResponseComments"] = "<b><span style='color:red'>Repeating redirects detected</span></b> found in this session response. "
                        + "This response has been seen with OWA and federated domains. Is this issue seen with non-federated user accounts? "
                        + "If not this might suggest an issue with a federation service. "
                        + "<p>Alternatively does the impacted account have too many roles assigned? Too many roles on an account have been seen as a cause of this type of issue.</p>"
                        + "<p>Otherwise this might be an issue which needs to be raised to Microsoft support.</p>";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 500 Internal Server Error.");

                    session["X-ResponseCodeDescription"] = "500 Internal Server Error";

                    // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                    SessionProcessor.Instance.SetSACL(this.session, "5");
                    SessionProcessor.Instance.SetSTCL(this.session, "10");
                    SessionProcessor.Instance.SetSRSCL(this.session, "5");
                }
            }
        }

        public void HTTP_500_Internal_Server_Error_Impersonate_User_Denied(Session session)
        {
            /////////////////////////////
            //
            // 500.2. EWS ErrorImpersonateUserDenied.
            //

            if (session.utilFindInResponse("ErrorImpersonateUserDenied", false) > 1)
            {
                if (session.HostnameIs("outlook.office365.com"))
                {
                    if (session.uriContains("/EWS/Exchange.asmx"))
                    {
                        session["ui-backcolor"] = Preferences.HTMLColourRed;
                        session["ui-color"] = "black";

                        session["X-SessionType"] = "***EWS Impersonate User Denied***";

                        session["X-ResponseAlert"] = "<b><span style='color:red'>HTTP 500 Internal Server Error - EWS Impersonate User Denied</span></b>";
                        session["X-ResponseComments"] = "<b><span style='color:red'>EWS Impersonate User Denied</span></b> found in this session response. "
                            + "Check the service account in use has impersonation rights on the mailbox you are trying to work with."
                            + "Are the impersonation permissions given directly on the service account or via a security group?</p>";

                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 500 EWS Impersonate User Denied.");

                        session["X-ResponseCodeDescription"] = "500 EWS Impersonate User Denied";

                        // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                        SessionProcessor.Instance.SetSACL(this.session, "5");
                        SessionProcessor.Instance.SetSTCL(this.session, "10");
                        SessionProcessor.Instance.SetSRSCL(this.session, "5");
                    }
                }
            }
        }

        public void HTTP_500_Internal_Server_Error_OWA_Something_Went_Wrong(Session session)
        {
            /////////////////////////////
            //
            // 500.50. OWA - Something went wrong.
            //

            // General highlight on an OWA session where "Something went wrong."
            // Making this rule #50, since we may find more specific rules for this scenario.

            if (session.utilFindInResponse("Something went wrong", false) > 1)
            {
                if (session.HostnameIs("outlook.office365.com"))
                {
                    session["ui-backcolor"] = Preferences.HTMLColourRed;
                    session["ui-color"] = "black";

                    session["X-SessionType"] = "***OWA SOMETHING WENT WRONG***";

                    session["X-ResponseAlert"] = "<b><span style='color:red'>HTTP 500 Internal Server Error - OWA Something went wrong.</span></b>";
                    session["X-ResponseComments"] = "<b><span style='color:red'>OWA - Something went wrong</span></b> found in this session response. "
                        + "<p>Check the response Raw and Webview tabs to see what further details can be pulled on the issue.</p>"
                        + "<p>Does the issue reproduce with federated and non-federated (managed) domains?</p>"
                        + "<p>Does the issue reproduce in different browsers?</p>"
                        + "<p>Otherwise this might be an issue which needs to be raised to Microsoft support.</p>";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 500 Internal Server Error - OWA Something went wrong.");

                    session["X-ResponseCodeDescription"] = "500 OWA Something went wrong";

                    // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                    SessionProcessor.Instance.SetSACL(this.session, "5");
                    SessionProcessor.Instance.SetSTCL(this.session, "10");
                    SessionProcessor.Instance.SetSRSCL(this.session, "5");
                }
            }
        }

        public void HTTP_500_Internal_Server_Error_All_Others(Session session)
        {
            /////////////////////////////
            //
            // 500.99. Everything else.
            //

            // Pick up any 500 Internal Server Error and write data into the comments box.
            // Specific scenario on Outlook and Office 365 invalid DNS lookup.
            // < Discuss and confirm thinking here, validate with a working trace. Is this a true false positive? Highlight in green? >
            session["ui-backcolor"] = Preferences.HTMLColourRed;
            session["ui-color"] = "black";
            session["X-SessionType"] = "!HTTP 500 Internal Server Error!";

            session["X-ResponseAlert"] = "<b><span style='color:red'>HTTP 500 Internal Server Error</span></b>";
            session["X-ResponseComments"] = "Consider the server that issued this response, "
                + "look at the IP address in the 'Host IP' column and lookup where it is hosted to know who should be looking at "
                + "the issue.";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 500 Internal Server Error.");

            session["X-ResponseCodeDescription"] = "500 Internal Server Error";

            // Possible something more to be found, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "5");
            SessionProcessor.Instance.SetSTCL(this.session, "5");
            SessionProcessor.Instance.SetSRSCL(this.session, "5");
        }
    }
}
