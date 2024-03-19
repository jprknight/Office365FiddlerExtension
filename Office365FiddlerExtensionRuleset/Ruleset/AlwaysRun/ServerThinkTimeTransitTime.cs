using System;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class ServerThinkTimeTransitTime
    {
        internal Session session { get; set; }

        private static ServerThinkTimeTransitTime _instance;

        public static ServerThinkTimeTransitTime Instance => _instance ?? (_instance = new ServerThinkTimeTransitTime());

        // Set Server Think Time and Transit Time for Inspector.
        public void SetServerThinkTimeTransitTime(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running SetServerThinkTimeTransitTime.");

            // ServerGotRequest, ServerBeginResponse or ServerDoneResponse can be blank. If so do not try to calculate and output 'Server Think Time' or
            // 'Transmit Time', we end up with a hideously large number.
            if (this.session.Timers.ServerGotRequest.ToString("H:mm:ss.fff") != "0:00:00.000" &&
                this.session.Timers.ServerBeginResponse.ToString("H:mm:ss.fff") != "0:00:00.000" &&
                this.session.Timers.ServerDoneResponse.ToString("H:mm:ss.fff") != "0:00:00.000")
            {

                double ServerMilliseconds = Math.Round((this.session.Timers.ServerBeginResponse - this.session.Timers.ServerGotRequest).TotalMilliseconds);
                double ServerSeconds = Math.Round((this.session.Timers.ServerBeginResponse - this.session.Timers.ServerGotRequest).TotalSeconds);

                // transit time = elapsed time - server think time.

                double ElapsedMilliseconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);

                double dTransitTimeMilliseconds = ElapsedMilliseconds - ServerMilliseconds;
                if (dTransitTimeMilliseconds < 0)
                {
                    dTransitTimeMilliseconds = 0;
                }

                int iTransitTimeSeconds = (int)Math.Round(dTransitTimeMilliseconds / 1000);

                // If 1/10th of the session elapsed time is more than the server think time, network roundtrip loses.
                if (ElapsedMilliseconds / 10 > ServerMilliseconds && ElapsedMilliseconds > SettingsJsonService.Instance.SlowRunningSessionThreshold)
                {
                    var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                    {
                        SessionTimersDescription = LangHelper.GetString("SessionTimersDescription")
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);

                    // Highlight server think time in green.
                    if (ServerMilliseconds < 1000)
                    {
                        sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                        {
                            ServerThinkTime = $"<b><span style='color:green'>{ServerMilliseconds}ms.</span></b>"
                        };

                        sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                    }
                    else if (ServerMilliseconds >= 1000 && ServerMilliseconds < 2000)
                    {
                        sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                        {
                            ServerThinkTime = $"<b><span style='color:green'>"
                                + $"{ServerSeconds} {LangHelper.GetString("Second")} ({ServerMilliseconds}{LangHelper.GetString("Milliseconds")}).</span></b>"
                        };

                        sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                    }
                    else
                    {
                        sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                        {
                            ServerThinkTime = $"<b><span style='color:green'>"
                                + $"{ServerSeconds} {LangHelper.GetString("Seconds")} ({ServerMilliseconds}{LangHelper.GetString("Milliseconds")}).</span></b>"
                        };

                        sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                    }

                    // Highlight transit time in red.
                    if (dTransitTimeMilliseconds < 1000)
                    {
                        sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                        {
                            TransitTime = $"<b><span style='color:red'>{dTransitTimeMilliseconds}{LangHelper.GetString("Milliseconds")}.</span></b>"
                        };

                        sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                    }
                    else if (dTransitTimeMilliseconds >= 1000 && dTransitTimeMilliseconds < 2000)
                    {
                        sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                        {
                            TransitTime = $"<b><span style='color:red'>"
                                + $"{iTransitTimeSeconds} {LangHelper.GetString("Second")} ({dTransitTimeMilliseconds}{LangHelper.GetString("Milliseconds")}).</span></b>"
                        };

                        sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                    }
                    else
                    {
                        sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                        {
                            TransitTime = $"<b><span style='color:red'>"
                                + $"{iTransitTimeSeconds} {LangHelper.GetString("Seconds")} ({dTransitTimeMilliseconds}{LangHelper.GetString("Milliseconds")}).</span></b>"
                        };

                        sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                    }
                }
                else
                {
                    if (ServerMilliseconds < 1000)
                    {
                        var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                        {
                            ServerThinkTime = $"{ServerMilliseconds}{LangHelper.GetString("Milliseconds")}"
                        };

                        var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                    }
                    else if (ServerMilliseconds >= 1000 && ServerMilliseconds < 2000)
                    {
                        var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                        {
                            ServerThinkTime = $"{ServerSeconds} {LangHelper.GetString("Second")} ({ServerMilliseconds}{LangHelper.GetString("Milliseconds")})."
                        };

                        var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                    }
                    else
                    {
                        var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                        {
                            ServerThinkTime = $"{ServerSeconds} {LangHelper.GetString("Seconds")} ({ServerMilliseconds}{LangHelper.GetString("Milliseconds")})."
                        };

                        var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                    }

                    if (dTransitTimeMilliseconds < 1000)
                    {
                        var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                        {
                            TransitTime = $"{dTransitTimeMilliseconds}{LangHelper.GetString("Milliseconds")}"
                        };

                        var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                    }
                    else if (dTransitTimeMilliseconds >= 1000 && dTransitTimeMilliseconds < 2000)
                    {
                        var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                        {
                            TransitTime = $"{iTransitTimeSeconds} {LangHelper.GetString("Second")} ({dTransitTimeMilliseconds}{LangHelper.GetString("Milliseconds")})."
                        };

                        var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                    }
                    else
                    {
                        var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                        {
                            TransitTime = $"{iTransitTimeSeconds} {LangHelper.GetString("Seconds")} ({dTransitTimeMilliseconds}{LangHelper.GetString("Milliseconds")})."
                        };

                        var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                    }
                }
            }
            else
            {
                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    ServerThinkTime = LangHelper.GetString("Insufficient data"),
                    TransitTime = LangHelper.GetString("Insufficient data"),
                    SessionTimesInsufficientData = true
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
        }
    }
}