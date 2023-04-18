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
            this.session = session;

            /////////////////////////////
            //
            // 200.1. Connection blocked by Client Access Rules.
            // 

            if (this.session.fullUrl.Contains("outlook.office365.com/mapi")
                && this.session.utilFindInResponse("Connection blocked by Client Access Rules", false) > 1)
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200.1 Connection blocked by Client Access Rules.");

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Red");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK");

                GetSetSessionFlags.Instance.SetSessionType(this.session, "!CLIENT ACCESS RULE!");

                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:red'>CLIENT ACCESS RULE</span></b>");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "<b><span style='color:red'>A client access rule has blocked MAPI connectivity to the mailbox</span></b>. "
                    + "<p>Check if the <b><span style='color:red'>client access rule includes OutlookAnywhere</span></b>.</p>"
                    + "<p>Per <a href='https://docs.microsoft.com/en-us/exchange/clients-and-mobile-in-exchange-online/client-access-rules/client-access-rules' target='_blank'>"
                    + "https://docs.microsoft.com/en-us/exchange/clients-and-mobile-in-exchange-online/client-access-rules/client-access-rules </a>, <br />"
                    + "OutlookAnywhere includes MAPI over HTTP.<p>"
                    + "<p>Remove OutlookAnywhere from the client access rule, wait 1 hour, then test again.</p>");

                // Set confidence level for Session Authentication, Session Type, and Session Response Server.
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
        }

        public void HTTP_200_Outlook_Mapi_Microsoft365_Protocol_Disabled(Session session)
        {
            /////////////////////////////
            //
            // 200.2. Outlook MAPI traffic.
            //

            // Microsoft365 MAPI traffic, protocol disabled.

            // If this session has already been classified with a confidence of 10. Return.
            if (GetSetSessionFlags.Instance.GetSessionTypeConfidenceLevel(this.session) == 10)
            {
                return;
            }

            // If this isn't Office 365 MAPI traffic, return.
            if (!this.session.HostnameIs("outlook.office365.com") && (!this.session.uriContains("/mapi/emsmdb/?MailboxId=")))
            {
                return;
            }

            // If we don't find "ProtocolDisabled" in the response body, return.
            if (!(this.session.utilFindInResponse("ProtocolDisabled", false) > 1))
            {
                return;
            }

            /////////////////////////////
            //
            // Protocol Disabled.
            //

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 Store Error Protocol Disabled.");

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Red");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK - <b><span style='color:red'>PROTOCOL DISABLED</span></b>");

            GetSetSessionFlags.Instance.SetSessionType(this.session, "***PROTOCOL DISABLED***");

            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:red'>Store Error Protocol Disabled</span></b>");
            GetSetSessionFlags.Instance.SetXResponseComments(this.session, "<b><span style='color:red'>Store Error Protocol disabled found in response body.</span></b>"
                + "Expect user to <b>NOT be able to connect using connecting client application.</b>.");

            // Absolute certainly we don't want to do anything further with this session.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "10");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "10");

        }

        public void HTTP_200_Outlook_Exchange_Online_Microsoft_365_Mapi(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            // 200.2. Outlook MAPI traffic.
            //

            // Microsoft 365 normal working MAPI traffic.

            // If this session has already been classified with a confidence of 10. Return.
            if (GetSetSessionFlags.Instance.GetSessionTypeConfidenceLevel(this.session) == 10)
            {
                return;
            }

            // If the session hostname isn't outlook.office365.com and isn't MAPI traffic, return.
            if (!this.session.HostnameIs("outlook.office365.com") && (!this.session.uriContains("/mapi/emsmdb/?MailboxId=")))
            {
                return;
            }

            FiddlerApplication.Log.LogString("Office365FiddlerExtention: " + this.session.id + " HTTP 200 Outlook Exchange Online / Microsoft365 MAPI traffic.");

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Green");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK");

            GetSetSessionFlags.Instance.SetSessionType(this.session, "Outlook M365 MAPI");
            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "Outlook for Windows M365 MAPI traffic");
            GetSetSessionFlags.Instance.SetXResponseComments(this.session, "This is normal Outlook MAPI over HTTP traffic to an Exchange Online / Microsoft365 mailbox.");

            // Possible something more to be found, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
        }

        public void HTTP_200_Outlook_Exchange_OnPremise_Mapi(Session session)
        {
            // Exchange On-Premise mailbox.
            this.session = session;

            // If this session has already been classified with a confidence of 10. Return.
            if (GetSetSessionFlags.Instance.GetSessionTypeConfidenceLevel(this.session) == 10)
            {
                return;
            }

            // If the session isn't MAPI traffic, return.
            if (!this.session.uriContains("/mapi/emsmdb/?MailboxId="))
            {
                return;
            }

            FiddlerApplication.Log.LogString("Office365FiddlerExtention: " + this.session.id + " HTTP 200 Outlook Exchange OnPremise MAPI traffic.");

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Green");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK");

            GetSetSessionFlags.Instance.SetSessionType(this.session, "Outlook MAPI");
            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "Outlook for Windows MAPI traffic");
            GetSetSessionFlags.Instance.SetXResponseComments(this.session, "This is normal Outlook MAPI over HTTP traffic to an Exchange OnPremise mailbox.");

            // Possible something more to be found, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
        }

        public void HTTP_200_Outlook_RPC(Session session)
        {
            this.session = session;

            // If this session has already been classified with a confidence of 10. Return.
            if (GetSetSessionFlags.Instance.GetSessionTypeConfidenceLevel(this.session) == 10)
            {
                return;
            }

            // If the session isn't RPC traffic, return.
            if (!this.session.uriContains("/rpc/emsmdb/"))
            {
                return;
            }

            /////////////////////////////
            //
            // 200.3. Outlook RPC traffic.
            //

            // Guessing at this time Outlook's RPC over HTTP looks like this when connected to an Exchange On-Premise mailbox.
            // REVIEW THIS *Need to validate*
            
            FiddlerApplication.Log.LogString("Office365FiddlerExtention: " + this.session.id + " HTTP 200 Outlook RPC traffic break.");

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Green");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK");

            GetSetSessionFlags.Instance.SetSessionType(this.session, "Outlook RPC");
            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "Outlook for Windows RPC traffic");
            GetSetSessionFlags.Instance.SetXResponseComments(this.session, "This is normal Outlook RPC over HTTP traffic to an Exchange On-Premise mailbox.");

            // Possible something more to be found, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
            
        }

        public void HTTP_200_Outlook_NSPI(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            // 200.4. Outlook Name Service Provider Interface (NSPI) traffic.
            //

            // If this session has already been classified with a confidence of 10. Return.
            if (GetSetSessionFlags.Instance.GetSessionTypeConfidenceLevel(this.session) == 10)
            {
                return;
            }

            // If this isn't NSPI traffic, return.
            if (!this.session.uriContains("/mapi/nspi/"))
            {
                return;
            }
            
            FiddlerApplication.Log.LogString("Office365FiddlerExtention: " + this.session.id + " HTTP 200 Outlook NSPI traffic.");

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Green");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK");

            GetSetSessionFlags.Instance.SetSessionType(this.session, "Outlook NSPI");
            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "Outlook for Windows NSPI traffic");
            GetSetSessionFlags.Instance.SetXResponseComments(this.session, "This is normal Outlook traffic to an Exchange On-Premise mailbox. Name Service Provider Interface (NSPI).");

            // Possible something more to be found, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
        }

        public void HTTP_200_OnPremise_AutoDiscover_Redirect_Address_Found(Session session)
        {
            this.session = session;

            // If this session has already been classified with a confidence of 10. Return.
            if (GetSetSessionFlags.Instance.GetSessionTypeConfidenceLevel(this.session) == 10)
            {
                return;
            }

            // if this session does not have redirectAddr in the response body, return.
            if (!(this.session.utilFindInResponse("<Action>redirectAddr</Action>", false) > 1))
            {
                return;
            }

            // If this autodiscover redirect is from Microsoft 365, return.
            if (this.session.HostnameIs("autodiscover-s.outlook.com") || this.session.HostnameIs("autodiscover.outlook.com"))
            {
                return;
            }

            /////////////////////////////
            // 200.5. Exchange On-Premise Autodiscover redirect.


            /*
            <?xml version="1.0" encoding="utf-8"?>
            <Autodiscover xmlns="http://schemas.microsoft.com/exchange/autodiscover/responseschema/2006">
                <Response xmlns="http://schemas.microsoft.com/exchange/autodiscover/outlook/responseschema/2006a">
                <Account>
                    <Action>redirectAddr</Action>
                    <RedirectAddr>user@contoso.mail.onmicrosoft.com</RedirectAddr>       
                </Account>
                </Response>
            </Autodiscover>
            */

            // Logic to detected the redirect address in this session.
            // 
            string RedirectResponseBody = this.session.GetResponseBodyAsString();
            int start = this.session.GetResponseBodyAsString().IndexOf("<RedirectAddr>");
            int end = this.session.GetResponseBodyAsString().IndexOf("</RedirectAddr>");
            int charcount = end - start;
            string RedirectAddress;

            if (charcount > 0)
            {
                RedirectAddress = RedirectResponseBody.Substring(start, charcount).Replace("<RedirectAddr>", "");
            }
            else
            {
                RedirectAddress = "Redirect address not found by extension.";
            }

            if (RedirectAddress.Contains(".onmicrosoft.com"))
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtention: " + this.session.id + " Exchange OnPremise Autodiscover redirect to Exchange Online / Microsoft365.");

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Green");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK");

                GetSetSessionFlags.Instance.SetSessionType(this.session, "On-Prem AutoD Redirect");
                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "Exchange On-Premise Autodiscover redirect.");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "Exchange On-Premise Autodiscover redirect address to Exchange Online found."
                    + "<p>RedirectAddress: "
                    + RedirectAddress
                    + "</p><p>This is what we want to see, the mail.onmicrosoft.com redirect address (you may know this as the <b>target address</b> or "
                    + "<b>remote routing address</b>) from On-Premise sends Outlook (MSI / Perpetual license) to Office 365 / Exchange Online.</p>");

                // Set confidence level for Session Authentication, Session Type, and Session Response Server.
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
            else
            {
                // Highlight if we got this far and we don't have a redirect address which points to
                // Exchange Online / Microsoft365 such as: contoso.mail.onmicrosoft.com.
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 Exchange On-Premise AUTOD REDIRECT ADDR! : " + RedirectAddress);

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Red");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK, Incorrect Redirect Address!");

                GetSetSessionFlags.Instance.SetSessionType(this.session, "!AUTOD REDIRECT ADDR!");
                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "!Exchange On-Premise Autodiscover redirect!");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "Exchange On-Premise Autodiscover redirect address found, which does not contain .onmicrosoft.com." +
                    "<p>RedirectAddress: " + RedirectAddress +
                    "</p><p>If this is an Office 365 mailbox the <b>targetAddress from On-Premise is not sending Outlook to Office 365</b>!</p>");

                // Set confidence level for Session Authentication, Session Type, and Session Response Server.
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
        }

        public void HTTP_200_OnPremise_AutoDiscover_Redirect_AddressNotFound(Session session)
        {
            this.session = session;

            // If this session has already been classified with a confidence of 10. Return.
            if (GetSetSessionFlags.Instance.GetSessionTypeConfidenceLevel(this.session) == 10)
            {
                return;
            }

            // If this autodiscover redirect is from Microsoft 365, return.
            if (this.session.HostnameIs("autodiscover-s.outlook.com") || this.session.HostnameIs("autodiscover.outlook.com"))
            {
                return;
            }

            /////////////////////////////
            //
            // 200.6. Exchange On-Premise Autodiscover redirect - address can't be found
            //
            if ((this.session.utilFindInResponse("<Message>The email address can't be found.</Message>", false) > 1) &&
                (this.session.utilFindInResponse("<ErrorCode>500</ErrorCode>", false) > 1))
            {
                /*
                <?xml version="1.0" encoding="utf-8"?>
                <Autodiscover xmlns="http://schemas.microsoft.com/exchange/autodiscover/responseschema/2006">
                    <Response>
                    <Error Time="12:03:32.8803744" Id="2422600485">
                        <ErrorCode>500</ErrorCode>
                        <Message>The email address can't be found.</Message>
                        <DebugData />
                    </Error>
                    </Response>
                </Autodiscover>
                */
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 Exchange On-Premise redirect address. Error code 500: The email address can't be found.");

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Red");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK, !Email address not found!");

                GetSetSessionFlags.Instance.SetSessionType(this.session, "!NO AUTOD REDIRECT ADDR!");
                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:red'>Exchange On-Premise Autodiscover Redirect</span></b>");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "Exchange On-Premise Autodiscover redirect address can't be found. "
                    + "Look for other On-Premise Autodiscover responses, we may have a "
                    + "valid Autodiscover targetAddress from On-Premise in another session in this trace."
                    + "Seeing some redirects return a HTTP 500 from Exchange OnPremise have been seen in a normal, working Outlook "
                    + "client which can connect to the Exchange Online mailbox.");

                // Set confidence level for Session Authentication, Session Type, and Session Response Server.
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
        }

        public void HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun(Session session)
        {
            this.session = session;

            // If this session has already been classified with a confidence of 10. Return.
            if (GetSetSessionFlags.Instance.GetSessionTypeConfidenceLevel(this.session) == 10)
            {
                return;
            }

            // If this session isn't a Autodiscover session, return; 
            if (!this.session.uriContains("autodiscover.xml"))
            {
                return;
            }

            /////////////////////////////
            //
            // 200.7. Exchange Online Autodiscover
            //

            // Make sure this session is an Exchange Online Autodiscover request.
            // Non-ClickToRun clients redirect to https://autodiscover-s.outlook.com/Autodiscover/AutoDiscover.xml
            if ((this.session.hostname == "autodiscover-s.outlook.com") && (this.session.uriContains("autodiscover.xml")))
            {
                if ((this.session.utilFindInResponse("<DisplayName>", false) > 1) &&
                    (this.session.utilFindInResponse("<MicrosoftOnline>", false) > 1) &&
                    (this.session.utilFindInResponse("<MailStore>", false) > 1) &&
                    (this.session.utilFindInResponse("<ExternalUrl>", false) > 1))
                {
                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 Exchange Online / Outlook MSI Autodiscover. Expected XML found.");

                    GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Green");
                    GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                    GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK");

                    GetSetSessionFlags.Instance.SetSessionType(this.session, "EXO MSI Autodiscover");
                    GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "Exchange Online / Outlook MSI Autodiscover.");
                    GetSetSessionFlags.Instance.SetXResponseComments(this.session, "For Autodiscover calls which go to autodiscover-s.outlook.com this is likely an Outlook (MSI / perpetual license) client"
                        + " being redirected from Exchange On-Premise to Exchange Online.");

                    // Possible something more to be found, let further processing try to pick up something.
                    GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                    GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "5");
                    GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
                }
                else
                {
                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 Exchange Online / Outlook MSI Autodiscover. Expected XML NOT found!");

                    GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Red");
                    GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                    GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK, Unexpected AutoDiscover XML response.");

                    GetSetSessionFlags.Instance.SetSessionType(this.session, "!EXO MSI Autodiscover!");
                    GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:red'>Exchange Online / Outlook MSI Autodiscover - Unusual Autodiscover Response</span></b>");
                    GetSetSessionFlags.Instance.SetXResponseComments(this.session, "This session was detected as an Autodiscover response from Exchange Online. However the response did not contain "
                        + "the expected XML data. Check if a device in-between the perimeter of your network and the client computer can / has altered the data in the response.");

                    // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                    GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                    GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
                    GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
                }
            }
        }

        public void HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun(Session session)
        {
            this.session = session;

            // If this session has already been classified with a confidence of 10. Return.
            if (GetSetSessionFlags.Instance.GetSessionTypeConfidenceLevel(this.session) == 10)
            {
                return;
            }

            // If this session isn't a Autodiscover session, return; 
            if (!this.session.uriContains("autodiscover.xml"))
            {
                return;
            }

            // ClickToRun clients use to https://outlook.office365.com/Autodiscover/AutoDiscover.xml.
            if ((this.session.hostname == "outlook.office365.com") && (this.session.uriContains("autodiscover.xml")))
            {
                if ((this.session.utilFindInResponse("<DisplayName>", false) > 1) &&
                    (this.session.utilFindInResponse("<MicrosoftOnline>", false) > 1) &&
                    (this.session.utilFindInResponse("<MailStore>", false) > 1) &&
                    (this.session.utilFindInResponse("<ExternalUrl>", false) > 1))
                {
                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 Exchange Online / Outlook CTR Autodiscover. Expected XML found.");

                    GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Green");
                    GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                    GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK");

                    GetSetSessionFlags.Instance.SetSessionType(this.session, "EXO CTR Autodiscover");
                    GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "Exchange Online / Outlook CTR Autodiscover.");
                    GetSetSessionFlags.Instance.SetXResponseComments(this.session, "For Autodiscover calls which go to outlook.office365.com this is likely an Outlook Click-To-Run (Downloaded or "
                        + "deployed from Office365) client being redirected from Exchange On-Premise to Exchange Online.");

                    // Possible something more to be found, let further processing try to pick up something.
                    GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                    GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "5");
                    GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
                }
                else
                {
                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 Exchange Online / Outlook CTR Autodiscover. Expected XML NOT found!");

                    GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Red");
                    GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                    GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK, !Unexpected XML response.!");

                    GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:red'>Exchange Online / Outlook CTR Autodiscover - Unusual Autodiscover Response</span></b>");
                    GetSetSessionFlags.Instance.SetXResponseComments(this.session, "This session was detected as an Autodiscover response from Exchange Online. However the response did not contain "
                        + "the expected XML data. Check if a device in-between the perimeter of your network and the client computer can / has altered the data in the response.");

                    // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                    GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                    GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
                    GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
                }
            }
        }

        public void HTTP_200_Unified_Groups_Settings(Session session)
        {
            this.session = session;

            // If this session has already been classified with a confidence of 10. Return.
            if (GetSetSessionFlags.Instance.GetSessionTypeConfidenceLevel(this.session) == 10)
            {
                return;
            }
            
            // If this session isn't for Microsoft 365 Unified Group Settings, return.
            if (!this.session.HostnameIs("outlook.office365.com") &&
                (!this.session.uriContains("ews/exchange.asmx") &&
                (!(this.session.utilFindInRequest("GetUnifiedGroupsSettings", false) > 1))))
            {
                return;
            }

            /////////////////////////////
            //
            // 200.8. GetUnifiedGroupsSettings EWS call.
            //

            // User can create Office 365 gropus.
            if (this.session.utilFindInResponse("<GroupCreationEnabled>true</GroupCreationEnabled>", false) > 1)
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 GetUnifiedGroupsSettings EWS call. User can create O365 Groups in Outlook.");

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Green");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK");

                GetSetSessionFlags.Instance.SetSessionType(this.session, "EWS GetUnifiedGroupsSettings");
                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "GetUnifiedGroupsSettings EWS call.");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "<GroupCreationEnabled>true</GroupCreationEnabled> found in response body. "
                    + "Expect user to be able to create Office 365 groups in Outlook.");

                // Possible something more to be found, let further processing try to pick up something.
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "5");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
            // User cannot create Office 365 groups. Not an error condition in and of itself.
            else if (this.session.utilFindInResponse("<GroupCreationEnabled>false</GroupCreationEnabled>", false) > 1)
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 GetUnifiedGroupsSettings EWS call. User cannot create O365 Groups in Outlook.");

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Green");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK, User cannot create Unified Groups.");

                GetSetSessionFlags.Instance.SetSessionType(this.session, "EWS GetUnifiedGroupsSettings");

                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:red'>GetUnifiedGroupsSettings EWS call</span></b>");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "<GroupCreationEnabled>false</GroupCreationEnabled> found in response body. "
                    + "Expect user to <b>NOT be able to create Office 365 groups</b> in Outlook.");

                // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
            // Did not see the expected keyword in the response body. This is the error condition.
            else
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 GetUnifiedGroupsSettings!");

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Red");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK, GetUnifiedGroupsSettings not found.");

                GetSetSessionFlags.Instance.SetSessionType(this.session, "!EWS GetUnifiedGroupsSettings!");

                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "GetUnifiedGroupsSettings EWS call");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "Though GetUnifiedGroupsSettings scenario was detected neither <GroupCreationEnabled>true</GroupCreationEnabled> or"
                    + "<GroupCreationEnabled>false</GroupCreationEnabled> was found in the response body. Check the Raw tab for more details.");

                // Possible something more to be found, let further processing try to pick up something.
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "5");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
        }

        public void HTTP_200_3S_Suggestions(Session session)
        {
            this.session = session;

            // If this session has already been classified with a confidence of 10. Return.
            if (GetSetSessionFlags.Instance.GetSessionTypeConfidenceLevel(this.session) == 10)
            {
                return;
            }

            // If this isn't a 3G Suggestions call, return.
            if (!this.session.uriContains("search/api/v1/suggestions"))
            {
                return;
            }

            /////////////////////////////
            //
            // 200.9. 3S Suggestions call.
            //
            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " 200 3S Suggestions call.");

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Green");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK");

            GetSetSessionFlags.Instance.SetSessionType(this.session, "3S Suggestions");

            Uri uri = new Uri(this.session.fullUrl);
            var queryStrings = System.Web.HttpUtility.ParseQueryString(uri.Query);
            var scenario = queryStrings["scenario"] ?? "scenario not specified in url";
            var entityTypes = queryStrings["entityTypes"] ?? "entityTypes not specified in url";
            var clientRequestId = this.session.RequestHeaders.Where(x => x.Name.Equals("client-request-id")).FirstOrDefault();

            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "3S Suggestions");
            GetSetSessionFlags.Instance.SetXResponseComments(this.session, $"Scenario: {scenario} Types: {entityTypes} {clientRequestId}");

            // Possible something more to be found, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
        }

        public void HTTP_200_REST_People_Request(Session session)
        {
            this.session = session;

            // If this session has already been classified with a confidence of 10. Return.
            if (GetSetSessionFlags.Instance.GetSessionTypeConfidenceLevel(this.session) == 10)
            {
                return;
            }

            // if the session Uri isn't for People, return;
            if (!this.session.uriContains("people"))
            {
                return;
            }

            /////////////////////////////
            //
            // 200.10. REST - People Request.
            //

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " 200 REST - People Request.");

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Green");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK");

            Uri uri = new Uri(this.session.fullUrl);
            var queryStrings = System.Web.HttpUtility.ParseQueryString(uri.Query);

            string sessionType = "";

            // /me/people : : Private FindPeople Request
            if (this.session.uriContains("/me/people"))
            {
                sessionType = "Private";
            }

            // /users()/people : Public FindPeople Request
            else if (this.session.uriContains("/users(") && this.session.uriContains("/people"))
            {
                sessionType = "Public";
            }

            var requestId = this.session.ResponseHeaders.Where(x => x.Name.Equals("request-id")).FirstOrDefault();

            GetSetSessionFlags.Instance.SetSessionType(this.session, $"REST People {sessionType}");
            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, $"REST People {sessionType}");
            GetSetSessionFlags.Instance.SetXResponseComments(this.session, $"{requestId} $search:{queryStrings["$search"]} $top:{queryStrings["$top"]} $skip:{queryStrings["$skip"]} $select:{queryStrings["$select"]} $filter:{queryStrings["$filter"]}");

            // Possible something more to be found, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
        }

        public void HTTP_200_Any_Other_Exchange_EWS(Session session)
        {
            this.session = session;

            // If this session has already been classified with a confidence of 10. Return.
            if (GetSetSessionFlags.Instance.GetSessionTypeConfidenceLevel(this.session) == 10)
            {
                return;
            }

            // If this isn't an EWS call, return.
            if (!this.session.uriContains("ews/exchange.asmx")) {
                return;
            }

            /////////////////////////////
            //
            // 200.11. Any other EWS call.
            //
            if (this.session.HostnameIs("outlook.office365.com"))
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 EXO / M365 EWS call.");

                GetSetSessionFlags.Instance.SetSessionType(this.session, "Exchange Online / Microsoft365 Web Services");

                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "Exchange Online / Microsoft365 Web Services (EWS) call.");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "Exchange Online / Microsoft365 Web Services (EWS) call.");
            }
            else
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 OnPremise EWS call.");
                
                GetSetSessionFlags.Instance.SetSessionType(this.session, "Exchange OnPremise Web Services");

                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "Exchange OnPremise Web Services (EWS) call.");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "Exchange OnPremise Web Services (EWS) call.");
            }

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Green");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK");

            // Possible something more to be found, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
        }

        public void HTTP_200_Lurking_Errors(Session session)
        {
            this.session = session;

            // If this session has already been classified with a confidence of 10. Return.
            if (GetSetSessionFlags.Instance.GetSessionTypeConfidenceLevel(this.session) == 10)
            {
                return;
            }

            string searchTerm = "empty";

            /////////////////////////////
            //
            // 200.99. All other specific scenarios, fall back to looking for errors lurking in HTTP 200 OK responses.

            int wordCountError = 0;
            int wordCountFailed = 0;
            int wordCountException = 0;

            string wordCountErrorText;
            string wordCountFailedText;
            string wordCountExceptionText;

            // Count the occurrences of common search terms match up to certain HTTP response codes to highlight certain scenarios.
            //
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-count-occurrences-of-a-word-in-a-string-linq
            //

            string text200 = this.session.ToString();

            // Convert the string into an array of words
            // 7/15/2021 Added '"' to split out "Error" and count these.
            string[] source200 = text200.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',', '"' }, StringSplitOptions.RemoveEmptyEntries);

            // Create the query. Use ToLowerInvariant to match "data" and "Data"   
            var matchQuery200 = from word in source200
                                where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                                select word;

            searchTerm = "Error";

            // Count the matches, which executes the query.  
            wordCountError = matchQuery200.Count();

            searchTerm = "failed";

            // Count the matches, which executes the query.  
            wordCountFailed = matchQuery200.Count();

            searchTerm = "exception";

            // Count the matches, which executes the query.  
            wordCountException = matchQuery200.Count();

            // If either the keyword searches give us a result.
            if (wordCountError > 0 || wordCountFailed > 0 || wordCountException > 0)
            {
                if (wordCountError == 0)
                {
                    wordCountErrorText = $"<b><span style='color:green'>Keyword 'Error' found {wordCountError} times.</span></b>";
                }
                else if (wordCountError == 1)
                {
                    wordCountErrorText = $"<b><span style='color:red'>Keyword 'Error' found {wordCountError} time.</span></b>";
                }
                else
                {
                    wordCountErrorText = $"<b><span style='color:red'>Keyword 'Error' found {wordCountError} times.</span></b>";
                }

                if (wordCountFailed == 0)
                {
                    wordCountFailedText = $"<b><span style='color:green'>Keyword 'Failed' found {wordCountFailed} times.</span></b>";
                }
                else if (wordCountFailed == 1)
                {
                    wordCountFailedText = $"<b><span style='color:red'>Keyword 'Failed' found {wordCountFailed} time.</span></b>";
                }
                else
                {
                    wordCountFailedText = $"<b><span style='color:red'>Keyword 'Failed' found {wordCountFailed} times.</span></b>";
                }

                if (wordCountException == 0)
                {
                    wordCountExceptionText = $"<b><span style='color:green'>Keyword 'Exception' found {wordCountException} times.</span></b>";
                }
                else if (wordCountException == 1)
                {
                    wordCountExceptionText = $"<b><span style='color:red'>Keyword 'Exception' found {wordCountException} time.</span></b>";
                }
                else
                {
                    wordCountExceptionText = $"<b><span style='color:red'>Keyword 'Exception' found {wordCountException} times.</span></b>";
                }

                // Special attention to HTTP 200's where the keyword 'error' or 'failed' is found.
                // Red text on black background.
                // The only issue here is when sessions contain javascript and other web source code, this tends to produce false positives.

                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 FAILURE LURKING!?");

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Black");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "Red");

                GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK, but possibly bad.");

                GetSetSessionFlags.Instance.SetSessionType(this.session, "!FAILURE LURKING!");

                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:red'>'error', 'failed' or 'exception' found in response body</span></b>");
                // REVIEW THIS.
                // There was a += on this XResponseComments. This probably means the response comments were being combined with other detections.
                // Something to think about and come back to.
                // REVIEW THIS.
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "<p>Session response body was scanned and errors or failures were found in response body. "
                    + "Check the Raw tab, click 'View in Notepad' button bottom right, and search for error in the response to review.</p>"
                    + "<p>After splitting all words in the response body the following were found:</p>"
                    + "<p>" + wordCountErrorText + "</p>"
                    + "<p>" + wordCountFailedText + "</p>"
                    + "<p>" + wordCountExceptionText + "</p>"
                    + "<p>Check the content body of the response for any failures you recognise. You may find <b>false positives, "
                    + "if lots of Javascript or other web code</b> is being loaded.</p>");

                // Possible something more to be found, let further processing try to pick up something.
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "5");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
            else
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 OK");

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Green");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK");

                GetSetSessionFlags.Instance.SetSessionType(this.session, "200 OK");

                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "HTTP 200 OK, with no errors, failed, or exceptions found.");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "HTTP 200 OK, with no errors, failed, or exceptions found.");

                // Possible something more to be found, let further processing try to pick up something.
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "5");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
        }
    }
}