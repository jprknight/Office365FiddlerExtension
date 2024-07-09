using System;
using Office365FiddlerExtensionRuleset.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class LongRunningSessions
    {
        internal Session session { get; set; }

        private static LongRunningSessions _instance;

        public static LongRunningSessions Instance => _instance ?? (_instance = new LongRunningSessions());

        /// <summary>
        /// Set long running sessions. Always run last as typically any other session analysis is more useful.
        /// Network captures rather than application data are better used when network retransmits are suspected as
        /// an underlying cause for a given issue. Used in the UI columns and response inspector.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            // Start by checking for Session Type Confidence of 10 and returning if already there.
            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            LongRunningSessionsWarning(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            LongRunningSessionsClientSlow(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            LongRunningSessionsServerSlow(this.session);
        }

        /// <summary>
        /// Set session flags to highlight long running sessions warning.
        /// </summary>
        /// <param name="session"></param>
        private void LongRunningSessionsWarning(Session session)
        {
            this.session = session;

            double ClientMilliseconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);

            // If the session is less than the warning threshold (quick) and more than the slow running threshold (slow), return.
            if (ClientMilliseconds < RulesetSettingsJsonService.Instance.WarningSessionTimeThreshold && ClientMilliseconds > 
                RulesetSettingsJsonService.Instance.SlowRunningSessionThreshold)
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running LongRunningSessionsWarning.");

            // Warn on a 2.5 second roundtrip time. Using ClientMilliseconds here since that represents the complete round trip.
            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "LongRunningSesions_Warning",

                SessionType = RulesetLangHelper.GetString("LongRunningSessionsWarning_SessionType"),
                ResponseAlert = RulesetLangHelper.GetString("LongRunningSessionsWarning_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("LongRunningSessionsWarning_ResponseComments"),

                SessionSeverity = 40
            };
            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set session flags to highlight long running sessions client slow. 
        /// </summary>
        /// <param name="session"></param>
        private void LongRunningSessionsClientSlow(Session session) {

            this.session = session;

            double ClientMilliseconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);

            // If the session round trip time is less than the slow session threshold, return.
            if (ClientMilliseconds < RulesetSettingsJsonService.Instance.SlowRunningSessionThreshold)
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Long running client session.");

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "LongRunningSessions_Client_Session",

                SessionType = RulesetLangHelper.GetString("LongRunningSessionsClientSlow_SessionType"),
                ResponseAlert = RulesetLangHelper.GetString("LongRunningSessionsClientSlow_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("LongRunningSessionsClientSlow_ResponseComments"),

                SessionSeverity = 60
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set session flags to highlight long running sessions server slow.
        /// </summary>
        /// <param name="session"></param>
        private void LongRunningSessionsServerSlow(Session session) {

            this.session = session;

            double ServerMilliseconds = Math.Round((this.session.Timers.ServerBeginResponse - this.session.Timers.ServerGotRequest).TotalMilliseconds);

            // If the Office 365 server think time runs longer than 5,000ms or 5 seconds.
            if (ServerMilliseconds < RulesetSettingsJsonService.Instance.SlowRunningSessionThreshold)
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Long running Office 365 session.");

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "LongRunningSessions_Server_Session",

                SessionType = RulesetLangHelper.GetString("LongRunningSessionsServerSlow_SessionType"),
                ResponseAlert = RulesetLangHelper.GetString("LongRunningSessionsServerSlow_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("LongRunningSessionsServerSlow_ResponseComments"),

                SessionSeverity = 60
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);           
        }
    }
}
