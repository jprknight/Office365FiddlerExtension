﻿using System.Reflection;
using Fiddler;
using Office365FiddlerExtensionRuleset.Ruleset;

namespace Office365FiddlerExtensionRuleset
{
    public class RunRuleSet
    {
        internal Session session { get; set; }

        /// <summary>
        /// 
        /// MAIN
        /// 
        /// This should be considered the main constructor for the extension ruleset DLL. 
        /// 
        /// </summary>
        /// <param name="session"></param>
        public void Initialize(Session session)
        {
            this.session = session;

            // Only want to see this once in the Fiddler log.
            if (this.session.id == 1)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}):" +
                $" Starting v" +
                $"{Assembly.GetExecutingAssembly().GetName().Version.Major}." +
                $"{Assembly.GetExecutingAssembly().GetName().Version.Minor}." +
                $"{Assembly.GetExecutingAssembly().GetName().Version.Build}");
            }

            ///////////////////////////////
            ///
            // Always run these functions on every session.

            // Broad logic checks on sessions regardless of response code.
            FiddlerUpdateSessions.Instance.Run(this.session);
            ApacheAutodiscover.Instance.Run(this.session);
            ConnectTunnelTLSVersion.Instance.Run(this.session);
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

            // Host IP.
            HostIP.Instance.Run(this.session);

            ///////////////////////////////
            ///
            // From here on out only run functions where session analysis isn't completed.
            if (!RulesetUtilities.Instance.SessionAnalysisCompleted(this.session))
            {
                ResponseCodeLogic(this.session);
            }

            ///////////////////////////////
            // AUTHENTICATION
            Authentication.Instance.Run(this.session);

            ///////////////////////////////
            // SESSION TYPE
            SessionType.Instance.Run(this.session);

            ///////////////////////////////
            // RESPONSE SERVER
            ResponseServer.Instance.Run(this.session);

            ///////////////////////////////
            // LONG RUNNING SESSIONS
            LongRunningSessions.Instance.Run(this.session);
        }

        // Function containing switch statement for response code logic.
        // https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
        private void ResponseCodeLogic(Session session)
        {
            this.session = session;

            switch (this.session.responseCode)
            {
                case 0:
                    HTTP_0.Instance.Run(this.session);
                    break;
                case 103:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_103s");
                    break;
                case 200:
                    HTTP_200.Instance.Run(this.session);
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
                    HTTP_302.Instance.Run(this.session);
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
                    HTTP_307.Instance.Run(this.session);
                    break;
                case 308:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_308s");
                    break;
                case 400:
                    HTTP_400.Instance.HTTP_400_Bad_Request(this.session);
                    break;
                case 401:
                    HTTP_401.Instance.Run(this.session);
                    break;
                case 402:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_402s");
                    break;
                case 403:
                    HTTP_403.Instance.Run(this.session);
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
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_429s");
                    break;
                case 430:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_430s");
                    break;
                case 431:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_431s");
                    break;
                case 440:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_440s");
                    break;
                case 449:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_449s");
                    break;
                case 450:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_450s");
                    break;
                case 451:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_451s");
                    break;
                case 456:
                    HTTP_456.Instance.Run(this.session);
                    break;
                case 460:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_460s");
                    break;
                case 463:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_463s");
                    break;
                case 494:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_494s");
                    break;
                case 495:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_495s");
                    break;
                case 496:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_496s");
                    break;
                case 497:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_497s");
                    break;
                case 498:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_498s");
                    break;
                case 499:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_499s");
                    break;
                case 500:
                    HTTP_500.Instance.Run(this.session);
                    break;
                case 501:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_501s");
                    break;
                case 502:
                    HTTP_502.Instance.Run(this.session);
                    break;
                case 503:
                    HTTP_503.Instance.Run(this.session);
                    break;
                case 504:
                    HTTP_504.Instance.Run(this.session);
                    break;
                case 505:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_505s");
                    break;
                case 506:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_506s");
                    break;
                case 507:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_507s");
                    break;
                case 508:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_508s");
                    break;
                case 509:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_509s");
                    break;
                case 510:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_510s");
                    break;
                case 511:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_511s");
                    break;
                case 520:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_520s");
                    break;
                case 521:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_521s");
                    break;
                case 522:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_522s");
                    break;
                case 523:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_523s");
                    break;
                case 524:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_524s");
                    break;
                case 525:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_525s");
                    break;
                case 526:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_526s");
                    break;
                case 527:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_527s");
                    break;
                case 529:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_529s");
                    break;
                case 530:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_530s");
                    break;
                case 561:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_561s");
                    break;
                case 598:
                    SimpleSessionAnalysis.Instance.Run(this.session, "HTTP_598s");
                    break;
                default:
                    Default_UnknownResponseCode.Instance.Run(this.session);
                    break;
            }
        }
    }
}
