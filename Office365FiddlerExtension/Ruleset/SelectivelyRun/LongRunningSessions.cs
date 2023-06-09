using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using static Office365FiddlerExtension.Services.SessionFlagHandler;

namespace Office365FiddlerExtension.Ruleset
{
    class LongRunningSessions : ActivationService
    {
        private static LongRunningSessions _instance;

        public static LongRunningSessions Instance => _instance ?? (_instance = new LongRunningSessions());

        // Function to highlight long running sessions.
        public void LongRunningSessionsWarning(Session session)
        {
            this.session = session;

            double ClientMilliseconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);

            // If the session is less than the warning threshold (quick) and more than the slow running threshold (slow), return.
            if (ClientMilliseconds < SettingsHandler.Instance.WarningSessionTimeThreshold && ClientMilliseconds > SettingsHandler.Instance.SlowRunningSessionThreshold)
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} Running LongRunningSessionsWarning.");

            // Warn on a 2.5 second roundtrip time. Using ClientMilliseconds here since that represents the complete round trip.
            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "LongRunningSesions_Warning",
                UIBackColour = "Orange",
                UITextColour = "Black",

                SessionType = "Roundtrip Time Warning",
                ResponseCodeDescription = "",
                ResponseAlert = "<b><span style='color:orange'>Roundtrip Time Warning</span></b>",
                ResponseComments = "This session took more than 2.5 seconds to complete. "
                + "A small number of sessions completing roundtrip in this timeframe is not necessary sign of an issue."
            };
            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        public void LongRunningSessionsClientSlow(Session session) {

            this.session = session;

            double ClientMilliseconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);

            // If the session round trip time is less than the slow session threshold, return.
            if (ClientMilliseconds < SettingsHandler.Instance.SlowRunningSessionThreshold)
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} Long running client session.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "LongRunningSessions_Client_Session",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "Long Running Client Session",
                ResponseCodeDescription = "",
                ResponseAlert = "<b><span style='color:red'>Long Running Client Session</span></b>",
                ResponseComments = "<p><b><span style='color:red'>Long running session found</span></b>. A small number of long running sessions in the < 10 "
                + "seconds time frame have been seen on normal working scenarios. This does not necessary signify an issue.</p>"
                + "<p>If, however, you are troubleshooting an application performance issue, consider the number of sessions which "
                + "have this warning. Investigate any proxy device or load balancer in your network, "
                + "or any other device sitting between the client computer and access to the application server the data resides on.</p>"
                + "<p>Try the divide and conquer approach. What can you remove or bypass from the equation to see if the application then performs "
                + "normally?</p>"
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        public void LongRunningSessionsServerSlow(Session session) {

            this.session = session;

            double ServerMilliseconds = Math.Round((this.session.Timers.ServerBeginResponse - this.session.Timers.ServerGotRequest).TotalMilliseconds);

            // If the Office 365 server think time runs longer than 5,000ms or 5 seconds.
            if (ServerMilliseconds < SettingsHandler.Instance.SlowRunningSessionThreshold)
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} Long running Office 365 session.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "LongRunningSessions_Server_Session",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "Long Running Server Session",
                ResponseCodeDescription = "",
                ResponseAlert = "<b><span style='color:red'>Long Running Server Session</span></b>",
                ResponseComments = "Long running Server session found. A small number of long running sessions in the < 10 "
                + "seconds time frame have been seen on normal working scenarios. This does not necessary signify an issue."
                + "<p>If, however, you are troubleshooting an application performance issue, consider the number of sessions which "
                + "have this warning alongany proxy device in your network, "
                + "or any other device sitting between the client computer and access to the internet."
                + "Try the divide and conquer approach. What can you remove or bypass from the equation to see if the application then performs "
                + "normally?</p>"
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);           
        }
    }
}