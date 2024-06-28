using System;
using Office365FiddlerExtensionRuleset.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class SessionElapsedTime
    {
        internal Session session { get; set; }

        private static SessionElapsedTime _instance;

        public static SessionElapsedTime Instance => _instance ?? (_instance = new SessionElapsedTime());

        /// <summary>
        /// Calculate session elapsed time for the UI column and response inspector.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            SetElapsedTime(this.session);
            SetInspectorElapsedTime(this.session);
        }

        /// <summary>
        /// Calculate session elapsed time for the UI column.
        /// </summary>
        /// <param name="session"></param>
        private void SetElapsedTime(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running SetElapsedTime.");

            if (this.session.Timers.ClientBeginRequest.ToString("H:mm:ss.fff") != "0:00:00.000" && this.session.Timers.ClientDoneResponse.ToString("H:mm:ss.fff") != "0:00:00.000")
            {
                double Milliseconds = Math.Round((session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);

                var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = RulesetLangHelper.GetString("Session Elapsed Time"),
                    ElapsedTime = Milliseconds.ToString()
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
            else
            {
                var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "SessionElapsedTime_NoData",
                    ElapsedTime = RulesetLangHelper.GetString("No data")
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
        }

        /// <summary>
        /// Determine the elapsed time for the response inspector.
        /// </summary>
        /// <param name="session"></param>
        private void SetInspectorElapsedTime(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running SetInspectorElapsedTime.");

            // ClientDoneResponse can be blank. If so do not try to calculate and output Elapsed Time, we end up with a hideously large number.
            if (this.session.Timers.ClientDoneResponse.ToString("H:mm:ss.fff") != "0:00:00.000")
            {
                double ClientMilliseconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);
                double ClientSeconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalSeconds);

                // If the roundtrip time is less than 1 second show the result in milliseconds.
                if (ClientMilliseconds < 1000)
                {
                    var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "InspectorElapsedTime_LessThanOneSecond",
                        InspectorElapsedTime = $"{ClientMilliseconds}{RulesetLangHelper.GetString("Milliseconds")}"
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                }
                // If the roundtrip is over warning and under slow running thresholds; orange.
                else if (ClientMilliseconds > RulesetSettingsJsonService.Instance.WarningSessionTimeThreshold && ClientMilliseconds < RulesetSettingsJsonService.Instance.SlowRunningSessionThreshold)
                {
                    var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "InspectorElapsedTime_Warning",
                        InspectorElapsedTime = $"<b><span style='color:orange'>"
                            + $"{ClientSeconds} {RulesetLangHelper.GetString("Seconds")} ({ClientMilliseconds}{RulesetLangHelper.GetString("Milliseconds")}).</span></b>"
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                }
                // If roundtrip is over slow running threshold; red.
                else if (ClientMilliseconds > RulesetSettingsJsonService.Instance.SlowRunningSessionThreshold)
                {
                    var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "InspectorElapsedTime_Slow",
                        InspectorElapsedTime = $"<b><span style='color:red'>"
                            + $"{ClientSeconds} {RulesetLangHelper.GetString("Seconds")} ({ClientMilliseconds}{RulesetLangHelper.GetString("Milliseconds")}).</span></b>"
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                }
                // If the roundtrip time is more than 1 second show the result in seconds.
                else
                {
                    if (ClientSeconds == 1)
                    {
                        var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                        {
                            SectionTitle = "InspectorElapsedTime_MoreThanOneSecond",
                            InspectorElapsedTime = $"{ClientSeconds} {RulesetLangHelper.GetString("Second")} ({ClientMilliseconds}{RulesetLangHelper.GetString("Milliseconds")})."
                        };

                        var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                    }
                    else
                    {
                        var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                        {
                            SectionTitle = "InspectorElapsedTime_Else",
                            InspectorElapsedTime = $"{ClientSeconds} {RulesetLangHelper.GetString("Seconds")} ({ClientMilliseconds}{RulesetLangHelper.GetString("Milliseconds")})."
                        };

                        var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                    }
                }
            }
            else
            {
                var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "InspectorElapsedTime_Insufficient_Data",

                    InspectorElapsedTime = RulesetLangHelper.GetString("Insufficient data")
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
        }
    }
}
