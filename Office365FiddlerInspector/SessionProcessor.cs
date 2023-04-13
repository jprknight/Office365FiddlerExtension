using Fiddler;
using Office365FiddlerInspector.Ruleset;
using Office365FiddlerInspector.Services;
using System;

namespace Office365FiddlerInspector
{
    public class SessionProcessor : ActivationService
    {
        private static SessionProcessor _instance;

        public static SessionProcessor Instance => _instance ?? (_instance = new SessionProcessor());

        private bool IsInitialized { get; set; }

        public SessionProcessor() { }

        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

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
            this.session = session;

            /////////////////////////////
            ///
            // *** START HERE***
            //
            // This function is where all the things happen, where everything else is called from,
            // and the order of operations is determined.
            ///
            /////////////////////////////

            // This wasn't a popular decision. Enabling live session processing again, even though it's known TLS and other types of
            // detections may not work 100% when doing live trace analysis.

            //if (!this.session.isFlagSet(SessionFlags.LoadedFromSAZ))
            //{
            // Live sessions, return.
            //    FiddlerApplication.Log.LogString("Office365FiddlerExtention: " + this.session.id + " live session, diliberate return.");
            //    return;
            //}

            // Decode session requests/responses.
            this.session.utilDecodeRequest(true);
            this.session.utilDecodeResponse(true);

            ///////////////////////
            ///
            // Always run these functions on every session.

            // Broad logic checks on sessions regardless of response code.
            BroadLogicChecks broadLogicChecks = new BroadLogicChecks();
            broadLogicChecks.FiddlerUpdateSessions(this.session);
            broadLogicChecks.ConnectTunnelSessions(this.session);
            broadLogicChecks.ApacheAutodiscover(this.session);

            // Calculate Session Age for inspector with HTML mark-up.
            CalculateSessionAge calculateSessionAge = new CalculateSessionAge();
            calculateSessionAge.SessionAge(this.session);

            // Set Server Think Time and Transit Time for inspector with HTML mark-up.
            ServerThinkTimeTransitTime setServerThinkTimeTransitTime = new ServerThinkTimeTransitTime();
            setServerThinkTimeTransitTime.SetServerThinkTimeTransitTime(this.session);

            // Set Elapsed Time column data.
            SessionElapsedTime sessionElapsedTime = new SessionElapsedTime();
            sessionElapsedTime.SetElapsedTime(this.session);
            sessionElapsedTime.SetInspectorElapsedTime(this.session);

            ///////////////////////
            ///
            // From here on out only run functions where there isn't a high level of confidence
            // on session classification.
            if (getSetSessionFlags.GetSessionAuthenticationConfidenceLevel(this.session) < 10 ||
                getSetSessionFlags.GetSessionTypeConfidenceLevel(this.session) < 10 ||
                getSetSessionFlags.GetSessionResponseServerConfidenceLevel(this.session) < 10)
            {
                // Response code based logic. This is the big one.
                Instance.ResponseCodeLogic(this.session);
            }

            // If the session does not already have a high auth classification confidence, run.
            if (getSetSessionFlags.GetSessionAuthenticationConfidenceLevel(this.session) < 10)
            {
                // Set Authentication column data and SAML Response Parser for inspector.
                SetAuthentication setAuthentication = new SetAuthentication();
                setAuthentication.SetAuthenticationData(this.session);
            }

            // If the session does not already have a high session type classification confidence, run.
            if (getSetSessionFlags.GetSessionTypeConfidenceLevel(this.session) < 10)
            {
                // If SSCL is low run Session Type override function.
                SetSessionType setSessionType = new SetSessionType();
                setSessionType.SetSessionTypeData(this.session);
            }

            // If the session does not already have a high response server classification confidence, run.
            if (getSetSessionFlags.GetSessionResponseServerConfidenceLevel(this.session) < 10)
            {
                // Set Response Server column data.
                SetResponseServer setResponseServer = new SetResponseServer();
                setResponseServer.SetResponseServerData(this.session);
                //Instance.SetResponseServer(this.session);
            }

            // If session has not been classified run Long Running Session override function.
            // In relatively few cases has roundtrip time been highlighted as an issue by Fiddler alone.
            // So this is the last function to run after all other logic has been exhausted.
            // Typically network traces are used to validate the underlying network connectivity.
            if (getSetSessionFlags.GetSessionAuthenticationConfidenceLevel(this.session) < 10 ||
                getSetSessionFlags.GetSessionResponseServerConfidenceLevel(this.session) < 10 ||
                getSetSessionFlags.GetSessionTypeConfidenceLevel(this.session) < 10)
            {
                SetLongRunningSessions setLongRunningSessions = new SetLongRunningSessions();
                setLongRunningSessions.SetLongRunningSessionsData(this.session);
                //Instance.SetLongRunningSessions(this.session);
            }
        }

        // Function containing switch statement for response code logic.
        // https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
        public void ResponseCodeLogic(Session session)
        {
            FiddlerApplication.Log.LogString("Office365FiddlerExtention: " + this.session.id + " Running ResponseCodeLogic.");

            switch (this.session.responseCode)
            {
                case 0:
                    HTTP_0 http_0 = new HTTP_0();
                    http_0.HTTP_0_NoSessionResponse(this.session);
                    //HTTP_0.Instance.HTTP_0_NoSessionResponse(this.session);
                    break;
                case 103:
                    HTTP_103 http_103 = new HTTP_103();
                    http_103.HTTP_100_Checkpoint(this.session);
                    break;
                case 200:

                    HTTP_200 http_200 = new HTTP_200();

                    http_200.HTTP_200_ClientAccessRule(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_200.HTTP_200_Outlook_Mapi(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_200.HTTP_200_Outlook_RPC(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_200.HTTP_200_Outlook_NSPI(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_200.HTTP_200_OnPremise_AutoDiscover_Redirect(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_200.HTTP_200_OnPremise_AutoDiscover_Redirect_AddressNotFound(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_200.HTTP_200_EXO_M365_AutoDiscover(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_200.HTTP_200_Unified_Groups_Settings(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_200.HTTP_200_3S_Suggestions(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_200.HTTP_200_REST_People_Request(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_200.HTTP_200_Any_Other_Exchange_EWS(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_200.HTTP_200_Lurking_Errors(this.session);
                    break;
                case 201:
                    HTTP_201 http_201 = new HTTP_201();
                    http_201.HTTP_201_Created(this.session);
                    break;
                case 202:
                    HTTP_202 http_202 = new HTTP_202();
                    http_202.HTTP_202_Accepted(this.session);
                    break;
                case 203:
                    HTTP_203 http_203 = new HTTP_203();
                    http_203.HTTP_203_NonAuthoritive_Answer(this.session);
                    break;
                case 204:
                    HTTP_204 http_204 = new HTTP_204();
                    http_204.HTTP_204_No_Content(this.session);
                    break;
                case 205:
                    HTTP_205 http_205 = new HTTP_205();
                    http_205.HTTP_205_Reset_Content(this.session);
                    break;
                case 206:
                    HTTP_206 http_206 = new HTTP_206();
                    http_206.HTTP_206_Partial_Content(this.session);
                    break;
                case 207:
                    HTTP_207 http_207 = new HTTP_207();
                    http_207.HTTP_207_Multi_Status(this.session);
                    break;
                case 208:
                    HTTP_208 http_208 = new HTTP_208();
                    http_208.HTTP_208_Already_Reported(this.session);
                    break;
                case 218:
                    HTTP_218 http_218 = new HTTP_218();
                    http_218.HTTP_218_This_Is_Fine_Apache_Web_Server(this.session);
                    break;
                case 226:
                    HTTP_226 http_226 = new HTTP_226();
                    http_226.HTTP_226_IM_Used(this.session);
                    break;
                case 300:
                    HTTP_300 http_300 = new HTTP_300();
                    http_300.HTTP_300_Multiple_Choices(this.session);
                    break;
                case 301:
                    HTTP_301 http_301 = new HTTP_301();
                    http_301.HTTP_301_Permanently_Moved(this.session);
                    break;
                case 302:
                    HTTP_302 http_302 = new HTTP_302();
                    http_302.HTTP_302_Redirect(this.session);
                    break;
                case 303:
                    HTTP_303 http_303 = new HTTP_303();
                    http_303.HTTP_303_See_Other(this.session);
                    break;
                case 304:
                    HTTP_304 http_304 = new HTTP_304();
                    http_304.HTTP_304_Not_Modified(this.session);
                    break;
                case 305:
                    HTTP_305 http_305 = new HTTP_305();
                    http_305.HTTP_305_Use_Proxy(this.session);
                    break;
                case 306:
                    HTTP_306 http_306 = new HTTP_306();
                    http_306.HTTP_306_Switch_Proxy(this.session);
                    break;
                case 307:
                    HTTP_307 http_307 = new HTTP_307();
                    http_307.HTTP_307_Temporary_Redirect(this.session);
                    break;
                case 308:
                    HTTP_308 http_308 = new HTTP_308();
                    http_308.HTTP_308_Permenant_Redirect(this.session);
                    break;
                case 400:
                    HTTP_400 http_400 = new HTTP_400();
                    http_400.HTTP_400_Bad_Request(this.session);
                    break;
                case 401:
                    HTTP_401 http_401 = new HTTP_401();
                    http_401.HTTP_401_Exchange_Online_AutoDiscover(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_401.HTTP_401_Exchange_OnPremise_AutoDiscover(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_401.HTTP_401_EWS(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_401.HTTP_401_Everything_Else(this.session);
                    break;
                case 402:
                    HTTP_402 http_402 = new HTTP_402();
                    http_402.HTTP_402_Payment_Required(this.session);
                    break;
                case 403:
                    HTTP_403 http_403 = new HTTP_403();
                    http_403.HTTP_403_Forbidden(this.session);
                    break;
                case 404:
                    HTTP_404 http_404 = new HTTP_404();
                    http_404.HTTP_404_Not_Found(this.session);
                    break;
                case 405:
                    HTTP_405 http_405 = new HTTP_405();
                    http_405.HTTP_405_Method_Not_Allowed(this.session);
                    break;
                case 406:
                    HTTP_406 http_406 = new HTTP_406();
                    http_406.HTTP_406_Not_Acceptable(this.session);
                    break;
                case 407:
                    HTTP_407 http_407 = new HTTP_407();
                    http_407.HTTP_407_Proxy_Auth_Required(this.session);
                    break;
                case 408:
                    HTTP_408 http_408 = new HTTP_408();
                    http_408.HTTP_408_Request_Timeout(this.session);
                    break;
                case 409:
                    HTTP_409 http_409 = new HTTP_409();
                    http_409.HTTP_409_Conflict(this.session);
                    break;
                case 410:
                    HTTP_410 http_410 = new HTTP_410();
                    http_410.HTTP_410_Gone(this.session);
                    break;
                case 411:
                    HTTP_411 http_411 = new HTTP_411();
                    http_411.HTTP_411_Length_Required(this.session);
                    break;
                case 412:
                    HTTP_412 http_412 = new HTTP_412();
                    http_412.HTTP_412_Precondition_Failed(this.session);
                    break;
                case 413:
                    HTTP_413 http_413 = new HTTP_413();
                    http_413.HTTP_413_Payload_Too_Large(this.session);
                    break;
                case 414:
                    HTTP_414 http_414 = new HTTP_414();
                    http_414.HTTP_414_URI_Too_Long(this.session);
                    break;
                case 415:
                    HTTP_415 http_415 = new HTTP_415();
                    http_415.HTTP_415_UnSupported_Media_Type(this.session);
                    break;
                case 416:
                    HTTP_416 http_416 = new HTTP_416();
                    http_416.HTTP_416_Range_Not_Satisfiable(this.session);
                    break;
                case 417:
                    HTTP_417 http_417 = new HTTP_417();
                    http_417.HTTP_417_Expectation_Failed(this.session);
                    break;
                case 418:
                    HTTP_418 http_418 = new HTTP_418();
                    http_418.HTTP_418_Im_A_Teapot(this.session);
                    break;
                case 419:
                    HTTP_419 http_419 = new HTTP_419();
                    http_419.HTTP_419_Page_Expired(this.session);
                    break;
                case 420:
                    HTTP_420 http_420 = new HTTP_420();
                    http_420.HTTP_420_Method_Failure_or_Enchance_Your_Calm(this.session);
                    break;
                case 421:
                    HTTP_421 http_421 = new HTTP_421();
                    http_421.HTTP_421_Misdirected_Request(this.session);
                    break;
                case 422:
                    HTTP_422 http_422 = new HTTP_422();
                    http_422.HTTP_422_Unprocessable_Entry(this.session);
                    break;
                case 423:
                    HTTP_423 http_423 = new HTTP_423();
                    http_423.HTTP_423_Locked(this.session);

                    break;
                case 424:
                    HTTP_424 http_424 = new HTTP_424();
                    http_424.HTTP_424_Failed_Dependency(this.session);
                    break;
                case 425:
                    HTTP_425 http_425 = new HTTP_425();
                    http_425.HTTP_425_Too_Early(this.session);
                    break;
                case 426:
                    HTTP_426 http_426 = new HTTP_426();
                    http_426.HTTP_426_Upgrade_Required(this.session);
                    break;
                case 428:
                    HTTP_428 http_428 = new HTTP_428();
                    http_428.HTTP_428_Precondition_Required(this.session);
                    break;
                case 429:
                    HTTP_429 http_429 = new HTTP_429();
                    http_429.HTTP_429_Too_Many_Requests(this.session);
                    break;
                case 430:
                    HTTP_430 http_430 = new HTTP_430();
                    http_430.HTTP_430_Request_Header_Feilds_Too_Large(this.session);
                    break;
                case 431:
                    HTTP_431 http_431 = new HTTP_431();
                    http_431.HTTP_431_Request_Header_Fields_Too_Large(this.session);
                    break;
                case 440:
                    HTTP_440 http_440 = new HTTP_440();
                    http_440.HTTP_440_IIS_Login_Timeout(this.session);
                    break;
                case 449:
                    HTTP_449 http_449 = new HTTP_449();
                    http_449.HTTP_449_IIS_Retry_With(this.session);
                    break;
                case 450:
                    HTTP_450 http_450 = new HTTP_450();
                    http_450.HTTP_450_Blocked_by_Windows_Parental_Controls(this.session);
                    break;
                case 451:
                    HTTP_451 http_451 = new HTTP_451();
                    http_451.HTTP_451_Unavailable_For_Legal_Reasons_or_IIS_Redirect(this.session);
                    break;
                case 456:
                    HTTP_456 http_456 = new HTTP_456();
                    http_456.HTTP_456_Multi_Factor_Required(this.session);
                    break;
                case 460:
                    HTTP_460 http_460 = new HTTP_460();
                    http_460.HTTP_460_Load_Balancer_Timeout(this.session);
                    break;
                case 463:
                    HTTP_463 http_463 = new HTTP_463();
                    http_463.HTTP_463_X_Forwarded_For_Header(this.session);
                    break;
                case 494:
                    HTTP_494 http_494 = new HTTP_494();
                    http_494.HTTP_494_Request_Header_Too_Large(this.session);
                    break;
                case 495:
                    HTTP_495 http_495 = new HTTP_495();
                    http_495.HTTP_495_SSL_Certificate_Error(this.session);
                    break;
                case 496:
                    HTTP_496 http_496 = new HTTP_496();
                    http_496.HTTP_496_SSL_Certificate_Required(this.session);
                    break;
                case 497:
                    HTTP_497 http_497 = new HTTP_497();
                    http_497.HTTP_497_Request_Sent_To_HTTPS_Port(this.session);
                    break;
                case 498:
                    HTTP_498 http_498 = new HTTP_498();
                    http_498.HTTP_498_Invalid_Token(this.session);
                    break;
                case 499:
                    HTTP_499 http_499 = new HTTP_499();
                    http_499.HTTP_499_Token_Required_or_Client_Closed_Request(this.session);
                    break;
                case 500:
                    HTTP_500 http_500 = new HTTP_500();
                    http_500.HTTP_500_Internal_Server_Error_Repeating_Redirects(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_500.HTTP_500_Internal_Server_Error_Impersonate_User_Denied(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_500.HTTP_500_Internal_Server_Error_OWA_Something_Went_Wrong(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_500.HTTP_500_Internal_Server_Error_All_Others(this.session);
                    break;
                case 501:
                    HTTP_501 http_501 = new HTTP_501();
                    http_501.HTTP_501_Not_Implemented(this.session);
                    break;
                case 502:
                    HTTP_502 http_502 = new HTTP_502();
                    http_502.HTTP_502_Bad_Gateway_Telemetry_False_Positive(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_502.HTTP_502_Bad_Gateway_EXO_DNS_Lookup_False_Positive(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_502.HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_502.HTTP_502_Bad_Gateway_Vanity_Domain_M365_AutoDiscover_False_Positive(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_502.HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_502.HTTP_502_Bad_Gateway_Anything_Else(this.session);
                    break;
                case 503:
                    HTTP_503 http_503 = new HTTP_503();
                    http_503.HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_503.HTTP_503_Service_Unavailable_Everything_Else(this.session);
                    break;
                case 504:
                    HTTP_504 http_504 = new HTTP_504();
                    http_504.HTTP_504_Gateway_Timeout_Internet_Access_Blocked(this.session);

                    if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
                    {
                        break;
                    }

                    http_504.HTTP_504_Gateway_Timeout_Anything_Else(this.session);
                    break;
                case 505:
                    HTTP_505 http_505 = new HTTP_505();
                    http_505.HTTP_505_HTTP_Version_Not_Supported(this.session);

                    break;
                case 506:
                    HTTP_506 http_506 = new HTTP_506();
                    http_506.HTTP_506_Variant_Also_Negociates(this.session);
                    break;
                case 507:
                    HTTP_507 http_507 = new HTTP_507();
                    http_507.HTTP_507_Insufficient_Storage(this.session);
                    break;
                case 508:
                    HTTP_508 http_508 = new HTTP_508();
                    http_508.HTTP_508_Loop_Detected(this.session);
                    break;
                case 509:
                    HTTP_509 http_509 = new HTTP_509();
                    http_509.HTTP_509_Bandwidth_Limit_Exceeeded(this.session);
                    break;
                case 510:
                    HTTP_510 http_510 = new HTTP_510();
                    http_510.HTTP_510_Not_Extended(this.session);
                    break;
                case 511:
                    HTTP_511 http_511 = new HTTP_511();
                    http_511.HTTP_511_Network_Authentication_Required(this.session);
                    break;
                case 520:
                    HTTP_520 http_520 = new HTTP_520();
                    http_520.HTTP_520_Web_Server_Returned_an_Unknown_Error(this.session);
                    break;
                case 521:
                    HTTP_521 http_521 = new HTTP_521();
                    http_521.HTTP_521_Web_Server_Is_Down(this.session);
                    break;
                case 522:
                    HTTP_522 http_522 = new HTTP_522();
                    http_522.HTTP_522_Connection_Timed_Out(this.session);
                    break;
                case 523:
                    HTTP_523 http_523 = new HTTP_523();
                    http_523.HTTP_523_Origin_Is_Unreachable(this.session);
                    break;
                case 524:
                    HTTP_524 http_524 = new HTTP_524();
                    http_524.HTTP_524_A_Timeout_Occurred(this.session);
                    break;
                case 525:
                    HTTP_525 http_525 = new HTTP_525();
                    http_525.HTTP_525_SSL_Handshake_Failed(this.session);
                    break;
                case 526:
                    HTTP_526 http_526 = new HTTP_526();
                    http_526.HTTP_526_Invalid_SSL_Certificate(this.session);
                    break;
                case 527:
                    HTTP_527 http_527 = new HTTP_527();
                    http_527.HTTP_527_Railgun_Error(this.session);
                    break;
                case 529:
                    HTTP_529 http_529 = new HTTP_529();
                    http_529.HTTP_529_Site_Is_Overloaded(this.session);
                    break;
                case 530:
                    HTTP_530 http_530 = new HTTP_530();
                    http_530.HTTP_530_Site_Is_Frozen(this.session);
                    break;
                case 561:
                    HTTP_561 http_561 = new HTTP_561();
                    http_561.HTTP_561_Unauthorized(this.session);
                    break;
                case 598:
                    HTTP_598 http_598 = new HTTP_598();
                    http_598.HTTP_598_Network_Read_Timeout_Error(this.session);
                    break;
                default:
                    // Not setting colours on sessions not recognised.
                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Session undefined in extension.");

                    getSetSessionFlags.SetUIBackColour(this.session, "Gray");
                    getSetSessionFlags.SetUITextColour(this.session, "Black");

                    getSetSessionFlags.SetSessionType(this.session, "Undefined");
                    getSetSessionFlags.SetXResponseAlert(this.session, "Undefined");
                    getSetSessionFlags.SetXResponseCommentsNoKnownIssue(this.session);

                    getSetSessionFlags.SetResponseCodeDescription(this.session, "Defaulted. HTTP Response Code undefined.");

                    // Nothing meaningful here, let further processing try to pick up something.
                    getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "0");
                    getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "0");
                    getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "0");
                    break;
            }
        }
    }
}