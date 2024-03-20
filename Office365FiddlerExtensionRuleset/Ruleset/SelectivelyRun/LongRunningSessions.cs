using System;
using Office365FiddlerExtension.Services;
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

        // Function to highlight long running sessions.
        public void LongRunningSessionsWarning(Session session)
        {
            this.session = session;

            double ClientMilliseconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);

            // If the session is less than the warning threshold (quick) and more than the slow running threshold (slow), return.
            if (ClientMilliseconds < SettingsJsonService.Instance.WarningSessionTimeThreshold && ClientMilliseconds > SettingsJsonService.Instance.SlowRunningSessionThreshold)
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running LongRunningSessionsWarning.");

            // Warn on a 2.5 second roundtrip time. Using ClientMilliseconds here since that represents the complete round trip.
            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "LongRunningSesions_Warning",

                SessionType = LangHelper.GetString("LongRunningSessionsWarning_SessionType"),
                //ResponseCodeDescription = "",
                ResponseAlert = LangHelper.GetString("LongRunningSessionsWarning_ResponseAlert"),
                ResponseComments = LangHelper.GetString("LongRunningSessionsWarning_ResponseComments"),

                SessionSeverity = 40
            };
            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        public void LongRunningSessionsClientSlow(Session session) {

            this.session = session;

            double ClientMilliseconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);

            // If the session round trip time is less than the slow session threshold, return.
            if (ClientMilliseconds < SettingsJsonService.Instance.SlowRunningSessionThreshold)
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Long running client session.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "LongRunningSessions_Client_Session",

                SessionType = LangHelper.GetString("LongRunningSessionsClientSlow_SessionType"),
                //ResponseCodeDescription = "",
                ResponseAlert = LangHelper.GetString("LongRunningSessionsClientSlow_ResponseAlert"),
                ResponseComments = LangHelper.GetString("LongRunningSessionsClientSlow_ResponseComments"),

                SessionSeverity = 60
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        public void LongRunningSessionsServerSlow(Session session) {

            this.session = session;

            double ServerMilliseconds = Math.Round((this.session.Timers.ServerBeginResponse - this.session.Timers.ServerGotRequest).TotalMilliseconds);

            // If the Office 365 server think time runs longer than 5,000ms or 5 seconds.
            if (ServerMilliseconds < SettingsJsonService.Instance.SlowRunningSessionThreshold)
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Long running Office 365 session.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "LongRunningSessions_Server_Session",

                SessionType = LangHelper.GetString("LongRunningSessionsServerSlow_SessionType"),
                //ResponseCodeDescription = "",
                ResponseAlert = LangHelper.GetString("LongRunningSessionsServerSlow_ResponseAlert"),
                ResponseComments = LangHelper.GetString("LongRunningSessionsServerSlow_ResponseComments"),

                SessionSeverity = 60
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);           
        }
    }
}
