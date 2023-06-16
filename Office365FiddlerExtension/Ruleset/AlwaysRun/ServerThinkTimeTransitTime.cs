using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Office365FiddlerExtension.Handler;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtension.Ruleset
{
    class ServerThinkTimeTransitTime : ActivationService
    {
        private static ServerThinkTimeTransitTime _instance;

        public static ServerThinkTimeTransitTime Instance => _instance ?? (_instance = new ServerThinkTimeTransitTime());

        // Set Server Think Time and Transit Time for Inspector.
        public void SetServerThinkTimeTransitTime(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} Running SetServerThinkTimeTransitTime.");

            // ServerGotRequest, ServerBeginResponse or ServerDoneResponse can be blank. If so do not try to calculate and output 'Server Think Time' or
            // 'Transmit Time', we end up with a hideously large number.
            if (this.Session.Timers.ServerGotRequest.ToString("H:mm:ss.fff") != "0:00:00.000" &&
                this.Session.Timers.ServerBeginResponse.ToString("H:mm:ss.fff") != "0:00:00.000" &&
                this.Session.Timers.ServerDoneResponse.ToString("H:mm:ss.fff") != "0:00:00.000")
            {

                double ServerMilliseconds = Math.Round((this.Session.Timers.ServerBeginResponse - this.Session.Timers.ServerGotRequest).TotalMilliseconds);
                double ServerSeconds = Math.Round((this.Session.Timers.ServerBeginResponse - this.Session.Timers.ServerGotRequest).TotalSeconds);

                // transit time = elapsed time - server think time.

                double ElapsedMilliseconds = Math.Round((this.Session.Timers.ClientDoneResponse - this.Session.Timers.ClientBeginRequest).TotalMilliseconds);

                double dTransitTimeMilliseconds = ElapsedMilliseconds - ServerMilliseconds;
                if (dTransitTimeMilliseconds < 0)
                {
                    dTransitTimeMilliseconds = 0;
                }

                int iTransitTimeSeconds = (int)Math.Round(dTransitTimeMilliseconds / 1000);

                // If 1/10th of the session elapsed time is more than the server think time, network roundtrip loses.
                if (ElapsedMilliseconds / 10 > ServerMilliseconds && ElapsedMilliseconds > SettingsHandler.Instance.SlowRunningSessionThreshold)
                {
                    var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                    {
                        SessionTimersDescription = "<p>The server think time for this session was less than 1/10th of the elapsed time. This indicates network latency in this session.</p>"
                        + "<p>If you are troubleshooting application latency, the next step is to collect network traces (Wireshark, NetMon etc) and troubleshoot at the network layer.</p>"
                        + "<p>Ideally collect concurrent network traces on the impacted client and a network perimeter device, to be analysed together by a member of your networking team.<p>"
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);

                    // Highlight server think time in green.
                    if (ServerMilliseconds < 1000)
                    {
                        sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                        {
                            ServerThinkTime = $"<b><span style='color:green'>{ServerMilliseconds}ms.</span></b>"
                        };

                        sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
                    }
                    else if (ServerMilliseconds >= 1000 && ServerMilliseconds < 2000)
                    {
                        sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                        {
                            ServerThinkTime = $"<b><span style='color:green'>{ServerSeconds} second ({ServerMilliseconds}ms).</span></b>"
                        };

                        sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
                    }
                    else
                    {
                        sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                        {
                            ServerThinkTime = $"<b><span style='color:green'>{ServerSeconds} seconds ({ServerMilliseconds}ms).</span></b>"
                        };

                        sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
                    }

                    // Highlight transit time in red.
                    if (dTransitTimeMilliseconds < 1000)
                    {
                        sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                        {
                            TransitTime = $"<b><span style='color:red'>{dTransitTimeMilliseconds}ms.</span></b>"
                        };

                        sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
                    }
                    else if (dTransitTimeMilliseconds >= 1000 && dTransitTimeMilliseconds < 2000)
                    {
                        sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                        {
                            TransitTime = $"<b><span style='color:red'>{iTransitTimeSeconds} second ({dTransitTimeMilliseconds} ms).</span></b>"
                        };

                        sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
                    }
                    else
                    {
                        sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                        {
                            TransitTime = $"<b><span style='color:red'>{iTransitTimeSeconds} seconds ({dTransitTimeMilliseconds} ms).</span></b>"
                        };

                        sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
                    }
                }
                else
                {
                    if (ServerMilliseconds < 1000)
                    {
                        var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                        {
                            ServerThinkTime = $"{ServerMilliseconds}ms"
                        };

                        var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
                    }
                    else if (ServerMilliseconds >= 1000 && ServerMilliseconds < 2000)
                    {
                        var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                        {
                            ServerThinkTime = $"{ServerSeconds} second ({ServerMilliseconds}ms)."
                        };

                        var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
                    }
                    else
                    {
                        var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                        {
                            ServerThinkTime = $"{ServerSeconds} seconds ({ServerMilliseconds}ms)."
                        };

                        var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
                    }

                    if (dTransitTimeMilliseconds < 1000)
                    {
                        var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                        {
                            TransitTime = $"{dTransitTimeMilliseconds}ms"
                        };

                        var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
                    }
                    else if (dTransitTimeMilliseconds >= 1000 && dTransitTimeMilliseconds < 2000)
                    {
                        var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                        {
                            TransitTime = $"{iTransitTimeSeconds} second ({dTransitTimeMilliseconds} ms)."
                        };

                        var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
                    }
                    else
                    {
                        var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                        {
                            TransitTime = $"{iTransitTimeSeconds} seconds ({dTransitTimeMilliseconds} ms)."
                        };

                        var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
                    }
                }
            }
            else
            {
                var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                {
                    ServerThinkTime = "Insufficient data",
                    TransitTime = "Insufficient data"
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
            }
        }
    }
}