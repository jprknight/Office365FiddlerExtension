using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_200 : ActivationService
    {
        private static HTTP_200 _instance;

        public static HTTP_200 Instance => _instance ?? (_instance = new HTTP_200());

        public void HTTP_200_ClientAccessRule(Session session)
        {
            /////////////////////////////
            //
            // 200.1. Connection blocked by Client Access Rules.
            // 

            if (session.fullUrl.Contains("outlook.office365.com/mapi")
                && session.utilFindInResponse("Connection blocked by Client Access Rules", false) > 1)
            {
                session["ui-backcolor"] = Preferences.HTMLColourRed;
                session["ui-color"] = "black";

                session["X-SessionType"] = "!CLIENT ACCESS RULE!";

                session["X-ResponseAlert"] = "<b><span style='color:red'>CLIENT ACCESS RULE</span></b>";
                session["X-ResponseComments"] = "<b><span style='color:red'>A client access rule has blocked MAPI connectivity to the mailbox</span></b>. "
                    + "<p>Check if the <b><span style='color:red'>client access rule includes OutlookAnywhere</span></b>.</p>"
                    + "<p>Per <a href='https://docs.microsoft.com/en-us/exchange/clients-and-mobile-in-exchange-online/client-access-rules/client-access-rules' target='_blank'>"
                    + "https://docs.microsoft.com/en-us/exchange/clients-and-mobile-in-exchange-online/client-access-rules/client-access-rules </a>, <br />"
                    + "OutlookAnywhere includes MAPI over HTTP.<p>"
                    + "<p>Remove OutlookAnywhere from the client access rule, wait 1 hour, then test again.</p>";

                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 200.1 Connection blocked by Client Access Rules.");

                session["X-ResponseCodeDescription"] = "200 OK";

                // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                Preferences.SetSACL(session, "5");
                Preferences.SetSTCL(session, "10");
                Preferences.SetSRSCL(session, "5");
            }
        }

        public void HTTP_200_M365_Outlook_Mapi(Session session)
        {
            /////////////////////////////
            //
            // 200.2. Outlook MAPI traffic.
            //

            // I thought about checking for outlook.exe in the logic here, but I have many sample traces where for some (unknown to me) reason,
            // the process is rundll.exe. This isn't really a fixable thing, since people using this tool are usually collecting traces from
            // various systems, and who wants to troubleshoot the troubleshooting anyway. Leaving this off, as most people just want to see if
            // they can get some information and solve the issue.

            if (session.HostnameIs("outlook.office365.com") && (session.uriContains("/mapi/emsmdb/?MailboxId=")))
            {
                /////////////////////////////
                //
                // Protocol Disabled.
                //
                if (session.utilFindInResponse("ProtocolDisabled", false) > 1)
                {
                    session["ui-backcolor"] = Preferences.HTMLColourRed;
                    session["ui-color"] = "black";
                    session["X-SessionType"] = "***PROTOCOL DISABLED***";

                    session["X-ResponseAlert"] = "<b><span style='color:red'>Store Error Protocol Disabled</span></b>";
                    session["X-ResponseComments"] = "<b><span style='color:red'>Store Error Protocol disabled found in response body.</span></b>"
                        + "Expect user to <b>NOT be able to connect using connecting client application.</b>.";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 200 Store Error Protocol Disabled.");

                    session["X-ResponseCodeDescription"] = "200 OK - <b><span style='color:red'>PROTOCOL DISABLED</span></b>";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtention: " + session.id + " HTTP 200 Outlook MAPI traffic return.");

                    // Absolute certainly we don't want to do anything further with this session.
                    Preferences.SetSACL(session, "10");
                    Preferences.SetSTCL(session, "10");
                    Preferences.SetSRSCL(session, "10");
                }
                else
                {
                    session["ui-backcolor"] = Preferences.HTMLColourGreen;
                    session["ui-color"] = "black";

                    session["X-SessionType"] = "Outlook MAPI";

                    session["X-ResponseAlert"] = "Outlook for Windows MAPI traffic";
                    session["X-ResponseComments"] = "This is normal Outlook MAPI over HTTP traffic to an Exchange Online mailbox.";

                    // No FiddlerApplication logging here.

                    session["X-ResponseCodeDescription"] = "200 OK";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtention: " + session.id + " HTTP 200 Outlook MAPI traffic.");

                    // Possible something more to be found, let further processing try to pick up something.
                    Preferences.SetSACL(session, "5");
                    Preferences.SetSTCL(session, "5");
                    Preferences.SetSRSCL(session, "5");
                }
            }
        }

        public void HTTP_200_Outlook_RPC(Session session)
        {
            /////////////////////////////
            //
            // 200.3. Outlook RPC traffic.
            //

            // Guessing at this time Outlook's RPC over HTTP looks like this when connected to an Exchange On-Premise mailbox.
            // *Need to validate*
            if (session.uriContains("/rpc/emsmdb/"))
            {
                session["ui-backcolor"] = Preferences.HTMLColourGreen;
                session["ui-color"] = "black";

                session["X-SessionType"] = "Outlook RPC";

                session["X-ResponseAlert"] = "Outlook for Windows RPC traffic";
                session["X-ResponseComments"] = "This is normal Outlook RPC over HTTP traffic to an Exchange On-Premise mailbox.";

                // No FiddlerApplication logging here.

                session["X-ResponseCodeDescription"] = "200 OK";

                FiddlerApplication.Log.LogString("Office365FiddlerExtention: " + session.id + " HTTP 200 Outlook RPC traffic break.");

                // Possible something more to be found, let further processing try to pick up something.
                Preferences.SetSACL(session, "5");
                Preferences.SetSTCL(session, "5");
                Preferences.SetSRSCL(session, "5");
            }
        }
    }
}
