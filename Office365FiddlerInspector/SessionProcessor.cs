using System;
using Fiddler;
using System.Linq;
using Office365FiddlerInspector.Services;
using Office365FiddlerInspector.Ruleset;

namespace Office365FiddlerInspector
{
    public class SessionProcessor : ActivationService
    {
        // Initialize code section, changes infrequently.

        // Colour codes for sessions. Softer tones, easier on the eye than standard red, orange and green.
        string HTMLColourBlue = "#81BEF7";
        string HTMLColourGreen = "#81F7BA";
        string HTMLColourRed = "#F06141";
        string HTMLColourGrey = "#BDBDBD";
        string HTMLColourOrange = "#F59758";

        private static SessionProcessor _instance;

        public static SessionProcessor Instance => _instance ?? (_instance = new SessionProcessor());

        private bool IsInitialized { get; set; }
        
        internal Session session { get; set; }

        //////////////////////////
        // Session Classification Confidence Levels.
        //

        // Session Authentication Confidence Level.
        private int iSACL;

        // Session Type Confidence Level.
        private int iSTCL;

        // Session Response Server Confidence Level.
        private int iSRSCL;

        // How are session classifications used?
        // Low - 0 : Session classification has low confidence, any and all subsequent functions should be run to
        // further attempt to classify the session.
        // Mid - 5 : Session classification has some confidence, but overriding functions should be run just in case.
        // High - 10 : Session classification has high level of confidence and any overriding functions should not be run.

        public SessionProcessor() {}

        public void Initialize()
        {
            // Stop HandleLoadSaz and further processing if the extension is not enabled.
            if (!Preferences.ExtensionEnabled)

            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: Extension not enabled, exiting.");
                return;
            }                

            FiddlerApplication.OnLoadSAZ += HandleLoadSaz;

            FiddlerApplication.OnSaveSAZ += HandleSaveSaz;

            if (!IsInitialized)
            {
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Custom", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Comments", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Content-Type", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Caching", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Body", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("URL", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Host", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Protocol", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Process", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Elapsed Time", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Session Type", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Authentication", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Host IP", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Response Server", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Result", 1, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("#", 0, -1);

                IsInitialized = true;
            }
        }

        // Function to handle saving a SAZ file.
        private void HandleSaveSaz(object sender, FiddlerApplication.WriteSAZEventArgs e)
        {
            // Remove the session flags the extension adds to save space in the file and
            // mitigate errors thrown when loading a SAZ file which was saved with the extension enabled.
            // https://github.com/jprknight/Office365FiddlerExtension/issues/45

            FiddlerApplication.UI.lvSessions.BeginUpdate();

            foreach (var session in e.arrSessions)
            {
                session.oFlags.Remove("UI-BACKCOLOR");
                session.oFlags.Remove("UI-COLOR");
                session.oFlags.Remove("X-SESSIONTYPE");
                session.oFlags.Remove("X-ATTRIBUTENAMEIMMUTABLEID");
                session.oFlags.Remove("X-ATTRIBUTENAMEUPN");
                session.oFlags.Remove("X-AUTHENTICATION");
                session.oFlags.Remove("X-AUTHENTICATIONDESC");
                session.oFlags.Remove("X-ELAPSEDTIME");
                session.oFlags.Remove("X-RESPONSESERVER");
                session.oFlags.Remove("X-ISSUER");
                session.oFlags.Remove("X-NAMEIDENTIFIERFORMAT");
                session.oFlags.Remove("X-OFFICE365AUTHTYPE");
                session.oFlags.Remove("X-PROCESSNAME");
                session.oFlags.Remove("X-RESPONSEALERT");
                session.oFlags.Remove("X-RESPONSECOMMENTS");
                session.oFlags.Remove("X-RESPONSECODEDESCRIPTION");
                session.oFlags.Remove("X-DATAAGE");
                session.oFlags.Remove("X-DATACOLLECTED");
                session.oFlags.Remove("X-SERVERTHINKTIME");
                session.oFlags.Remove("X-TRANSITTIME");
                session.oFlags.Remove("X-CALCULATEDSESSIONAGE");
                session.oFlags.Remove("X-PROCESSINFO");
                session.oFlags.Remove("X-SACL");
                session.oFlags.Remove("X-STCL");
                session.oFlags.Remove("X-SRSCL");
            }

            FiddlerApplication.UI.lvSessions.EndUpdate();
        }

        // Function to handle loading a SAZ file.
        private void HandleLoadSaz(object sender, FiddlerApplication.ReadSAZEventArgs e)
        {
            FiddlerApplication.UI.lvSessions.BeginUpdate();

            // Looking at this I can't see a good reason why it would be updated here.
            // Whether the extension is loaded or not and what the enable/disble option looks like would be determined elsewhere.
            //MenuUI.Instance.MiEnabled.Checked = Preferences.ExtensionEnabled;

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: LoadSaz with Extension Enabled {Preferences.ExtensionEnabled}.");

            foreach (var session in e.arrSessions)
            {
                
                if (Preferences.ExtensionEnabled)
                {
                    // Call the main fuction which runs through all session logic checks.
                    Instance.OnPeekAtResponseHeaders(session);

                    session.RefreshUI();
                }
            }
            FiddlerApplication.UI.lvSessions.EndUpdate();
        }

        // This is the main function where everything is called from.
        public void OnPeekAtResponseHeaders(Session session)
        {
            /////////////////////////////
            ///
            // *** START HERE***
            //
            // This function is where all the things happen, where everything else is called from,
            // and the order of operations is determined.
            ///
            /////////////////////////////

            this.session = session;

            if (!this.session.isFlagSet(SessionFlags.LoadedFromSAZ))
            {
                // Live sessions, return.
                FiddlerApplication.Log.LogString("Office365FiddlerExtention: " + this.session.id + " live session, diliberate return.");
                return;
            }

            // Decode session requests/responses.
            this.session.utilDecodeRequest(true);
            this.session.utilDecodeResponse(true);

            ///////////////////////
            ///
            // Always run these functions on every session.

            // Broad logic checks on sessions regardless of response code.
            BroadLogicChecks broadLogiccChecks = new BroadLogicChecks();
            broadLogiccChecks.FiddlerUpdateSessions(session);
            broadLogiccChecks.ConnectTunnelSessions(session);
            broadLogiccChecks.ApacheAutodiscover(session);

            // Calculate Session Age for inspector with HTML mark-up.
            CalculateSessionAge calculateSessionAge = new CalculateSessionAge();
            calculateSessionAge.SessionAge(session);

            // Set Server Think Time and Transit Time for inspector with HTML mark-up.
            ServerThinkTimeTransitTime setServerThinkTimeTransitTime = new ServerThinkTimeTransitTime();
            setServerThinkTimeTransitTime.SetServerThinkTimeTransitTime(session);

            // Set Elapsed Time column data.
            SessionElapsedTime sessionElapsedTime = new SessionElapsedTime();
            sessionElapsedTime.SetElapsedTime(session);
            sessionElapsedTime.SetInspectorElapsedTime(session);
            
            ///////////////////////
            ///
            // From here on out only run functions where there isn't a high level of confidence
            // on session classification.
            GetSACL(session);
            GetSTCL(session);
            GetSRSCL(session);
            if (iSACL < 10 || iSTCL < 10 || iSTCL < 10)
            {
                // Response code based logic. This is the big one.
                Instance.ResponseCodeLogic(session);
            }

            // If the session does not already have a high auth classification confidence, run.
            GetSACL(session);
            if (iSACL < 10)
            {
                // Set Authentication column data and SAML Response Parser for inspector.
                SetAuthentication setAuthentication = new SetAuthentication();
                setAuthentication.SetAuthenticationData(session);
            }

            // If the session does not already have a high session type classification confidence, run.
            GetSTCL(session);
            if (iSTCL < 10)
            {
                // If SSCL is low run Session Type override function.
                SetSessionType setSessionType = new SetSessionType();
                setSessionType.SetSessionTypeData(session);
            }

            // If the session does not already have a high response server classification confidence, run.
            GetSRSCL(session);
            if (iSRSCL < 10)
            {
                // Set Response Server column data.
                Instance.SetResponseServer(session);
            }

            // If session has not been classified run Long Running Session override function.
            // In relatively few cases has roundtrip time been highlighted as an issue by Fiddler alone.
            // So this is the last function to run after all other logic has been exhausted.
            // Typically network traces are used to validate the underlying network connectivity.
            GetSACL(session);
            GetSTCL(session);
            GetSRSCL(session);
            if (iSACL < 10 || iSTCL < 10 || iSTCL < 10)
            {
                Instance.SetLongRunningSessions(session);
            }
        }

        

        // Function containing switch statement for response code logic.
        // https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
        public void ResponseCodeLogic (Session session)
        {
            FiddlerApplication.Log.LogString("Office365FiddlerExtention: " + this.session.id + " Running ResponseCodeLogic.");

            switch (this.session.responseCode)
            {
                // Note, the breakdown of response codes, for example 200.5, has no formal meaning.
                // It's just an easy way to organise the content.
                case 0:
                    HTTP_0 http_0 = new HTTP_0();
                    http_0.HTTP_0_NoSessionResponse(session);
                    
                    break;
                case 200:

                    HTTP_200 http_200 = new HTTP_200();

                    http_200.HTTP_200_ClientAccessRule(session);
                    
                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_200.HTTP_200_Outlook_Mapi(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_200.HTTP_200_Outlook_RPC(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_200.HTTP_200_Outlook_NSPI(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_200.HTTP_200_OnPremise_AutoDiscover_Redirect(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_200.HTTP_200_OnPremise_AutoDiscover_Redirect_AddressNotFound(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_200.HTTP_200_EXO_M365_AutoDiscover(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_200.HTTP_200_Unified_Groups_Settings(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_200.HTTP_200_3S_Suggestions(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_200.HTTP_200_REST_People_Request(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_200.HTTP_200_Any_Other_Exchange_EWS(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_200.HTTP_200_Lurking_Errors(session);

                    break;
                case 201:
                    HTTP_201 http_201 = new HTTP_201();
                    http_201.HTTP_201_Created(session);

                    break;
                case 202:
                    HTTP_202 http_202 = new HTTP_202();
                    http_202.HTTP_202_Accepted(session);

                    break;
                case 203:
                    HTTP_203 http_203 = new HTTP_203();
                    http_203.HTTP_203_NonAuthoritive_Answer(session);

                    break;
                case 204:
                    HTTP_204 http_204 = new HTTP_204();
                    http_204.HTTP_204_No_Content(session);

                    break;
                case 205:
                    HTTP_205 http_205 = new HTTP_205();
                    http_205.HTTP_205_Reset_Content(session);

                    break;
                case 206:
                    HTTP_206 http_206 = new HTTP_206();
                    http_206.HTTP_206_Partial_Content(session);

                    break;
                case 207:
                    HTTP_207 http_207 = new HTTP_207();
                    http_207.HTTP_207_Multi_Status(session);

                    break;
                case 208:
                    HTTP_208 http_208 = new HTTP_208();
                    http_208.HTTP_208_Already_Reported(session);

                    break;
                case 226:
                    HTTP_226 http_226 = new HTTP_226();
                    http_226.HTTP_226_IM_Used(session);

                    break;
                case 300:
                    HTTP_300 http_300 = new HTTP_300();
                    http_300.HTTP_300_Multiple_Choices(session);

                    break;
                case 301:
                    HTTP_301 http_301 = new HTTP_301();
                    http_301.HTTP_301_Permanently_Moved(session);

                    break;
                case 302:
                    HTTP_302 http_302 = new HTTP_302();
                    http_302.HTTP_302_Redirect(session);

                    break;
                case 303:
                    HTTP_303 http_303 = new HTTP_303();
                    http_303.HTTP_303_See_Other(session);

                    break;
                case 304:
                    HTTP_304 http_304 = new HTTP_304();
                    http_304.HTTP_304_Not_Modified(session);

                    break;
                case 305:
                    HTTP_305 http_305 = new HTTP_305();
                    http_305.HTTP_305_Use_Proxy(session);

                    break;
                case 306:
                    HTTP_306 http_306 = new HTTP_306();
                    http_306.HTTP_306_Switch_Proxy(session);

                    break;
                case 307:
                    HTTP_307 http_307 = new HTTP_307();
                    http_307.HTTP_307_Temporary_Redirect(session);

                    break;
                case 308:
                    HTTP_308 http_308 = new HTTP_308();
                    http_308.HTTP_308_Permenant_Redirect(session);

                    break;
                case 400:
                    HTTP_400 http_400 = new HTTP_400();
                    http_400.HTTP_400_Bad_Request(session);

                    break;
                case 401:
                    HTTP_401 http_401 = new HTTP_401();
                    http_401.HTTP_401_Exchange_Online_AutoDiscover(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_401.HTTP_401_Exchange_OnPremise_AutoDiscover(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_401.HTTP_401_EWS(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_401.HTTP_401_Everything_Else(session);

                    break;
                case 402:
                    HTTP_402 http_402 = new HTTP_402();
                    http_402.HTTP_402_Payment_Required(session);

                    break;
                case 403:
                    HTTP_403 http_403 = new HTTP_403();
                    http_403.HTTP_403_Forbidden(session);

                    break;
                case 404:
                    HTTP_404 http_404 = new HTTP_404();
                    http_404.HTTP_404_Not_Found(session);

                    break;
                case 405:
                    HTTP_405 http_405 = new HTTP_405();
                    http_405.HTTP_405_Method_Not_Allowed(session);

                    break;
                case 406:
                    HTTP_406 http_406 = new HTTP_406();
                    http_406.HTTP_406_Not_Acceptable(session);

                    break;
                case 407:
                    HTTP_407 http_407 = new HTTP_407();
                    http_407.HTTP_407_Proxy_Auth_Required(session);

                    break;
                case 408:
                    HTTP_408 http_408 = new HTTP_408();
                    http_408.HTTP_408_Request_Timeout(session);

                    break;
                case 409:
                    HTTP_409 http_409 = new HTTP_409();
                    http_409.HTTP_409_Conflict(session);

                    break;
                case 410:
                    HTTP_410 http_410 = new HTTP_410();
                    http_410.HTTP_410_Gone(session);

                    break;
                case 411:
                    HTTP_411 http_411 = new HTTP_411();
                    http_411.HTTP_411_Length_Required(session);

                    break;
                case 412:
                    HTTP_412 http_412 = new HTTP_412();
                    http_412.HTTP_412_Precondition_Failed(session);

                    break;
                case 413:
                    HTTP_413 http_413 = new HTTP_413();
                    http_413.HTTP_413_Payload_Too_Large(session);

                    break;
                case 414:
                    HTTP_414 http_414 = new HTTP_414();
                    http_414.HTTP_414_URI_Too_Long(session);

                    break;
                case 415:
                    HTTP_415 http_415 = new HTTP_415();
                    http_415.HTTP_415_UnSupported_Media_Type(session);

                    break;
                case 416:
                    HTTP_416 http_416 = new HTTP_416();
                    http_416.HTTP_416_Range_Not_Satisfiable(session);

                    break;
                case 417:
                    HTTP_417 http_417 = new HTTP_417();
                    http_417.HTTP_417_Expectation_Failed(session);

                    break;
                case 418:
                    HTTP_418 http_418 = new HTTP_418();
                    http_418.HTTP_418_Im_A_Teapot(session);

                    break;
                case 421:
                    HTTP_421 http_421 = new HTTP_421();
                    http_421.HTTP_421_Misdirected_Request(session);

                    break;
                case 422:
                    HTTP_422 http_422 = new HTTP_422();
                    http_422.HTTP_422_Unprocessable_Entry(session);

                    break;
                case 423:
                    HTTP_423 http_423 = new HTTP_423();
                    http_423.HTTP_423_Locked(session);

                    break;
                case 424:
                    HTTP_424 http_424 = new HTTP_424();
                    http_424.HTTP_424_Failed_Dependency(session);

                    break;
                case 425:
                    HTTP_425 http_425 = new HTTP_425();
                    http_425.HTTP_425_Too_Early(session);

                    break;
                case 426:
                    HTTP_426 http_426 = new HTTP_426();
                    http_426.HTTP_426_Upgrade_Required(session);

                    break;
                case 428:
                    HTTP_428 http_428 = new HTTP_428();
                    http_428.HTTP_428_Precondition_Required(session);

                    break;
                case 429:
                    HTTP_429 http_429 = new HTTP_429();
                    http_429.HTTP_429_Too_Many_Requests(session);

                    break;
                case 431:
                    HTTP_431 http_431 = new HTTP_431();
                    http_431.HTTP_431_Request_Header_Fields_Too_Large(session);

                    break;
                case 451:
                    HTTP_451 http_451 = new HTTP_451();
                    http_451.HTTP_451_Unavailable_For_Legal_Reasons_or_IIS_Redirect(session);

                    break;
                case 456:
                    HTTP_456 http_456 = new HTTP_456();
                    http_456.HTTP_456_Multi_Factor_Required(session);

                    break;
                case 500:
                    HTTP_500 http_500 = new HTTP_500();
                    http_500.HTTP_500_Internal_Server_Error_Repeating_Redirects(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_500.HTTP_500_Internal_Server_Error_Impersonate_User_Denied(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_500.HTTP_500_Internal_Server_Error_OWA_Something_Went_Wrong(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_500.HTTP_500_Internal_Server_Error_All_Others(session);

                    break;
                case 501:
                    HTTP_501 http_501 = new HTTP_501();
                    http_501.HTTP_501_Not_Implemented(session);

                    break;
                case 502:
                    HTTP_502 http_502 = new HTTP_502();
                    http_502.HTTP_502_Bad_Gateway_Telemetry_False_Positive(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_502.HTTP_502_Bad_Gateway_EXO_DNS_Lookup_False_Positive(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_502.HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_502.HTTP_502_Bad_Gateway_Vanity_Domain_M365_AutoDiscover_False_Positive(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_502.HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_502.HTTP_502_Bad_Gateway_Anything_Else(session);

                    break;
                case 503:
                    HTTP_503 http_503 = new HTTP_503();
                    http_503.HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable(session);
                    
                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_503.HTTP_503_Service_Unavailable_Everything_Else(session);
                    
                    break;
                case 504:
                    HTTP_504 http_504 = new HTTP_504();
                    http_504.HTTP_504_Gateway_Timeout_Internet_Access_Blocked(session);

                    if (this.session["X-SACL"] == "10" || this.session["X-STCL"] == "10" || this.session["X-SRSCL"] == "10")
                    {
                        break;
                    }

                    http_504.HTTP_504_Gateway_Timeout_Anything_Else(session);
                    
                    break;
                case 505:
                    this.session["X-ResponseAlert"] = "HTTP 505 HTTP Version Not Supported.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 505 HTTP Version Not Supported.");

                    this.session["X-ResponseCodeDescription"] = "505 HTTP Version Not Supported";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 506:
                    this.session["X-ResponseAlert"] = "HTTP 506 Variant Also Negotiates (RFC 2295).";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 506 Variant Also Negotiates (RFC 2295).");

                    this.session["X-ResponseCodeDescription"] = "506 Variant Also Negotiates (RFC 2295)";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 507:
                    this.session["X-ResponseAlert"] = "HTTP 507 Insufficient Storage (WebDAV; RFC 4918).";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 507 Insufficient Storage (WebDAV; RFC 4918).");

                    this.session["X-ResponseCodeDescription"] = "507 Insufficient Storage (WebDAV; RFC 4918)";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 508:
                    this.session["X-ResponseAlert"] = "HTTP 508 Loop Detected (WebDAV; RFC 5842).";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 508 Loop Detected (WebDAV; RFC 5842).");

                    this.session["X-ResponseCodeDescription"] = "508 Loop Detected (WebDAV; RFC 5842)";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 510:
                    this.session["X-ResponseAlert"] = "HTTP 510 Not Extended (RFC 2774).";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 510 Not Extended (RFC 2774).");

                    this.session["X-ResponseCodeDescription"] = "510 Not Extended (RFC 2774)";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 511:
                    this.session["X-ResponseAlert"] = "HTTP 511 Network Authentication Required (RFC 6585).";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 511 Network Authentication Required (RFC 6585).");

                    this.session["X-ResponseCodeDescription"] = "511 Network Authentication Required (RFC 6585)";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 103:
                    this.session["X-ResponseAlert"] = "HTTP 103 Checkpoint.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 103 Checkpoint.");

                    this.session["X-ResponseCodeDescription"] = "103 Checkpoint";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 218:
                    this.session["X-ResponseAlert"] = "HTTP 218 This is fine (Apache Web Server).";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 218 This is fine (Apache Web Server).");

                    this.session["X-ResponseCodeDescription"] = "218 This is fine (Apache Web Server)";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 419:
                    this.session["X-ResponseAlert"] = "HTTP 419 Page Expired (Laravel Framework).";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 419 Page Expired (Laravel Framework).");

                    this.session["X-ResponseCodeDescription"] = "419 Page Expired (Laravel Framework)";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 420:
                    this.session["X-ResponseAlert"] = "HTTP 420 Method Failure (Spring Framework) or Enhance Your Calm (Twitter).";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 420 Method Failure (Spring Framework) or Enhance Your Calm (Twitter).");

                    this.session["X-ResponseCodeDescription"] = "420 Method Failure (Spring Framework) or Enhance Your Calm (Twitter)";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 430:
                    this.session["X-ResponseAlert"] = "HTTP 430 Request Header Fields Too Large (Shopify).";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 430 Request Header Fields Too Large (Shopify).");

                    this.session["X-ResponseCodeDescription"] = "430 Request Header Fields Too Large (Shopify)";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 450:
                    this.session["X-ResponseAlert"] = "HTTP 450 Blocked by Windows Parental Controls (Microsoft).";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 450 Blocked by Windows Parental Controls (Microsoft).");

                    this.session["X-ResponseCodeDescription"] = "450 Blocked by Windows Parental Controls (Microsoft)";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 498:
                    this.session["X-ResponseAlert"] = "HTTP 498 Invalid Token (Esri).";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 498 Invalid Token (Esri).");

                    this.session["X-ResponseCodeDescription"] = "498 Invalid Token (Esri)";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 499:
                    this.session["X-ResponseAlert"] = "HTTP 499 Token Required (Esri) or nginx Client Closed Request.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 499 Token Required (Esri) or nginx Client Closed Request.");

                    this.session["X-ResponseCodeDescription"] = "499 Token Required (Esri) or nginx Client Closed Request";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 509:
                    this.session["X-ResponseAlert"] = "HTTP 509 Bandwidth Limit Exceeded (Apache Web Server/cPanel).";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 509 Bandwidth Limit Exceeded (Apache Web Server/cPanel).");

                    this.session["X-ResponseCodeDescription"] = "509 Bandwidth Limit Exceeded (Apache Web Server/cPanel)";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 529:
                    this.session["X-ResponseAlert"] = "HTTP 529 Site is overloaded.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 529 Site is overloaded.");

                    this.session["X-ResponseCodeDescription"] = "529 Site is overloaded";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 530:
                    this.session["X-ResponseAlert"] = "HTTP 530 Site is frozen or Cloudflare Error returned with 1xxx error.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 530 Site is frozen or Cloudflare Error returned with 1xxx error.");

                    this.session["X-ResponseCodeDescription"] = "530 Site is frozen or Cloudflare Error returned with 1xxx error.";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 598:
                    this.session["X-ResponseAlert"] = "HTTP 598 (Informal convention) Network read timeout error.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 598 (Informal convention) Network read timeout error.");

                    this.session["X-ResponseCodeDescription"] = "598 (Informal convention) Network read timeout error";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 440:
                    this.session["X-ResponseAlert"] = "HTTP 440 IIS Login Time-out.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 440 IIS Login Time-out");

                    this.session["X-ResponseCodeDescription"] = "440 IIS Login Time-out";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 449:
                    this.session["X-ResponseAlert"] = "HTTP 449 IIS Retry With.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    this.session["X-ResponseCodeDescription"] = "449 IIS Retry With";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 449 IIS Retry With");

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 494:
                    this.session["X-ResponseAlert"] = "HTTP 494 nginx Request header too large.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 494 nginx Request header too large");

                    this.session["X-ResponseCodeDescription"] = "494 nginx Request header too large";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 495:
                    this.session["X-ResponseAlert"] = "HTTP 495 nginx SSL Certificate Error.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 495 nginx SSL Certificate Error");

                    this.session["X-ResponseCodeDescription"] = "495 nginx SSL Certificate Error";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 496:
                    this.session["X-ResponseAlert"] = "HTTP 496 nginx SSL Certificate Required.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 496 nginx SSL Certificate Required");

                    this.session["X-ResponseCodeDescription"] = "496 nginx SSL Certificate Required";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 497:
                    this.session["X-ResponseAlert"] = "HTTP 497 nginx HTTP Request Sent to HTTPS Port.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 497 nginx HTTP Request Sent to HTTPS Port");

                    this.session["X-ResponseCodeDescription"] = "497 nginx HTTP Request Sent to HTTPS Port";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 520:
                    this.session["X-ResponseAlert"] = "HTTP 520 Cloudflare Web Server Returned an Unknown Error.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 520 Cloudflare Web Server Returned an Unknown Error");

                    this.session["X-ResponseCodeDescription"] = "520 Cloudflare Web Server Returned an Unknown Error";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 521:
                    this.session["X-ResponseAlert"] = "HTTP 521 Cloudflare Web Server Is Down.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 521 Cloudflare Web Server Is Down");

                    this.session["X-ResponseCodeDescription"] = "521 Cloudflare Web Server Is Down";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 522:
                    this.session["X-ResponseAlert"] = "HTTP 522 Cloudflare Connection Timed Out.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 522 Cloudflare Connection Timed Out");

                    this.session["X-ResponseCodeDescription"] = "522 Cloudflare Connection Timed Out";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 523:
                    this.session["X-ResponseAlert"] = "HTTP 523 Cloudflare Origin Is Unreachable.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 523 Cloudflare Origin Is Unreachable");

                    this.session["X-ResponseCodeDescription"] = "523 Cloudflare Origin Is Unreachable";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 524:
                    this.session["X-ResponseAlert"] = "HTTP 524 Cloudflare A Timeout Occurred.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 524 Cloudflare A Timeout Occurred");

                    this.session["X-ResponseCodeDescription"] = "524 Cloudflare A Timeout Occurred";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 525:
                    this.session["X-ResponseAlert"] = "HTTP 525 Cloudflare SSL Handshake Failed.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 525 Cloudflare SSL Handshake Failed");

                    this.session["X-ResponseCodeDescription"] = "525 Cloudflare SSL Handshake Failed";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 526:
                    this.session["X-ResponseAlert"] = "HTTP 526 Cloudflare Invalid SSL Certificate.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 526 Cloudflare Invalid SSL Certificate");

                    this.session["X-ResponseCodeDescription"] = "526 Cloudflare Invalid SSL Certificate";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 527:
                    this.session["X-ResponseAlert"] = "HTTP 527 Cloudflare Railgun Error.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 527 Cloudflare Railgun Error");

                    this.session["X-ResponseCodeDescription"] = "527 Cloudflare Railgun Error";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 460:
                    this.session["X-ResponseAlert"] = "HTTP 460 AWS Load balancer Timeout.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 460 AWS Load balancer Timeout");

                    this.session["X-ResponseCodeDescription"] = "460 AWS Load balancer Timeout";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 463:
                    this.session["X-ResponseAlert"] = "HTTP 463 AWS X-Forwarded-For Header > 30 IP addresses.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 463 AWS X-Forwarded-For Header > 30 IP addresses");

                    this.session["X-ResponseCodeDescription"] = "463 AWS X-Forwarded-For Header > 30 IP addresses";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                case 561:
                    this.session["X-ResponseAlert"] = "HTTP 561 AWS Unauthorized.";
                    this.session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 561 AWS Unauthorized");

                    this.session["X-ResponseCodeDescription"] = "561 AWS Unauthorized";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                /////////////////////////////
                // Fallen into default, so undefined in the extension.
                // Mark the session as such.
                default:
                    // Commented out setting colours on sessions not recognised.
                    // Find in Fiddler will highlight sessions as yellow, so this would make reviewing find results difficult.
                    //this.session["ui-backcolor"] = "Yellow";
                    //this.session["ui-color"] = "black";
                    this.session["X-SessionType"] = "Undefined";

                    this.session["X-ResponseAlert"] = "Undefined.";
                    this.session["X-ResponseComments"] = "No specific information on this session in the Office 365 Fiddler Extension.<br />"
                        + Preferences.GetStrNoKnownIssue();

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Session undefined in extension.");

                    this.session["X-ResponseCodeDescription"] = "Defaulted. HTTP Response Code undefined.";

                    // Nothing meaningful here, let further processing try to pick up something.
                    SetSACL(session, "0");
                    SetSTCL(session, "0");
                    SetSRSCL(session, "0");

                    break;
                    //
                    /////////////////////////////
            }
        }




        // Function where the Response Server column is populated.
        public void SetResponseServer(Session session)
        {
            // ResponseServer
            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Running SetResponseServer.");

            this.session = session;

            // Populate Response Server on session in order of preference from common to obsure.

            // If the response server header is not null or blank then populate it into the response server value.
            if ((this.session.oResponse["Server"] != null) && (this.session.oResponse["Server"] != ""))
            {
                this.session["X-ResponseServer"] = this.session.oResponse["Server"];
                SetSRSCL(session, "10");
            }
            // Else if the reponnse Host header is not null or blank then populate it into the response server value
            // Some traffic identifies a host rather than a response server.
            else if ((this.session.oResponse["Host"] != null && (this.session.oResponse["Host"] != "")))
            {
                this.session["X-ResponseServer"] = "Host: " + this.session.oResponse["Host"];
                SetSRSCL(session, "10");
            }
            // Else if the response PoweredBy header is not null or blank then populate it into the response server value.
            // Some Office 365 servers respond as X-Powered-By ASP.NET.
            else if ((this.session.oResponse["X-Powered-By"] != null) && (this.session.oResponse["X-Powered-By"] != ""))
            {
                this.session["X-ResponseServer"] = "X-Powered-By: " + this.session.oResponse["X-Powered-By"];
                SetSRSCL(session, "10");
            }
            // Else if the response X-Served-By header is not null or blank then populate it into the response server value.
            else if ((this.session.oResponse["X-Served-By"] != null && (this.session.oResponse["X-Served-By"] != "")))
            {
                this.session["X-ResponseServer"] = "X-Served-By: " + this.session.oResponse["X-Served-By"];
                SetSRSCL(session, "10");
            }
            // Else if the response X-Served-By header is not null or blank then populate it into the response server value.
            else if ((this.session.oResponse["X-Server-Name"] != null && (this.session.oResponse["X-Server-Name"] != "")))
            {
                this.session["X-ResponseServer"] = "X-Served-Name: " + this.session.oResponse["X-Server-Name"];
                SetSRSCL(session, "10");
            }
            else if ((this.session.isTunnel))
            {
                this.session["X-ResponseServer"] = this.session["X-SessionType"];
                SetSRSCL(session, "10");
            }
        }

        // Function to highlight long running sessions.
        public void SetLongRunningSessions(Session session)
        {
            // LongRunningSessions
            // Code section for response code logic overrides (long running sessions).

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Running SetLongRunningSessions.");

            double ClientMilliseconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);

            double ServerMilliseconds = Math.Round((this.session.Timers.ServerBeginResponse - this.session.Timers.ServerGotRequest).TotalMilliseconds);

            // Warn on a 2.5 second roundtrip time.
            if (ClientMilliseconds > Preferences.GetWarningSessionTimeThreshold() && ClientMilliseconds < Preferences.GetSlowRunningSessionThreshold())
            {
                if (this.session["X-SessionType"] == null)
                {
                    this.session["ui-backcolor"] = HTMLColourOrange;
                    this.session["ui-color"] = "black";

                    this.session["X-SessionType"] = "Roundtrip Time Warning";

                    this.session["X-ResponseAlert"] = "<b><span style='color:orange'>Roundtrip Time Warning</span></b>";
                }

                this.session["X-ResponseComments"] += "This session took more than 2.5 seconds to complete. "
                    + "A small number of sessions completing roundtrip in this timeframe is not necessary sign of an issue.";
            }
            // If the overall session time runs longer than 5,000ms or 5 seconds.
            else if (ClientMilliseconds > Preferences.GetSlowRunningSessionThreshold())
            {
                if (this.session["X-SessionType"] == null)
                {
                    this.session["ui-backcolor"] = HTMLColourRed;
                    this.session["ui-color"] = "black";

                    this.session["X-SessionType"] = "Long Running Client Session";

                    this.session["X-ResponseAlert"] = "<b><span style='color:red'>Long Running Client Session</span></b>";
                }

                this.session["X-ResponseComments"] += "<p><b><span style='color:red'>Long running session found</span></b>. A small number of long running sessions in the < 10 "
                    + "seconds time frame have been seen on normal working scenarios. This does not necessary signify an issue.</p>"
                    + "<p>If, however, you are troubleshooting an application performance issue, consider the number of sessions which "
                    + "have this warning. Investigate any proxy device or load balancer in your network, "
                    + "or any other device sitting between the client computer and access to the application server the data resides on.</p>"
                    + "<p>Try the divide and conquer approach. What can you remove or bypass from the equation to see if the application then performs "
                    + "normally?</p>";

                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Long running client session.");
            }
            // If the Office 365 server think time runs longer than 5,000ms or 5 seconds.
            else if (ServerMilliseconds > Preferences.GetSlowRunningSessionThreshold())
            {
                if (this.session["X-SessionType"] == null)
                {
                    this.session["ui-backcolor"] = HTMLColourRed;
                    this.session["ui-color"] = "black";

                    this.session["X-SessionType"] = "Long Running Server Session";

                    this.session["X-ResponseAlert"] = "<b><span style='color:red'>Long Running Server Session</span></b>";
                }

                this.session["X-ResponseComments"] += "Long running Server session found. A small number of long running sessions in the < 10 "
                    + "seconds time frame have been seen on normal working scenarios. This does not necessary signify an issue."
                    + "<p>If, however, you are troubleshooting an application performance issue, consider the number of sessions which "
                    + "have this warning alongany proxy device in your network, "
                    + "or any other device sitting between the client computer and access to the internet."
                    + "Try the divide and conquer approach. What can you remove or bypass from the equation to see if the application then performs "
                    + "normally?</p>";

                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Long running Office 365 session.");
            }
        }

        // Functions which support population of data into fields, inspector etc.



        

        // Functions to set session confidence levels.
        // SessionConfidenceFunctions

        // Get Session Authentication Confidence Level.
        public void GetSACL(Session session)
        {
            // Avoid null object exceptions by setting this session flag to something
            // rather than nothing.
            if (this.session["X-SACL"] == null || this.session["X-SACL"] == "")
            {
                this.session["X-SACL"] = "00";
            }
            iSACL = int.Parse(this.session["X-SACL"]);
        }

        // Set Session Authentication Confidence Level.
        public void SetSACL(Session session, string SACL)
        {
            this.session["X-SACL"] = SACL;
        }

        // Get Session Type Confidence Level.
        public void GetSTCL(Session session)
        {
            // Avoid null object exceptions by setting this session flag to something
            // rather than nothing.
            if (this.session["X-STCL"] == null || this.session["X-STCL"] == "")
            {
                this.session["X-STCL"] = "00";
            }
            iSTCL = int.Parse(this.session["X-STCL"]);
        }

        // Set Session Type Confidence Level.
        public void SetSTCL(Session session, string STCL)
        {
            this.session["X-STCL"] = STCL;
        }

        // Get Session Response Server Confidence Level.
        public void GetSRSCL( Session session)
        {
            // Avoid null object exceptions by setting this session flag to something
            // rather than nothing.
            if (this.session["X-SRSCL"] == null || this.session["X-SRSCL"] == "")
            {
                this.session["X-SRSCL"] = "00";
            }
            iSRSCL = int.Parse(this.session["X-SRSCL"]);
        }

        // Set Session Response Server Confidence Level.
        public void SetSRSCL(Session session, string SRSCL)
        {
            this.session["X-SRSCL"] = SRSCL;
        }
    }
}