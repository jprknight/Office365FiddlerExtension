using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{

    class SetLongRunningSessions
    {

        // Function to highlight long running sessions.
        public void SetLongRunningSessionsData(Session session)
        {
            // LongRunningSessions
            // Code section for response code logic overrides (long running sessions).

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " Running SetLongRunningSessions.");

            double ClientMilliseconds = Math.Round((session.Timers.ClientDoneResponse - session.Timers.ClientBeginRequest).TotalMilliseconds);

            double ServerMilliseconds = Math.Round((session.Timers.ServerBeginResponse - session.Timers.ServerGotRequest).TotalMilliseconds);

            // Warn on a 2.5 second roundtrip time.
            if (ClientMilliseconds > Preferences.GetWarningSessionTimeThreshold() && ClientMilliseconds < Preferences.GetSlowRunningSessionThreshold())
            {
                if (session["X-SessionType"] == null)
                {
                    session["ui-backcolor"] = Preferences.HTMLColourOrange;
                    session["ui-color"] = "black";

                    session["X-SessionType"] = "Roundtrip Time Warning";

                    session["X-ResponseAlert"] = "<b><span style='color:orange'>Roundtrip Time Warning</span></b>";
                }

                session["X-ResponseComments"] += "This session took more than 2.5 seconds to complete. "
                    + "A small number of sessions completing roundtrip in this timeframe is not necessary sign of an issue.";
            }
            // If the overall session time runs longer than 5,000ms or 5 seconds.
            else if (ClientMilliseconds > Preferences.GetSlowRunningSessionThreshold())
            {
                if (session["X-SessionType"] == null)
                {
                    session["ui-backcolor"] = Preferences.HTMLColourRed;
                    session["ui-color"] = "black";

                    session["X-SessionType"] = "Long Running Client Session";

                    session["X-ResponseAlert"] = "<b><span style='color:red'>Long Running Client Session</span></b>";
                }

                session["X-ResponseComments"] += "<p><b><span style='color:red'>Long running session found</span></b>. A small number of long running sessions in the < 10 "
                    + "seconds time frame have been seen on normal working scenarios. This does not necessary signify an issue.</p>"
                    + "<p>If, however, you are troubleshooting an application performance issue, consider the number of sessions which "
                    + "have this warning. Investigate any proxy device or load balancer in your network, "
                    + "or any other device sitting between the client computer and access to the application server the data resides on.</p>"
                    + "<p>Try the divide and conquer approach. What can you remove or bypass from the equation to see if the application then performs "
                    + "normally?</p>";

                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " Long running client session.");
            }
            // If the Office 365 server think time runs longer than 5,000ms or 5 seconds.
            else if (ServerMilliseconds > Preferences.GetSlowRunningSessionThreshold())
            {
                if (session["X-SessionType"] == null)
                {
                    session["ui-backcolor"] = Preferences.HTMLColourRed;
                    session["ui-color"] = "black";

                    session["X-SessionType"] = "Long Running Server Session";

                    session["X-ResponseAlert"] = "<b><span style='color:red'>Long Running Server Session</span></b>";
                }

                session["X-ResponseComments"] += "Long running Server session found. A small number of long running sessions in the < 10 "
                    + "seconds time frame have been seen on normal working scenarios. This does not necessary signify an issue."
                    + "<p>If, however, you are troubleshooting an application performance issue, consider the number of sessions which "
                    + "have this warning alongany proxy device in your network, "
                    + "or any other device sitting between the client computer and access to the internet."
                    + "Try the divide and conquer approach. What can you remove or bypass from the equation to see if the application then performs "
                    + "normally?</p>";

                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " Long running Office 365 session.");
            }
        }
    }
}