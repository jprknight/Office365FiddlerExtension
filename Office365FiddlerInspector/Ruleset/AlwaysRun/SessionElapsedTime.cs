using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class SessionElapsedTime : ActivationService
    {
        private static SessionElapsedTime _instance;

        public static SessionElapsedTime Instance => _instance ?? (_instance = new SessionElapsedTime());

        // Function where Elapsed Time column data is populated.
        public void SetElapsedTime(Session session)
        {
            this.session = session;

            GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "Running SetElapsedTime.");

            // Populate the ElapsedTime column.
            if (this.session.Timers.ClientBeginRequest.ToString("H:mm:ss.fff") != "0:00:00.000" && this.session.Timers.ClientDoneResponse.ToString("H:mm:ss.fff") != "0:00:00.000")
            {
                double Milliseconds = Math.Round((session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);

                GetSetSessionFlags.Instance.SetXElapsedTime(this.session, Milliseconds + "ms");
            }
            else
            {
                GetSetSessionFlags.Instance.SetXElapsedTime(this.session, "No Data");
            }
        }

        // Function to set the Elapsed Time for the inspector. HTML mark up.
        public void SetInspectorElapsedTime(Session session)
        {
            this.session = session;

            GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "Running SetInspectorElapsedTime.");

            // ClientDoneResponse can be blank. If so do not try to calculate and output Elapsed Time, we end up with a hideously large number.
            if (this.session.Timers.ClientDoneResponse.ToString("H:mm:ss.fff") != "0:00:00.000")
            {
                double ClientMilliseconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);
                double ClientSeconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalSeconds);

                // If the roundtrip time is less than 1 second show the result in milliseconds.
                if (ClientMilliseconds == 0)
                {
                    GetSetSessionFlags.Instance.SetXInspectorElapsedTime(this.session, $"{ClientMilliseconds}ms");
                }
                else if (ClientMilliseconds < 1000)
                {
                    GetSetSessionFlags.Instance.SetXInspectorElapsedTime(this.session, $"{ClientMilliseconds}ms");
                }
                // If the roundtrip is over warning and under slow running thresholds; orange.
                else if (ClientMilliseconds > Preferences.GetWarningSessionTimeThreshold() && ClientMilliseconds < Preferences.GetSlowRunningSessionThreshold())
                {
                    GetSetSessionFlags.Instance.SetXInspectorElapsedTime(this.session, $"<b><span style='color:orange'>{ClientSeconds} seconds ({ClientMilliseconds}ms).</span></b>");
                }
                // If roundtrip is over slow running threshold; red.
                else if (ClientMilliseconds > Preferences.GetSlowRunningSessionThreshold())
                {
                    GetSetSessionFlags.Instance.SetXInspectorElapsedTime(this.session, $"<b><span style='color:red'>{ClientSeconds} seconds ({ClientMilliseconds}ms).</span></b>");
                }
                // If the roundtrip time is more than 1 second show the result in seconds.
                else
                {
                    if (ClientSeconds == 1)
                    {
                        GetSetSessionFlags.Instance.SetXInspectorElapsedTime(this.session, $"{ClientSeconds} second({ClientMilliseconds}ms).");
                    }
                    else
                    {
                        GetSetSessionFlags.Instance.SetXInspectorElapsedTime(this.session, $"{ClientSeconds} seconds ({ClientMilliseconds}ms).");
                    }
                }
            }
            else
            {
                GetSetSessionFlags.Instance.SetXInspectorElapsedTime(this.session, "Insufficient data");
            }
        }
    }
}