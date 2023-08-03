using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using Office365FiddlerExtensionRuleset.Ruleset;
using Office365FiddlerExtensionRuleset.Ruleset.AlwaysRun.BroadLogicChecks;

namespace Office365FiddlerExtensionRuleset
{
    public class RunRuleSet
    {
        internal Session session { get; set; }

        public void Initialize(Session session)
        {
            this.session = session;

            ///////////////////////////////
            ///
            // Always run these functions on every session.

            // Broad logic checks on sessions regardless of response code.
            FiddlerUpdateSessions.Instance.Run(this.session);
            ConnectTunnelSessions.Instance.Run(this.session);
            ApacheAutodiscover.Instance.Run(this.session);
            LoopBackTunnel.Instance.Run(this.session);

            // Calculate Session Age for inspector with HTML mark-up.
            CalculateSessionAge.Instance.SessionAge(this.session);

            // Set Server Think Time and Transit Time for inspector with HTML mark-up.
            ServerThinkTimeTransitTime.Instance.SetServerThinkTimeTransitTime(this.session);

            // Set Elapsed Time column data.
            SessionElapsedTime.Instance.SetElapsedTime(this.session);
            SessionElapsedTime.Instance.SetInspectorElapsedTime(this.session);

            // Set Process Name.
            ProcessName.Instance.SetProcessName(this.session);

            ///////////////////////////////
            ///
            // From here on out only run functions where there isn't a high level of confidence
            // on session classification.
            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                ResponseCodeLogic(this.session);
            }

            ///////////////////////////////
            // AUTHENTICATION
            #region Authentication
            // If the session does not already have a high auth classification confidence, run.
            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionAuthenticationConfidenceLevel < 10)
            {
                Authentication.Instance.SetAuthentication_NoAuthHeaders(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionAuthenticationConfidenceLevel < 10)
            {
                Authentication.Instance.SetAuthentication_SAML_Parser(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionAuthenticationConfidenceLevel < 10)
            {
                Authentication.Instance.SetAuthentication_Basic_Modern_Auth_Disabled(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionAuthenticationConfidenceLevel < 10)
            {
                Authentication.Instance.SetAuthentication_Modern_Auth_Capable_Client(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionAuthenticationConfidenceLevel < 10)
            {
                Authentication.Instance.SetAuthentication_Basic_Auth_Capable_Client(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionAuthenticationConfidenceLevel < 10)
            {
                Authentication.Instance.SetAuthentication_Modern_Auth_Client_Using_Token(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionAuthenticationConfidenceLevel < 10)
            {
                Authentication.Instance.SetAuthentication_Basic_Auth_Client_Using_Token(this.session);
            }
            #endregion

            ///////////////////////////////
            // SESSION TYPE
            #region SessionType
            // If the session does not already have a high session type classification confidence, run these functions.
            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                SessionType.Instance.SetSessionType_FreeBusy(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                SessionType.Instance.SetSessionType_Microsoft365_EWS(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                SessionType.Instance.SetSessionType_EWS(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                SessionType.Instance.SetSessionType_Microsoft365_Authentication(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                SessionType.Instance.SetSessionType_ADFS_Authentication(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                SessionType.Instance.SetSessionType_General_Microsoft365(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                SessionType.Instance.SetSessionType_Office_Applications(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                SessionType.Instance.SetSessionType_Internet_Browsers(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                SessionType.Instance.SetSessionType_Unknown(this.session);
            }
            #endregion

            ///////////////////////////////
            // RESPONSE SERVER
            #region ResponseServer
            // If the session does not already have a high response server classification confidence, run
            // this function as a last effort to classify the session type.
            // None of these overlap, so not checking SessionResponseServerConfidenceLevel before running each function.
            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionResponseServerConfidenceLevel < 10)
            {
                ResponseServer.Instance.SetResponseServer_Server(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionResponseServerConfidenceLevel < 10)
            {
                ResponseServer.Instance.SetResponseServer_Host(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionResponseServerConfidenceLevel < 10)
            {
                ResponseServer.Instance.SetResponseServer_PoweredBy(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionResponseServerConfidenceLevel < 10)
            {
                ResponseServer.Instance.SetResponseServer_ServedBy(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionResponseServerConfidenceLevel < 10)
            {
                ResponseServer.Instance.SetResponseServer_ServerName(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionResponseServerConfidenceLevel < 10)
            {
                ResponseServer.Instance.SetResponseServer_Unknown(this.session);
            }
            #endregion

            ///////////////////////////////
            // LONG RUNNING SESSIONS
            #region LongRunningSessions
            // If session has not been classified run Long Running Session override functions.
            // In relatively few scenarios has roundtrip time been an underlying cause.
            // So this is the last function to run after all other logic has been exhausted.
            // Typically network traces are used to validate the underlying network connectivity.
            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                LongRunningSessions.Instance.LongRunningSessionsWarning(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                LongRunningSessions.Instance.LongRunningSessionsClientSlow(this.session);
            }

            ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel < 10)
            {
                LongRunningSessions.Instance.LongRunningSessionsServerSlow(this.session);
            }
            #endregion
        }

        // Function containing switch statement for response code logic.
        // https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
        private void ResponseCodeLogic(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Running ResponseCodeLogic.");

            switch (this.session.responseCode)
            {
                case 0:
                    HTTP_0.Instance.HTTP_0_NoSessionResponse(this.session);
                    break;
                case 103:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_103s");
                    break;
                case 200:
                    HTTP_200_ClientAccessRule.Instance.Run(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200_Outlook_MAPI_Protocol_Disabled.Instance.Run(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200_Outlook_MAPI_Exchange_Online.Instance.Run(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200_Outlook_MAPI_Exchange_OnPremise.Instance.Run(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200_OWA.Instance.Run(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200_Outlook_RPC.Instance.Run(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200_Outlook_NSPI.Instance.Run(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_Address_Found.Instance.Run(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_AddressNotFound.Instance.Run(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun.Instance.Run(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun.Instance.Run(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200_Unified_Groups_Settings.Instance.Run(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200_3S_Suggestions.Instance.Run(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200_REST_People_Request.Instance.Run(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200_Exchange_OnPremise_Any_Other_EWS.Instance.Run(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200_Exchange_Online_Any_Other_EWS.Instance.Run(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200_Javascript.Instance.Run(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200_Lurking_Errors.Instance.Run(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    ///////////////////////////////

                    HTTP_200_Actually_OK.Instance.Run(this.session);
                    break;
                case 201:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_201s");
                    break;
                case 202:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_202s");
                    break;
                case 203:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_203s");
                    break;
                case 204:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_204s");
                    break;
                case 205:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_205s");
                    break;
                case 206:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_206s");
                    break;
                case 207:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_207s");
                    break;
                case 208:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_208s");
                    break;
                case 218:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_218s");
                    break;
                case 226:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_226s");
                    break;
                case 300:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_300s");
                    break;
                case 301:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_301s");
                    break;
                case 302:
                    HTTP_302.Instance.HTTP_302_Redirect_AutoDiscover(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }
                    HTTP_302.Instance.HTTP_302_Redirect_AllOthers(this.session);
                    break;
                case 303:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_303s");
                    break;
                case 304:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_304s");
                    break;
                case 305:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_305s");
                    break;
                case 306:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_306s");
                    break;
                case 307:
                    HTTP_307.Instance.HTTP_307_AutoDiscover_Temporary_Redirect(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }
                    HTTP_307.Instance.HTTP_307_Other_AutoDiscover_Redirects(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }
                    HTTP_307.Instance.HTTP_307_All_Other_Redirects(this.session);
                    break;
                case 308:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_308s");
                    break;
                case 400:
                    HTTP_400.Instance.HTTP_400_Bad_Request(this.session);
                    break;
                case 401:
                    HTTP_401.Instance.HTTP_401_Exchange_Online_AutoDiscover(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    HTTP_401.Instance.HTTP_401_Exchange_OnPremise_AutoDiscover(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    HTTP_401.Instance.HTTP_401_EWS(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    HTTP_401.Instance.HTTP_401_Everything_Else(this.session);
                    break;
                case 402:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_402s");
                    break;
                case 403:
                    HTTP_403.Instance.HTTP_403_Forbidden_Proxy_Block(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }
                    HTTP_403.Instance.HTTP_403_Forbidden_EWS_Mailbox_Language_Not_Set(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }
                    HTTP_403.Instance.HTTP_403_Forbidden_Everything_Else(this.session);
                    break;
                case 404:
                    HTTP_404.Instance.HTTP_404_Not_Found(this.session);
                    break;
                case 405:
                    HTTP_405.Instance.HTTP_405_Method_Not_Allowed(this.session);
                    break;
                case 406:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_406s");
                    break;
                case 407:
                    HTTP_407.Instance.HTTP_407_Proxy_Auth_Required(this.session);
                    break;
                case 408:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_408s");
                    break;
                case 409:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_409s");
                    break;
                case 410:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_410s");
                    break;
                case 411:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_411s");
                    break;
                case 412:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_412s");
                    break;
                case 413:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_413s");
                    break;
                case 414:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_414s");
                    break;
                case 415:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_415s");
                    break;
                case 416:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_416s");
                    break;
                case 417:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_417s");
                    break;
                case 418:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_418s");
                    break;
                case 419:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_419s");
                    break;
                case 420:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_420s");
                    break;
                case 421:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_421s");
                    break;
                case 422:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_422s");
                    break;
                case 423:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_423s");
                    break;
                case 424:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_424s");
                    break;
                case 425:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_425s");
                    break;
                case 426:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_426s");
                    break;
                case 428:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_428s");
                    break;
                case 429:
                    HTTP_429.Instance.HTTP_429_Too_Many_Requests(this.session);
                    break;
                case 430:
                    HTTP_430.Instance.HTTP_430_Request_Header_Feilds_Too_Large(this.session);
                    break;
                case 431:
                    HTTP_431.Instance.HTTP_431_Request_Header_Fields_Too_Large(this.session);
                    break;
                case 440:
                    HTTP_440.Instance.HTTP_440_IIS_Login_Timeout(this.session);
                    break;
                case 449:
                    HTTP_449.Instance.HTTP_449_IIS_Retry_With(this.session);
                    break;
                case 450:
                    HTTP_450.Instance.HTTP_450_Blocked_by_Windows_Parental_Controls(this.session);
                    break;
                case 451:
                    HTTP_451.Instance.HTTP_451_Unavailable_For_Legal_Reasons_or_IIS_Redirect(this.session);
                    break;
                case 456:
                    HTTP_456.Instance.HTTP_456_Multi_Factor_Required(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }
                    HTTP_456.Instance.HTTP_456_OAuth_Not_Available(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }
                    HTTP_456.Instance.HTTP_456_Anything_Else(this.session);
                    break;
                case 460:
                    HTTP_460.Instance.HTTP_460_Load_Balancer_Timeout(this.session);
                    break;
                case 463:
                    HTTP_463.Instance.HTTP_463_X_Forwarded_For_Header(this.session);
                    break;
                case 494:
                    HTTP_494.Instance.HTTP_494_Request_Header_Too_Large(this.session);
                    break;
                case 495:
                    HTTP_495.Instance.HTTP_495_SSL_Certificate_Error(this.session);
                    break;
                case 496:
                    HTTP_496.Instance.HTTP_496_SSL_Certificate_Required(this.session);
                    break;
                case 497:
                    HTTP_497.Instance.HTTP_497_Request_Sent_To_HTTPS_Port(this.session);
                    break;
                case 498:
                    HTTP_498.Instance.HTTP_498_Invalid_Token(this.session);
                    break;
                case 499:
                    HTTP_499.Instance.HTTP_499_Token_Required_or_Client_Closed_Request(this.session);
                    break;
                case 500:
                    HTTP_500.Instance.HTTP_500_Internal_Server_Error_Repeating_Redirects(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    HTTP_500.Instance.HTTP_500_Internal_Server_Error_Impersonate_User_Denied(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    HTTP_500.Instance.HTTP_500_Internal_Server_Error_OWA_Something_Went_Wrong(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    HTTP_500.Instance.HTTP_500_Internal_Server_Error_All_Others(this.session);
                    break;
                case 501:
                    HTTP_501.Instance.HTTP_501_Not_Implemented(this.session);
                    break;
                case 502:
                    HTTP_502.Instance.HTTP_502_Bad_Gateway_Telemetry_False_Positive(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    HTTP_502.Instance.HTTP_502_Bad_Gateway_EXO_DNS_Lookup_False_Positive(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    HTTP_502.Instance.HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    HTTP_502.Instance.HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    HTTP_502.Instance.HTTP_502_Bad_Gateway_Anything_Else(this.session);
                    break;
                case 503:
                    HTTP_503.Instance.HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    HTTP_503.Instance.HTTP_503_Service_Unavailable_Everything_Else(this.session);
                    break;
                case 504:
                    HTTP_504.Instance.HTTP_504_Gateway_Timeout_Internet_Access_Blocked(this.session);
                    if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                    {
                        break;
                    }

                    HTTP_504.Instance.HTTP_504_Gateway_Timeout_Anything_Else(this.session);
                    break;
                case 505:
                    HTTP_505.Instance.HTTP_505_HTTP_Version_Not_Supported(this.session);

                    break;
                case 506:
                    HTTP_506.Instance.HTTP_506_Variant_Also_Negociates(this.session);
                    break;
                case 507:
                    HTTP_507.Instance.HTTP_507_Insufficient_Storage(this.session);
                    break;
                case 508:
                    HTTP_508.Instance.HTTP_508_Loop_Detected(this.session);
                    break;
                case 509:
                    HTTP_509.Instance.HTTP_509_Bandwidth_Limit_Exceeeded(this.session);
                    break;
                case 510:
                    HTTP_510.Instance.HTTP_510_Not_Extended(this.session);
                    break;
                case 511:
                    HTTP_511.Instance.HTTP_511_Network_Authentication_Required(this.session);
                    break;
                case 520:
                    HTTP_520.Instance.HTTP_520_Web_Server_Returned_an_Unknown_Error(this.session);
                    break;
                case 521:
                    HTTP_521.Instance.HTTP_521_Web_Server_Is_Down(this.session);
                    break;
                case 522:
                    HTTP_522.Instance.HTTP_522_Connection_Timed_Out(this.session);
                    break;
                case 523:
                    HTTP_523.Instance.HTTP_523_Origin_Is_Unreachable(this.session);
                    break;
                case 524:
                    HTTP_524.Instance.HTTP_524_A_Timeout_Occurred(this.session);
                    break;
                case 525:
                    HTTP_525.Instance.HTTP_525_SSL_Handshake_Failed(this.session);
                    break;
                case 526:
                    HTTP_526.Instance.HTTP_526_Invalid_SSL_Certificate(this.session);
                    break;
                case 527:
                    HTTP_527.Instance.HTTP_527_Railgun_Error(this.session);
                    break;
                case 529:
                    HTTP_529.Instance.HTTP_529_Site_Is_Overloaded(this.session);
                    break;
                case 530:
                    HTTP_530.Instance.HTTP_530_Site_Is_Frozen(this.session);
                    break;
                case 561:
                    HTTP_561.Instance.HTTP_561_Unauthorized(this.session);
                    break;
                case 598:
                    HTTP_598.Instance.HTTP_598_Network_Read_Timeout_Error(this.session);
                    break;
                default:
                    // Not setting colours on sessions not recognised.

                    var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "Session undefined in extension.",
                        UIBackColour = "Gray",
                        UITextColour = "Black",

                        SessionType = "Undefined",
                        ResponseCodeDescription = "Defaulted. HTTP Response Code undefined.",
                        ResponseAlert = "Undefined",
                        ResponseComments = SessionFlagService.Instance.ResponseCommentsNoKnownIssue(),

                        SessionAuthenticationConfidenceLevel = 0,
                        SessionTypeConfidenceLevel = 0,
                        SessionResponseServerConfidenceLevel = 0
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                    break;
            }
        }
    }
}
