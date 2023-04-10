using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class ServerThinkTimeTransitTime : ActivationService
    {
        private static ServerThinkTimeTransitTime _instance;

        public static ServerThinkTimeTransitTime Instance => _instance ?? (_instance = new ServerThinkTimeTransitTime());

        // Function to set Server Think Time and Transit Time for use within Inspector.
        public void SetServerThinkTimeTransitTime(Session session)
        {
            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " Running SetServerThinkTimeTransitTime.");

            // ServerGotRequest, ServerBeginResponse or ServerDoneResponse can be blank. If so do not try to calculate and output 'Server Think Time' or 'Transmit Time', we end up with a hideously large number.
            if (session.Timers.ServerGotRequest.ToString("H:mm:ss.fff") != "0:00:00.000" &&
                session.Timers.ServerBeginResponse.ToString("H:mm:ss.fff") != "0:00:00.000" &&
                session.Timers.ServerDoneResponse.ToString("H:mm:ss.fff") != "0:00:00.000")
            {

                double ServerMilliseconds = Math.Round((session.Timers.ServerBeginResponse - session.Timers.ServerGotRequest).TotalMilliseconds);
                double ServerSeconds = Math.Round((session.Timers.ServerBeginResponse - session.Timers.ServerGotRequest).TotalSeconds);

                // transit time = elapsed time - server think time.

                double ElapsedMilliseconds = Math.Round((session.Timers.ClientDoneResponse - session.Timers.ClientBeginRequest).TotalMilliseconds);

                double dTransitTimeMilliseconds = ElapsedMilliseconds - ServerMilliseconds;
                if (dTransitTimeMilliseconds < 0)
                {
                    dTransitTimeMilliseconds = 0;
                }

                int iTransitTimeSeconds = (int)Math.Round(dTransitTimeMilliseconds / 1000);

                // If 1/10th of the session elapsed time is more than the server think time, network roundtrip loses.
                if (ElapsedMilliseconds / 10 > ServerMilliseconds && ElapsedMilliseconds > Preferences.GetSlowRunningSessionThreshold())
                {
                    session["X-SessionTimersDescription"] = "<p>The server think time for this session was less than 1/10th of the elapsed time. This indicates network latency in this session.</p>" +
                        "<p>If you are troubleshooting application latency, the next step is to collect network traces (Wireshark, NetMon etc) and troubleshoot at the network layer.</p>" +
                        "<p>Ideally collect concurrent network traces on the impacted client and a network perimeter device, to be analysed together by a member of your networking team.<p>";



                    // Highlight server think time in green.
                    if (ServerMilliseconds < 1000)
                    {
                        session["X-ServerThinkTime"] = $"<b><span style='color:green'>{ServerMilliseconds}ms.</span></b>";
                    }
                    else if (ServerMilliseconds >= 1000 && ServerMilliseconds < 2000)
                    {
                        session["X-ServerThinkTime"] = $"<b><span style='color:green'>{ServerSeconds} second ({ServerMilliseconds}ms).</span></b>";
                    }
                    else
                    {
                        session["X-ServerThinkTime"] = $"<b><span style='color:green'>{ServerSeconds} seconds ({ServerMilliseconds}ms).</span></b>";
                    }

                    // Highlight transit time in red.
                    if (dTransitTimeMilliseconds < 1000)
                    {
                        session["X-TransitTime"] = $"<b><span style='color:red'>{dTransitTimeMilliseconds}ms.</span></b>";
                    }
                    else if (dTransitTimeMilliseconds >= 1000 && dTransitTimeMilliseconds < 2000)
                    {
                        session["X-TransitTime"] = $"<b><span style='color:red'>{iTransitTimeSeconds} second ({dTransitTimeMilliseconds} ms).</span></b>";
                    }
                    else
                    {
                        session["X-TransitTime"] = $"<b><span style='color:red'>{iTransitTimeSeconds} seconds ({dTransitTimeMilliseconds} ms).</span></b>";
                    }
                }
                else
                {
                    if (ServerMilliseconds < 1000)
                    {
                        session["X-ServerThinkTime"] = $"{ServerMilliseconds}ms";
                    }
                    else if (ServerMilliseconds >= 1000 && ServerMilliseconds < 2000)
                    {
                        session["X-ServerThinkTime"] = $"{ServerSeconds} second ({ServerMilliseconds}ms).";
                    }
                    else
                    {
                        session["X-ServerThinkTime"] = $"{ServerSeconds} seconds ({ServerMilliseconds}ms).";
                    }

                    if (dTransitTimeMilliseconds < 1000)
                    {
                        session["X-TransitTime"] = $"{dTransitTimeMilliseconds}ms";
                    }
                    else if (dTransitTimeMilliseconds >= 1000 && dTransitTimeMilliseconds < 2000)
                    {
                        session["X-TransitTime"] = $"{iTransitTimeSeconds} second ({dTransitTimeMilliseconds} ms).";
                    }
                    else
                    {
                        session["X-TransitTime"] = $"{iTransitTimeSeconds} seconds ({dTransitTimeMilliseconds} ms).";
                    }
                }
            }
            else
            {
                session["X-ServerThinkTime"] = "Insufficient data";
                session["X-TransitTime"] = "Insufficient data";
            }
        }
    }
}
