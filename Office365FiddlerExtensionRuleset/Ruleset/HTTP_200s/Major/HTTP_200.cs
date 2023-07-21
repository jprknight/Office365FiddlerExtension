﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;
using System.Web;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_200
    {
        internal Session session { get; set; }

        private static HTTP_200 _instance;

        public static HTTP_200 Instance => _instance ?? (_instance = new HTTP_200());

        /// <summary>
        /// Connection blocked by Client Access Rules.
        /// </summary>
        /// <param name="session"></param>
        public void HTTP_200_ClientAccessRule(Session session)
        {
            this.session = session;

            // If the session content doesn't match the intended rule, return.
            if (!this.session.fullUrl.Contains("outlook.office365.com/mapi"))
            {
                return;
            }

            if (!(this.session.utilFindInResponse("Connection blocked by Client Access Rules", false) > 1))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Connection blocked by Client Access Rules.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_Client_Access_Rule",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "!CLIENT ACCESS RULE!",
                ResponseCodeDescription = "200 OK Client Access Rule",
                ResponseAlert = "<b><span style='color:red'>CLIENT ACCESS RULE</span></b>",
                ResponseComments = "<b><span style='color:red'>A client access rule has blocked MAPI connectivity to the mailbox</span></b>. "
                + "<p>Check if the <b><span style='color:red'>client access rule includes OutlookAnywhere</span></b>.</p>"
                + "<p>Per <a href='https://docs.microsoft.com/en-us/exchange/clients-and-mobile-in-exchange-online/client-access-rules/client-access-rules' target='_blank'>"
                + "https://docs.microsoft.com/en-us/exchange/clients-and-mobile-in-exchange-online/client-access-rules/client-access-rules </a>, <br />"
                + "OutlookAnywhere includes MAPI over HTTP.<p>"
                + "<p>Remove OutlookAnywhere from the client access rule, wait 1 hour, then test again.</p>",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 100
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            
        }

        /// <summary>
        /// Microsoft365 Outlook MAPI traffic, protocol disabled.
        /// </summary>
        /// <param name="session"></param>
        public void HTTP_200_Outlook_Mapi_Microsoft365_Protocol_Disabled(Session session)
        {
            this.session = session;

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

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Store Error Protocol Disabled.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_Mapi_Protocol_Disabled",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "***PROTOCOL DISABLED***",
                ResponseCodeDescription = "200 OK - <b><span style='color:red'>PROTOCOL DISABLED</span></b>",
                ResponseAlert = "<b><span style='color:red'>Store Error Protocol Disabled</span></b>",
                ResponseComments = "<b><span style='color:red'>Store Error Protocol disabled found in response body.</span></b>"
                + "Expect user to <b>NOT be able to connect using connecting client application.</b>.",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 100
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        /// <summary>
        /// Microsoft 365 normal working MAPI traffic.
        /// </summary>
        /// <param name="session"></param>
        public void HTTP_200_Outlook_Exchange_Online_Microsoft_365_Mapi(Session session)
        {
            this.session = session;

            // If the session hostname isn't outlook.office365.com and isn't MAPI traffic, return.
            if (!this.session.HostnameIs("outlook.office365.com"))
            {
                return;
            }

            if (!this.session.uriContains("/mapi/emsmdb/?MailboxId="))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Outlook Exchange Online / Microsoft365 MAPI traffic.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_Microsoft365_Mapi",
                UIBackColour = "Green",
                UITextColour = "Black",

                SessionType = "Outlook M365 MAPI",
                ResponseCodeDescription = "200 OK Microsoft365 / Exchange Online MAPI",
                ResponseAlert = "Outlook for Windows M365 MAPI traffic",
                ResponseComments = "This is normal Outlook MAPI over HTTP traffic to an Exchange Online / Microsoft365 mailbox.",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        /// <summary>
        /// Exchange On-Premise Mailbox MAPI
        /// </summary>
        /// <param name="session"></param>
        public void HTTP_200_Outlook_Exchange_OnPremise_Mapi(Session session)
        {
            this.session = session;

            // If the session isn't MAPI traffic, return.
            if (!this.session.uriContains("/mapi/emsmdb/?MailboxId="))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Outlook Exchange OnPremise MAPI traffic.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_Exchange_OnPremise_Mapi",
                UIBackColour = "Green",
                UITextColour = "Black",

                SessionType = "Outlook MAPI",
                ResponseCodeDescription = "200 OK Exchange MAPI",
                ResponseAlert = "Outlook for Windows MAPI traffic",
                ResponseComments = "This is normal Outlook MAPI over HTTP traffic to an Exchange OnPremise mailbox.",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        /// <summary>
        /// Outlook Web App.
        /// </summary>
        /// <param name="session"></param>
        public void HTTP_200_Outlook_Web_App(Session session)
        {
            this.session = session;

            // If the session isn't MAPI traffic, return.
            if (!this.session.uriContains("/owa/"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Outlook Web App.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_Exchange_Outlook_Web_App",
                UIBackColour = "Green",
                UITextColour = "Black",

                SessionType = "Outlook Web App",
                ResponseCodeDescription = "200 OK Outlook Web App / OWA",
                ResponseAlert = "Outlook Web App",
                ResponseComments = "This is normal Outlook Web App traffic to an Exchange mailbox.",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        /// <summary>
        /// Outlook RPC.
        /// </summary>
        /// <param name="session"></param>
        public void HTTP_200_Outlook_RPC(Session session)
        {
            this.session = session;

            // If the session isn't RPC traffic, return.
            if (!this.session.uriContains("/rpc/emsmdb/"))
            {
                return;
            }

            // Outlook RPC traffic.

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Outlook RPC traffic break.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_Outlook_RPC",
                UIBackColour = "Green",
                UITextColour = "Black",

                SessionType = "Outlook RPC",
                ResponseCodeDescription = "200 OK Outlook over RPC",
                ResponseAlert = "Outlook for Windows RPC traffic",
                ResponseComments = "This is normal Outlook RPC over HTTP traffic to an Exchange On-Premise mailbox.",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 5,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);          
        }

        /// <summary>
        /// Outlook Name Service Provider Interface (NSPI) traffic.
        /// </summary>
        /// <param name="session"></param>
        public void HTTP_200_Outlook_NSPI(Session session)
        {
            this.session = session;

            // If this isn't NSPI traffic, return.
            if (!this.session.uriContains("/mapi/nspi/"))
            {
                return;
            }
            
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " + this.session.id + " HTTP 200 Outlook NSPI traffic.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_Outlook_NSPI",
                UIBackColour = "Green",
                UITextColour = "Black",

                SessionType = "Outlook NSPI",
                ResponseCodeDescription = "200 OK Outlook NSPI",
                ResponseAlert = "Outlook for Windows NSPI traffic",
                ResponseComments = "This is normal Outlook traffic to an Exchange On-Premise mailbox. Name Service Provider Interface (NSPI).",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        /// <summary>
        /// Exchange OnPremise AutoDiscover Redirect Address Found.
        /// </summary>
        /// <param name="session"></param>
        public void HTTP_200_OnPremise_AutoDiscover_Redirect_Address_Found(Session session)
        {
            this.session = session;

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
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " + this.session.id + " Exchange OnPremise Autodiscover redirect to Exchange Online / Microsoft365.");

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s_Redirect_Address",
                    UIBackColour = "Green",
                    UITextColour = "Black",

                    SessionType = "On-Prem AutoD Redirect",
                    ResponseCodeDescription = "200 OK Redirect Address",
                    ResponseAlert = "Exchange On-Premise Autodiscover redirect.",
                    ResponseComments = "Exchange On-Premise Autodiscover redirect address to Exchange Online found."
                    + "<p>RedirectAddress: "
                    + RedirectAddress
                    + "</p><p>This is what we want to see, the mail.onmicrosoft.com redirect address (you may know this as the <b>target address</b> or "
                    + "<b>remote routing address</b>) from On-Premise sends Outlook (MSI / Perpetual license) to Office 365 / Exchange Online.</p>",

                    SessionAuthenticationConfidenceLevel = 5,
                    SessionTypeConfidenceLevel = 10,
                    SessionResponseServerConfidenceLevel = 5,
                    SessionSeverity = 0
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
            else
            {
                // Highlight if we got this far and we don't have a redirect address which points to
                // Exchange Online / Microsoft365 such as: contoso.mail.onmicrosoft.com.
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " + this.session.id + " HTTP 200 Exchange On-Premise AUTOD REDIRECT ADDR! : " + RedirectAddress);

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s_Redirect_Address_Not_Found",
                    UIBackColour = "Red",
                    UITextColour = "Black",

                    SessionType = "!AUTOD REDIRECT ADDR!",
                    ResponseCodeDescription = "200 OK, Incorrect Redirect Address!",
                    ResponseServer = "Fiddler Update Check",
                    ResponseAlert = "!Exchange On-Premise Autodiscover redirect!",
                    ResponseComments = "Exchange On-Premise Autodiscover redirect address found, which does not contain .onmicrosoft.com." +
                    "<p>RedirectAddress: " + RedirectAddress +
                    "</p><p>If this is an Office 365 mailbox the <b>targetAddress from On-Premise is not sending Outlook to Office 365</b>!</p>",

                    SessionAuthenticationConfidenceLevel = 5,
                    SessionTypeConfidenceLevel = 10,
                    SessionResponseServerConfidenceLevel = 5,
                    SessionSeverity = 50
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
        }

        /// <summary>
        /// Exchange OnPremise AutoDiscover Redirect Address Not Found.
        /// </summary>
        /// <param name="session"></param>
        public void HTTP_200_OnPremise_AutoDiscover_Redirect_AddressNotFound(Session session)
        {
            this.session = session;

            // If this autodiscover redirect is from Microsoft 365, return.
            if (this.session.HostnameIs("autodiscover-s.outlook.com") || this.session.HostnameIs("autodiscover.outlook.com"))
            {
                return;
            }
            
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
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Exchange On-Premise redirect address. Error code 500: The email address can't be found.");

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s_Redirect_Address_Not_Found",
                    UIBackColour = "Red",
                    UITextColour = "Black",

                    SessionType = "!NO AUTOD REDIRECT ADDR!",
                    ResponseCodeDescription = "200 OK, !Email address not found!",
                    ResponseServer = "Fiddler Update Check",
                    ResponseAlert = "<b><span style='color:red'>Exchange On-Premise Autodiscover Redirect</span></b>",
                    ResponseComments = "Exchange On-Premise Autodiscover redirect address can't be found. "
                    + "Look for other On-Premise Autodiscover responses, we may have a "
                    + "valid Autodiscover targetAddress from On-Premise in another session in this trace."
                    + "Seeing some redirects return a HTTP 500 from Exchange OnPremise have been seen in a normal, working Outlook "
                    + "client which can connect to the Exchange Online mailbox.",

                    SessionAuthenticationConfidenceLevel = 5,
                    SessionTypeConfidenceLevel = 10,
                    SessionResponseServerConfidenceLevel = 5,
                    SessionSeverity = 50
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
        }

        /// <summary>
        /// Exchange Online / Microsoft 365 AutoDiscover MSI Non-ClickToRun.
        /// </summary>
        /// <param name="session"></param>
        public void HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun(Session session)
        {
            this.session = session;

            // If this session isn't a Autodiscover session, return; 
            if (!this.session.uriContains("autodiscover.xml"))
            {
                return;
            }

            // 200 Exchange Online Autodiscover

            // Make sure this session is an Exchange Online Autodiscover request.
            // Non-ClickToRun clients redirect to https://autodiscover-s.outlook.com/Autodiscover/AutoDiscover.xml
            if ((this.session.hostname == "autodiscover-s.outlook.com") && (this.session.uriContains("autodiscover.xml")))
            {
                if ((this.session.utilFindInResponse("<DisplayName>", false) > 1) &&
                    (this.session.utilFindInResponse("<MicrosoftOnline>", false) > 1) &&
                    (this.session.utilFindInResponse("<MailStore>", false) > 1) &&
                    (this.session.utilFindInResponse("<ExternalUrl>", false) > 1))
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Exchange Online / Outlook MSI Autodiscover. Expected XML found.");

                    var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "HTTP_200s_MSI_AutoDiscover",
                        UIBackColour = "Green",
                        UITextColour = "Black",

                        SessionType = "EXO MSI Autodiscover",
                        ResponseCodeDescription = "200 OK Outlook MSI AutoDiscover",
                        ResponseAlert = "Exchange Online / Outlook MSI AutoDiscover.",
                        ResponseComments = "For AutoDiscover calls which go to autodiscover-s.outlook.com this is likely an Outlook (MSI / perpetual license) client"
                        + " being redirected from Exchange On-Premise to Exchange Online.",

                        SessionAuthenticationConfidenceLevel = 5,
                        SessionTypeConfidenceLevel = 10,
                        SessionResponseServerConfidenceLevel = 5,
                        SessionSeverity = 0
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
                }
                else
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Exchange Online / Outlook MSI Autodiscover. Expected XML NOT found!");

                    var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "HTTP_200s_MSI_AutoDiscover",
                        UIBackColour = "Red",
                        UITextColour = "Black",

                        SessionType = "!EXO MSI AutoDiscover!",
                        ResponseCodeDescription = "200 OK, Unexpected AutoDiscover XML response.",
                        ResponseAlert = "<b><span style='color:red'>Exchange Online / Outlook MSI AutoDiscover - Unusual AutoDiscover Response</span></b>",
                        ResponseComments = "This session was detected as an AutoDiscover response from Exchange Online. However the response did not contain "
                        + "the expected XML data. Check if a device in-between the perimeter of your network and the client computer can / has altered the data in the response.",

                        SessionAuthenticationConfidenceLevel = 5,
                        SessionTypeConfidenceLevel = 10,
                        SessionResponseServerConfidenceLevel = 5,
                        SessionSeverity = 50
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
                }
            }
        }

        /// <summary>
        /// Exchange Online / Microsoft 365 AutoDiscover ClickToRun.
        /// </summary>
        /// <param name="session"></param>
        public void HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun(Session session)
        {
            this.session = session;

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
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Exchange Online / Outlook CTR Autodiscover. Expected XML found.");

                    var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "HTTP_200s_CTR_AutoDiscover",
                        UIBackColour = "Green",
                        UITextColour = "Black",

                        SessionType = "EXO CTR Autodiscover",
                        ResponseCodeDescription = "200 OK",
                        ResponseAlert = "Exchange Online / Outlook CTR AutoDiscover.",
                        ResponseComments = "For AutoDiscover calls which go to outlook.office365.com this is likely an Outlook Click-To-Run (Downloaded or "
                        + "deployed from Office365) client being redirected from Exchange On-Premise to Exchange Online.",

                        SessionAuthenticationConfidenceLevel = 5,
                        SessionTypeConfidenceLevel = 5,
                        SessionResponseServerConfidenceLevel = 5,
                        SessionSeverity = 0
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
                }
                else
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Exchange Online / Outlook CTR Autodiscover. Expected XML NOT found!");

                    var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "HTTP_200s_CTR_AutoDiscover",
                        UIBackColour = "Red",
                        UITextColour = "Black",

                        SessionType = "Outlook AutoDiscover XML NOT found!",
                        ResponseCodeDescription = "200 OK, !Unexpected XML response!",
                        ResponseAlert = "<b><span style='color:red'>Exchange Online / Outlook CTR Autodiscover - Unusual Autodiscover Response</span></b>",
                        ResponseComments = "This session was detected as an Autodiscover response from Exchange Online. However the response did not contain "
                        + "the expected XML data. Check if a device in-between the perimeter of your network and the client computer can / has altered the data in the response.",

                        SessionAuthenticationConfidenceLevel = 5,
                        SessionTypeConfidenceLevel = 10,
                        SessionResponseServerConfidenceLevel = 5,
                        SessionSeverity = 50
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
                }
            }
        }

        /// <summary>
        /// Exchange Online / Microsoft 365 Unified Groups Settings.
        /// </summary>
        /// <param name="session"></param>
        public void HTTP_200_Unified_Groups_Settings(Session session)
        {
            this.session = session;
            
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
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 GetUnifiedGroupsSettings EWS call. User can create O365 Groups in Outlook.");

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s_Unfied_Groups_Settings",
                    UIBackColour = "Green",
                    UITextColour = "Black",

                    SessionType = "EWS GetUnifiedGroupsSettings",
                    ResponseCodeDescription = "200 OK Get Unified Groups Settings",
                    ResponseAlert = "GetUnifiedGroupsSettings EWS call.",
                    ResponseComments = "<GroupCreationEnabled>true</GroupCreationEnabled> found in response body. "
                    + "Expect user to be able to create Office 365 groups in Outlook.",

                    SessionAuthenticationConfidenceLevel = 5,
                    SessionTypeConfidenceLevel = 10,
                    SessionResponseServerConfidenceLevel = 5,
                    SessionSeverity = 0
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
            // User cannot create Office 365 groups. Not an error condition in and of itself.
            else if (this.session.utilFindInResponse("<GroupCreationEnabled>false</GroupCreationEnabled>", false) > 1)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 GetUnifiedGroupsSettings EWS call. User cannot create O365 Groups in Outlook.");

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s_Unified_Groups_Settings",
                    UIBackColour = "Red",
                    UITextColour = "Black",

                    SessionType = "EWS GetUnifiedGroupsSettings",
                    ResponseCodeDescription = "200 OK, User cannot create Unified Groups.",
                    ResponseAlert = "<b><span style='color:red'>GetUnifiedGroupsSettings EWS call</span></b>",
                    ResponseComments = "<GroupCreationEnabled>false</GroupCreationEnabled> found in response body. "
                    + "Expect user to <b>NOT be able to create Office 365 groups</b> in Outlook.",

                    SessionAuthenticationConfidenceLevel = 5,
                    SessionTypeConfidenceLevel = 10,
                    SessionResponseServerConfidenceLevel = 5,
                    SessionSeverity = 50
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
            // Did not see the expected keyword in the response body. This is the error condition.
            else
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 GetUnifiedGroupsSettings!");

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s_Unified_Groups_Settings",
                    UIBackColour = "Green",
                    UITextColour = "Black",

                    SessionType = "!EWS GetUnifiedGroupsSettings!",
                    ResponseCodeDescription = "200 OK, GetUnifiedGroupsSettings not found.",
                    ResponseAlert = "GetUnifiedGroupsSettings EWS call",
                    ResponseComments = "Though GetUnifiedGroupsSettings scenario was detected neither <GroupCreationEnabled>true</GroupCreationEnabled> nor"
                    + "<GroupCreationEnabled>false</GroupCreationEnabled> was found in the response body. Check the Raw tab for more details.",

                    SessionAuthenticationConfidenceLevel = 5,
                    SessionTypeConfidenceLevel = 5,
                    SessionResponseServerConfidenceLevel = 5,
                    SessionSeverity = 25
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
        }

        /// <summary>
        /// 3S Suggestions call.
        /// </summary>
        /// <param name="session"></param>
        public void HTTP_200_3S_Suggestions(Session session)
        {
            this.session = session;

            // If this isn't a 3G Suggestions call, return.
            if (!this.session.uriContains("search/api/v1/suggestions"))
            {
                return;
            }
            
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} 200 3S Suggestions call.");

            Uri uri = new Uri(this.session.fullUrl);
            var queryStrings = HttpUtility.ParseQueryString(uri.Query);
            var scenario = queryStrings["scenario"] ?? "scenario not specified in url";
            var entityTypes = queryStrings["entityTypes"] ?? "entityTypes not specified in url";
            var clientRequestId = this.session.RequestHeaders.Where(x => x.Name.Equals("client-request-id")).FirstOrDefault();

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_3S_Suggestions",
                UIBackColour = "Green",
                UITextColour = "Black",

                SessionType = "3S Suggestions",
                ResponseCodeDescription = "200 OK 3S Suggestions",
                ResponseAlert = "3S Suggestions",
                ResponseComments = $"Scenario: {scenario} Types: {entityTypes} {clientRequestId}",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 5,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        /// <summary>
        /// REST People Request.
        /// </summary>
        /// <param name="session"></param>
        public void HTTP_200_REST_People_Request(Session session)
        {
            this.session = session;

            // if the session Uri isn't for People, return;
            if (!this.session.uriContains("people"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} 200 REST - People Request.");

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

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_REST_People_Request",
                UIBackColour = "Green",
                UITextColour = "Black",

                SessionType = $"REST People {sessionType}",
                ResponseCodeDescription = "200 OK REST People Request",
                ResponseAlert = $"REST People {sessionType}",
                ResponseComments = $"{requestId} $search:{queryStrings["$search"]} $top:{queryStrings["$top"]} $skip:{queryStrings["$skip"]} $select:{queryStrings["$select"]} $filter:{queryStrings["$filter"]}",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 5,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        /// <summary>
        /// Exchange Online / Microsoft 365 Any Other Exchange Web Services.
        /// </summary>
        /// <param name="session"></param>
        public void HTTP_200_Microsoft365_Any_Other_EWS(Session session)
        {
            // Any other (Microsoft365 / EXO) EWS call.

            this.session = session;

            // If this isn't an EWS call, return.
            if (!this.session.uriContains("ews/exchange.asmx"))
            {
                return;
            }

            if (!this.session.HostnameIs("outlook.office365.com"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 EXO / M365 EWS call.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_Microsoft365_Any_Other_EWS",
                UIBackColour = "Green",
                UITextColour = "Black",

                SessionType = "Exchange Online / Microsoft365 Web Services",
                ResponseCodeDescription = "200 OK Microsoft365 Other EWS",
                ResponseAlert = "Exchange Online / Microsoft365 Web Services (EWS) call.",
                ResponseComments = "Exchange Online / Microsoft365 Web Services (EWS) call.",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 5,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            
        }

        /// <summary>
        /// Exchange OnPremise Any Other Exchange Web Services.
        /// </summary>
        /// <param name="session"></param>
        public void HTTP_200_OnPremise_Any_Other_EWS(Session session)
        {
            // Any other EWS call.
            // Note: There are some organizations who have vanity domains for Office 365. They are the outliers for this scenario.

            this.session = session;

            // If this isn't an EWS call, return.
            if (!this.session.uriContains("ews/exchange.asmx"))
            {
                return;
            }

            if (this.session.HostnameIs("outlook.office365.com"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " + this.session.id + " HTTP 200 OnPremise EWS call.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_OnPremise_Exchange_EWS",
                UIBackColour = "Green",
                UITextColour = "Black",

                SessionType = "Exchange OnPremise Web Services",
                ResponseCodeDescription = "200 OK Exchange Web Services / EWS",
                ResponseAlert = "Exchange OnPremise Web Services (EWS) call.",
                ResponseComments = "Exchange OnPremise Web Services (EWS) call.",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 5,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        /// <summary>
        /// Function to look for lurking errors, failures, and exceptions in HTTP 200s.
        /// Exclude any session which contains a content-type of javascript.
        /// </summary>
        /// <param name="session"></param>
        public void HTTP_200_Lurking_Errors(Session session)
        {
            this.session = session;

            if (this.session.ResponseHeaders["Content-Type"].Contains("javascript"))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Javascript");

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s_Javascript",
                    UIBackColour = "Green",
                    UITextColour = "Black",

                    SessionType = "HTTP 200 OK with Javascript",
                    ResponseCodeDescription = "HTTP 200 OK with Javascript.",
                    ResponseAlert = "HTTP 200 OK with Javascript.",
                    ResponseComments = "<p>HTTP 200 OK response with javascript.</p>",

                    SessionAuthenticationConfidenceLevel = 5,
                    SessionTypeConfidenceLevel = 10,
                    SessionResponseServerConfidenceLevel = 5,
                    SessionSeverity = 0
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);

                return;
            }

            // REVIEW THIS -- Call SessionWordSearch utility function and clean this up.

            string searchTerm = "empty";

            /////////////////////////////
            //
            // All other specific scenarios, fall back to looking for errors lurking in HTTP 200 OK responses.

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

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 FAILURE LURKING!?");

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s_Lurking_Errors",
                    UIBackColour = "Black",
                    UITextColour = "Red",

                    SessionType = "!FAILURE LURKING!",
                    ResponseCodeDescription = "200 OK, but possibly bad.",
                    ResponseAlert = "<b><span style='color:red'>'error', 'failed' or 'exception' found in response body</span></b>",
                    ResponseComments = "<p>Session response body was scanned and errors or failures were found in response body. "
                    + "Check the Raw tab, click 'View in Notepad' button bottom right, and search for error in the response to review.</p>"
                    + "<p>After splitting all words in the response body the following were found:</p>"
                    + "<p>" + wordCountErrorText + "</p>"
                    + "<p>" + wordCountFailedText + "</p>"
                    + "<p>" + wordCountExceptionText + "</p>"
                    + "<p>Check the content body of the response for any failures you recognise. You may find <b>false positives, "
                    + "if lots of Javascript or other web code</b> is being loaded.</p>",

                    SessionAuthenticationConfidenceLevel = 5,
                    SessionTypeConfidenceLevel = 10,
                    SessionResponseServerConfidenceLevel = 5,
                    SessionSeverity = 70
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
            else
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 OK");

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s_No_Lurking_Errors",
                    UIBackColour = "Green",
                    UITextColour = "Black",

                    SessionType = "200 OK",
                    ResponseCodeDescription = "200 OK",
                    ResponseAlert = "HTTP 200 OK, with no errors, failed, or exceptions found.",
                    ResponseComments = "HTTP 200 OK, with no errors, failed, or exceptions found.",

                    SessionAuthenticationConfidenceLevel = 5,
                    SessionTypeConfidenceLevel = 10,
                    SessionResponseServerConfidenceLevel = 5,
                    SessionSeverity = 0
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
        }
    }
}