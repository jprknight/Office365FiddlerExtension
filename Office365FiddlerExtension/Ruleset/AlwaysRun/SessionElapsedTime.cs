﻿using System;
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
    class SessionElapsedTime : ActivationService
    {
        private static SessionElapsedTime _instance;

        public static SessionElapsedTime Instance => _instance ?? (_instance = new SessionElapsedTime());

        // Function where Elapsed Time column data is populated.
        public void SetElapsedTime(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} Running SetElapsedTime.");

            if (this.Session.Timers.ClientBeginRequest.ToString("H:mm:ss.fff") != "0:00:00.000" && this.Session.Timers.ClientDoneResponse.ToString("H:mm:ss.fff") != "0:00:00.000")
            {
                double Milliseconds = Math.Round((session.Timers.ClientDoneResponse - this.Session.Timers.ClientBeginRequest).TotalMilliseconds);

                var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                {
                    SectionTitle = "SessionElapsedTime",
                    ElapsedTime = Milliseconds + "ms"
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
            }
            else
            {
                var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                {
                    SectionTitle = "SessionElapsedTime_NoData",
                    ElapsedTime = "No data"
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
            }
        }

        // Function to set the Elapsed Time for the inspector. HTML mark up.
        public void SetInspectorElapsedTime(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} Running SetInspectorElapsedTime.");

            // ClientDoneResponse can be blank. If so do not try to calculate and output Elapsed Time, we end up with a hideously large number.
            if (this.Session.Timers.ClientDoneResponse.ToString("H:mm:ss.fff") != "0:00:00.000")
            {
                double ClientMilliseconds = Math.Round((this.Session.Timers.ClientDoneResponse - this.Session.Timers.ClientBeginRequest).TotalMilliseconds);
                double ClientSeconds = Math.Round((this.Session.Timers.ClientDoneResponse - this.Session.Timers.ClientBeginRequest).TotalSeconds);

                // If the roundtrip time is less than 1 second show the result in milliseconds.
                if (ClientMilliseconds < 1000)
                {
                    var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                    {
                        SectionTitle = "InspectorElapsedTime_LessThanOneSecond",
                        InspectorElapsedTime = $"{ClientMilliseconds}ms"
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
                }
                // If the roundtrip is over warning and under slow running thresholds; orange.
                else if (ClientMilliseconds > SettingsHandler.Instance.WarningSessionTimeThreshold && ClientMilliseconds < SettingsHandler.Instance.SlowRunningSessionThreshold)
                {
                    var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                    {
                        SectionTitle = "InspectorElapsedTime_Warning",
                        InspectorElapsedTime = $"<b><span style='color:orange'>{ClientSeconds} seconds ({ClientMilliseconds}ms).</span></b>"
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
                }
                // If roundtrip is over slow running threshold; red.
                else if (ClientMilliseconds > SettingsHandler.Instance.SlowRunningSessionThreshold)
                {
                    var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                    {
                        SectionTitle = "InspectorElapsedTime_Slow",
                        InspectorElapsedTime = $"<b><span style='color:red'>{ClientSeconds} seconds ({ClientMilliseconds}ms).</span></b>"
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
                }
                // If the roundtrip time is more than 1 second show the result in seconds.
                else
                {
                    if (ClientSeconds == 1)
                    {
                        var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                        {
                            SectionTitle = "InspectorElapsedTime_MoreThanOneSecond",
                            InspectorElapsedTime = $"{ClientSeconds} second({ClientMilliseconds}ms)."
                        };

                        var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
                    }
                    else
                    {
                        var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                        {
                            SectionTitle = "InspectorElapsedTime_Else",
                            InspectorElapsedTime = $"{ClientSeconds} seconds ({ClientMilliseconds}ms)."
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
                    SectionTitle = "InspectorElapsedTime_Insufficient_Data",
                    InspectorElapsedTime = "Insufficient data"
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
            }
        }
    }
}