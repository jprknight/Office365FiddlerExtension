﻿using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Handler;
using Office365FiddlerExtension.Ruleset;
using Office365FiddlerExtension.Services;
using Office365FiddlerExtension.UI;
using System.Reflection;

namespace Office365FiddlerExtension
{
    /// <summary>
    /// Function that calls all logic together to run against each session loaded in Fiddler.
    /// </summary>
    public class SessionHandler : ActivationService
    {
        private static SessionHandler _instance;

        public static SessionHandler Instance => _instance ?? (_instance = new SessionHandler());

        public void OnPeekAtResponseHeaders(Session Session)
        {
            this.Session = Session;

            //SessionFlagHandler.Instance.CreateExtensionSessionFlag(this.Session);

            // Decode session requests/responses.
            this.Session.utilDecodeRequest(true);
            this.Session.utilDecodeResponse(true);

            ///////////////////////////////
            ///
            // Always run these functions on every session.

            // Broad logic checks on sessions regardless of response code.
            //BroadLogicChecks.Instance.FiddlerUpdateSessions(this.Session);
            RuleSetHandler.Instance.RunAssembly(this.Session);
            BroadLogicChecks.Instance.ConnectTunnelSessions(this.Session);
            BroadLogicChecks.Instance.ApacheAutodiscover(this.Session);
            BroadLogicChecks.Instance.LoopBackTunnel(this.Session);

            // Calculate Session Age for inspector with HTML mark-up.
            CalculateSessionAge.Instance.SessionAge(this.Session);

            // Set Server Think Time and Transit Time for inspector with HTML mark-up.
            ServerThinkTimeTransitTime.Instance.SetServerThinkTimeTransitTime(this.Session);

            // Set Elapsed Time column data.
            SessionElapsedTime.Instance.SetElapsedTime(this.Session);
            SessionElapsedTime.Instance.SetInspectorElapsedTime(this.Session);

            // Set Process Name.
            ProcessName.Instance.SetProcessName(this.Session);

            ///////////////////////////////
            ///
            // From here on out only run functions where there isn't a high level of confidence
            // on session classification.
            var ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                Instance.ResponseCodeLogic(this.Session);
            }

            ///////////////////////////////
            // AUTHENTICATION
            #region Authentication
            // If the session does not already have a high auth classification confidence, run.
            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionAuthenticationConfidenceLevel < 10)
            {
                Authentication.Instance.SetAuthentication_NoAuthHeaders(this.Session);                
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionAuthenticationConfidenceLevel < 10)
            {
                Authentication.Instance.SetAuthentication_SAML_Parser(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionAuthenticationConfidenceLevel < 10)
            {
                Authentication.Instance.SetAuthentication_Basic_Modern_Auth_Disabled(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionAuthenticationConfidenceLevel < 10)
            {
                Authentication.Instance.SetAuthentication_Modern_Auth_Capable_Client(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionAuthenticationConfidenceLevel < 10)
            {
                Authentication.Instance.SetAuthentication_Basic_Auth_Capable_Client(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionAuthenticationConfidenceLevel < 10)
            {
                Authentication.Instance.SetAuthentication_Modern_Auth_Client_Using_Token(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionAuthenticationConfidenceLevel < 10)
            {
                Authentication.Instance.SetAuthentication_Basic_Auth_Client_Using_Token(this.Session);
            }
            #endregion

            ///////////////////////////////
            // SESSION TYPE
            #region SessionType
            // If the session does not already have a high session type classification confidence, run these functions.
            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10) 
            {
                SessionType.Instance.SetSessionType_FreeBusy(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                SessionType.Instance.SetSessionType_Microsoft365_EWS(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                SessionType.Instance.SetSessionType_EWS(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                SessionType.Instance.SetSessionType_Microsoft365_Authentication(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                SessionType.Instance.SetSessionType_ADFS_Authentication(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                SessionType.Instance.SetSessionType_General_Microsoft365(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                SessionType.Instance.SetSessionType_Office_Applications(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                SessionType.Instance.SetSessionType_Internet_Browsers(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                SessionType.Instance.SetSessionType_Unknown(this.Session);
            }
            #endregion

            ///////////////////////////////
            // RESPONSE SERVER
            #region ResponseServer
            // If the session does not already have a high response server classification confidence, run
            // this function as a last effort to classify the session type.
            // None of these overlap, so not checking SessionResponseServerConfidenceLevel before running each function.
            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionResponseServerConfidenceLevel < 10)
            {
                ResponseServer.Instance.SetResponseServer_Server(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionResponseServerConfidenceLevel < 10)
            {
                ResponseServer.Instance.SetResponseServer_Host(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionResponseServerConfidenceLevel < 10)
            {
                ResponseServer.Instance.SetResponseServer_PoweredBy(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionResponseServerConfidenceLevel < 10)
            {
                ResponseServer.Instance.SetResponseServer_ServedBy(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionResponseServerConfidenceLevel < 10)
            {
                ResponseServer.Instance.SetResponseServer_ServerName(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionResponseServerConfidenceLevel < 10)
            {
                ResponseServer.Instance.SetResponseServer_Unknown(this.Session);
            }
            #endregion

            ///////////////////////////////
            // LONG RUNNING SESSIONS
            #region LongRunningSessions
            // If session has not been classified run Long Running Session override functions.
            // In relatively few scenarios has roundtrip time been an underlying cause.
            // So this is the last function to run after all other logic has been exhausted.
            // Typically network traces are used to validate the underlying network connectivity.
            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                LongRunningSessions.Instance.LongRunningSessionsWarning(this.Session);               
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                LongRunningSessions.Instance.LongRunningSessionsClientSlow(this.Session);
            }

            ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                LongRunningSessions.Instance.LongRunningSessionsServerSlow(this.Session);
            }
            #endregion

            EnhanceSessionUX.Instance.EnhanceSession(this.Session);
        }

        // Function containing switch statement for response code logic.
        // https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
        public void ResponseCodeLogic(Session Session)
        {
            this.Session = Session;
            
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} Running ResponseCodeLogic.");

            switch (this.Session.responseCode)
            {
                case 0:
                    HTTP_0.Instance.HTTP_0_NoSessionResponse(this.Session);
                    break;
                case 103:
                    HTTP_103.Instance.HTTP_103_Checkpoint(this.Session);
                    break;
                case 200:
                    HTTP_200.Instance.HTTP_200_ClientAccessRule(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200.Instance.HTTP_200_Outlook_Mapi_Microsoft365_Protocol_Disabled(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200.Instance.HTTP_200_Outlook_Exchange_Online_Microsoft_365_Mapi(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200.Instance.HTTP_200_Outlook_Exchange_OnPremise_Mapi(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200.Instance.HTTP_200_Outlook_Web_App(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200.Instance.HTTP_200_Outlook_RPC(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200.Instance.HTTP_200_Outlook_NSPI(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200.Instance.HTTP_200_OnPremise_AutoDiscover_Redirect_Address_Found(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200.Instance.HTTP_200_OnPremise_AutoDiscover_Redirect_AddressNotFound(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////
                    
                    HTTP_200.Instance.HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200.Instance.HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200.Instance.HTTP_200_Unified_Groups_Settings(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200.Instance.HTTP_200_3S_Suggestions(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200.Instance.HTTP_200_REST_People_Request(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200.Instance.HTTP_200_OnPremise_Any_Other_EWS(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200.Instance.HTTP_200_Microsoft365_Any_Other_EWS(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200.Instance.HTTP_200_Lurking_Errors(this.Session);
                    break;
                case 201:
                    HTTP_201.Instance.HTTP_201_Created(this.Session);
                    break;
                case 202:
                    HTTP_202.Instance.HTTP_202_Accepted(this.Session);
                    break;
                case 203:
                    HTTP_203.Instance.HTTP_203_NonAuthoritive_Answer(this.Session);
                    break;
                case 204:
                    HTTP_204.Instance.HTTP_204_No_Content(this.Session);
                    break;
                case 205:
                    HTTP_205.Instance.HTTP_205_Reset_Content(this.Session);
                    break;
                case 206:
                    HTTP_206.Instance.HTTP_206_Partial_Content(this.Session);
                    break;
                case 207:
                    HTTP_207.Instance.HTTP_207_Multi_Status(this.Session);
                    break;
                case 208:
                    HTTP_208.Instance.HTTP_208_Already_Reported(this.Session);
                    break;
                case 218:
                    HTTP_218.Instance.HTTP_218_This_Is_Fine_Apache_Web_Server(this.Session);
                    break;
                case 226:
                    HTTP_226.Instance.HTTP_226_IM_Used(this.Session);
                    break;
                case 300:
                    HTTP_300.Instance.HTTP_300_Multiple_Choices(this.Session);
                    break;
                case 301:
                    HTTP_301.Instance.HTTP_301_Permanently_Moved(this.Session);
                    break;
                case 302:
                    HTTP_302.Instance.HTTP_302_Redirect_AutoDiscover(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }
                    HTTP_302.Instance.HTTP_302_Redirect_AllOthers(this.Session);
                    break;
                case 303:
                    HTTP_303.Instance.HTTP_303_See_Other(this.Session);
                    break;
                case 304:
                    HTTP_304.Instance.HTTP_304_Not_Modified(this.Session);
                    break;
                case 305:
                    HTTP_305.Instance.HTTP_305_Use_Proxy(this.Session);
                    break;
                case 306:
                    HTTP_306.Instance.HTTP_306_Switch_Proxy(this.Session);
                    break;
                case 307:
                    HTTP_307.Instance.HTTP_307_AutoDiscover_Temporary_Redirect(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }
                    HTTP_307.Instance.HTTP_307_Other_AutoDiscover_Redirects(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }
                    HTTP_307.Instance.HTTP_307_All_Other_Redirects(this.Session);
                    break;
                case 308:
                    HTTP_308.Instance.HTTP_308_Permenant_Redirect(this.Session);
                    break;
                case 400:
                    HTTP_400.Instance.HTTP_400_Bad_Request(this.Session);
                    break;
                case 401:
                    HTTP_401.Instance.HTTP_401_Exchange_Online_AutoDiscover(this.Session);
                    if (SessionFlagHandler.Instance.GetAnySessionConfidenceLevelTen(this.Session))
                    {
                        break;
                    }

                    HTTP_401.Instance.HTTP_401_Exchange_OnPremise_AutoDiscover(this.Session);
                    if (SessionFlagHandler.Instance.GetAnySessionConfidenceLevelTen(this.Session))
                    {
                        break;
                    }

                    HTTP_401.Instance.HTTP_401_EWS(this.Session);
                    if (SessionFlagHandler.Instance.GetAnySessionConfidenceLevelTen(this.Session))
                    {
                        break;
                    }

                    HTTP_401.Instance.HTTP_401_Everything_Else(this.Session);
                    break;
                case 402:
                    HTTP_402.Instance.HTTP_402_Payment_Required(this.Session);
                    break;
                case 403:
                    HTTP_403.Instance.HTTP_403_Forbidden_Proxy_Block(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }
                    HTTP_403.Instance.HTTP_403_Forbidden_EWS_Mailbox_Language_Not_Set(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }
                    HTTP_403.Instance.HTTP_403_Forbidden_Everything_Else(this.Session);
                    break;
                case 404:
                    HTTP_404.Instance.HTTP_404_Not_Found(this.Session);
                    break;
                case 405:
                    HTTP_405.Instance.HTTP_405_Method_Not_Allowed(this.Session);
                    break;
                case 406:
                    HTTP_406.Instance.HTTP_406_Not_Acceptable(this.Session);
                    break;
                case 407:
                    HTTP_407.Instance.HTTP_407_Proxy_Auth_Required(this.Session);
                    break;
                case 408:
                    HTTP_408.Instance.HTTP_408_Request_Timeout(this.Session);
                    break;
                case 409:
                    HTTP_409.Instance.HTTP_409_Conflict(this.Session);
                    break;
                case 410:
                    HTTP_410.Instance.HTTP_410_Gone(this.Session);
                    break;
                case 411:
                    HTTP_411.Instance.HTTP_411_Length_Required(this.Session);
                    break;
                case 412:
                    HTTP_412.Instance.HTTP_412_Precondition_Failed(this.Session);
                    break;
                case 413:
                    HTTP_413.Instance.HTTP_413_Payload_Too_Large(this.Session);
                    break;
                case 414:
                    HTTP_414.Instance.HTTP_414_URI_Too_Long(this.Session);
                    break;
                case 415:
                    HTTP_415.Instance.HTTP_415_UnSupported_Media_Type(this.Session);
                    break;
                case 416:
                    HTTP_416.Instance.HTTP_416_Range_Not_Satisfiable(this.Session);
                    break;
                case 417:
                    HTTP_417.Instance.HTTP_417_Expectation_Failed(this.Session);
                    break;
                case 418:
                    HTTP_418.Instance.HTTP_418_Im_A_Teapot(this.Session);
                    break;
                case 419:
                    HTTP_419.Instance.HTTP_419_Page_Expired(this.Session);
                    break;
                case 420:
                    HTTP_420.Instance.HTTP_420_Method_Failure_or_Enchance_Your_Calm(this.Session);
                    break;
                case 421:
                    HTTP_421.Instance.HTTP_421_Misdirected_Request(this.Session);
                    break;
                case 422:
                    HTTP_422.Instance.HTTP_422_Unprocessable_Entry(this.Session);
                    break;
                case 423:
                    HTTP_423.Instance.HTTP_423_Locked(this.Session);

                    break;
                case 424:
                    HTTP_424.Instance.HTTP_424_Failed_Dependency(this.Session);
                    break;
                case 425:
                    HTTP_425.Instance.HTTP_425_Too_Early(this.Session);
                    break;
                case 426:
                    HTTP_426.Instance.HTTP_426_Upgrade_Required(this.Session);
                    break;
                case 428:
                    HTTP_428.Instance.HTTP_428_Precondition_Required(this.Session);
                    break;
                case 429:
                    HTTP_429.Instance.HTTP_429_Too_Many_Requests(this.Session);
                    break;
                case 430:
                    HTTP_430.Instance.HTTP_430_Request_Header_Feilds_Too_Large(this.Session);
                    break;
                case 431:
                    HTTP_431.Instance.HTTP_431_Request_Header_Fields_Too_Large(this.Session);
                    break;
                case 440:
                    HTTP_440.Instance.HTTP_440_IIS_Login_Timeout(this.Session);
                    break;
                case 449:
                    HTTP_449.Instance.HTTP_449_IIS_Retry_With(this.Session);
                    break;
                case 450:
                    HTTP_450.Instance.HTTP_450_Blocked_by_Windows_Parental_Controls(this.Session);
                    break;
                case 451:
                    HTTP_451.Instance.HTTP_451_Unavailable_For_Legal_Reasons_or_IIS_Redirect(this.Session);
                    break;
                case 456:
                    HTTP_456.Instance.HTTP_456_Multi_Factor_Required(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }
                    HTTP_456.Instance.HTTP_456_OAuth_Not_Available(this.Session);
                    if (SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }
                    HTTP_456.Instance.HTTP_456_Anything_Else(this.Session);
                    break;
                case 460:
                    HTTP_460.Instance.HTTP_460_Load_Balancer_Timeout(this.Session);
                    break;
                case 463:
                    HTTP_463.Instance.HTTP_463_X_Forwarded_For_Header(this.Session);
                    break;
                case 494:
                    HTTP_494.Instance.HTTP_494_Request_Header_Too_Large(this.Session);
                    break;
                case 495:
                    HTTP_495.Instance.HTTP_495_SSL_Certificate_Error(this.Session);
                    break;
                case 496:
                    HTTP_496.Instance.HTTP_496_SSL_Certificate_Required(this.Session);
                    break;
                case 497:
                    HTTP_497.Instance.HTTP_497_Request_Sent_To_HTTPS_Port(this.Session);
                    break;
                case 498:
                    HTTP_498.Instance.HTTP_498_Invalid_Token(this.Session);
                    break;
                case 499:
                    HTTP_499.Instance.HTTP_499_Token_Required_or_Client_Closed_Request(this.Session);
                    break;
                case 500:
                    HTTP_500.Instance.HTTP_500_Internal_Server_Error_Repeating_Redirects(this.Session);
                    if (SessionFlagHandler.Instance.GetAnySessionConfidenceLevelTen(this.Session))
                    {
                        break;
                    }

                    HTTP_500.Instance.HTTP_500_Internal_Server_Error_Impersonate_User_Denied(this.Session);
                    if (SessionFlagHandler.Instance.GetAnySessionConfidenceLevelTen(this.Session))
                    {
                        break;
                    }

                    HTTP_500.Instance.HTTP_500_Internal_Server_Error_OWA_Something_Went_Wrong(this.Session);
                    if (SessionFlagHandler.Instance.GetAnySessionConfidenceLevelTen(this.Session))
                    {
                        break;
                    }

                    HTTP_500.Instance.HTTP_500_Internal_Server_Error_All_Others(this.Session);
                    break;
                case 501:
                    HTTP_501.Instance.HTTP_501_Not_Implemented(this.Session);
                    break;
                case 502:
                    HTTP_502.Instance.HTTP_502_Bad_Gateway_Telemetry_False_Positive(this.Session);
                    if (SessionFlagHandler.Instance.GetAnySessionConfidenceLevelTen(this.Session))
                    {
                        break;
                    }

                    HTTP_502.Instance.HTTP_502_Bad_Gateway_EXO_DNS_Lookup_False_Positive(this.Session);
                    if (SessionFlagHandler.Instance.GetAnySessionConfidenceLevelTen(this.Session))
                    {
                        break;
                    }

                    HTTP_502.Instance.HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive(this.Session);
                    if (SessionFlagHandler.Instance.GetAnySessionConfidenceLevelTen(this.Session))
                    {
                        break;
                    }

                    HTTP_502.Instance.HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover(this.Session);
                    if (SessionFlagHandler.Instance.GetAnySessionConfidenceLevelTen(this.Session))
                    {
                        break;
                    }

                    HTTP_502.Instance.HTTP_502_Bad_Gateway_Anything_Else(this.Session);
                    break;
                case 503:
                    HTTP_503.Instance.HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable(this.Session);
                    if (SessionFlagHandler.Instance.GetAnySessionConfidenceLevelTen(this.Session))
                    {
                        break;
                    }

                    HTTP_503.Instance.HTTP_503_Service_Unavailable_Everything_Else(this.Session);
                    break;
                case 504:
                    HTTP_504.Instance.HTTP_504_Gateway_Timeout_Internet_Access_Blocked(this.Session);
                    if (SessionFlagHandler.Instance.GetAnySessionConfidenceLevelTen(this.Session))
                    {
                        break;
                    }

                    HTTP_504.Instance.HTTP_504_Gateway_Timeout_Anything_Else(this.Session);
                    break;
                case 505:
                    HTTP_505.Instance.HTTP_505_HTTP_Version_Not_Supported(this.Session);

                    break;
                case 506:
                    HTTP_506.Instance.HTTP_506_Variant_Also_Negociates(this.Session);
                    break;
                case 507:
                    HTTP_507.Instance.HTTP_507_Insufficient_Storage(this.Session);
                    break;
                case 508:
                    HTTP_508.Instance.HTTP_508_Loop_Detected(this.Session);
                    break;
                case 509:
                    HTTP_509.Instance.HTTP_509_Bandwidth_Limit_Exceeeded(this.Session);
                    break;
                case 510:
                    HTTP_510.Instance.HTTP_510_Not_Extended(this.Session);
                    break;
                case 511:
                    HTTP_511.Instance.HTTP_511_Network_Authentication_Required(this.Session);
                    break;
                case 520:
                    HTTP_520.Instance.HTTP_520_Web_Server_Returned_an_Unknown_Error(this.Session);
                    break;
                case 521:
                    HTTP_521.Instance.HTTP_521_Web_Server_Is_Down(this.Session);
                    break;
                case 522:
                    HTTP_522.Instance.HTTP_522_Connection_Timed_Out(this.Session);
                    break;
                case 523:
                    HTTP_523.Instance.HTTP_523_Origin_Is_Unreachable(this.Session);
                    break;
                case 524:
                    HTTP_524.Instance.HTTP_524_A_Timeout_Occurred(this.Session);
                    break;
                case 525:
                    HTTP_525.Instance.HTTP_525_SSL_Handshake_Failed(this.Session);
                    break;
                case 526:
                    HTTP_526.Instance.HTTP_526_Invalid_SSL_Certificate(this.Session);
                    break;
                case 527:
                    HTTP_527.Instance.HTTP_527_Railgun_Error(this.Session);
                    break;
                case 529:
                    HTTP_529.Instance.HTTP_529_Site_Is_Overloaded(this.Session);
                    break;
                case 530:
                    HTTP_530.Instance.HTTP_530_Site_Is_Frozen(this.Session);
                    break;
                case 561:
                    HTTP_561.Instance.HTTP_561_Unauthorized(this.Session);
                    break;
                case 598:
                    HTTP_598.Instance.HTTP_598_Network_Read_Timeout_Error(this.Session);
                    break;
                default:
                    // Not setting colours on sessions not recognised.

                    var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                    {
                        SectionTitle = "Session undefined in extension.",
                        UIBackColour = "Gray",
                        UITextColour = "Black",

                        SessionType = "Undefined",
                        ResponseCodeDescription = "Defaulted. HTTP Response Code undefined.",
                        ResponseAlert = "Undefined",
                        ResponseComments = SessionFlagHandler.Instance.ResponseCommentsNoKnownIssue(),

                        SessionAuthenticationConfidenceLevel = 0,
                        SessionTypeConfidenceLevel = 0,
                        SessionResponseServerConfidenceLevel = 0
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
                    break;
            }
        }
    }
}