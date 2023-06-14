using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;

namespace Office365FiddlerExtension.Ruleset
{
    class CalculateSessionAge : ActivationService
    {
        private static CalculateSessionAge _instance;

        public static CalculateSessionAge Instance => _instance ?? (_instance = new CalculateSessionAge());

        // Calculate session age on Inspector.
        public void SessionAge(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} Running CalculateSessionAge.");

            String TimeSpanDaysText;
            String TimeSpanHoursText;
            String TimeSpanMinutesText;

            DateTime SessionDateTime = session.Timers.ClientBeginRequest;
            DateTime DateTimeNow = DateTime.Now;
            TimeSpan CalcDataAge = DateTimeNow - SessionDateTime;
            int TimeSpanDays = CalcDataAge.Days;
            int TimeSpanHours = CalcDataAge.Hours;
            int TimeSpanMinutes = CalcDataAge.Minutes;

            if (TimeSpanDays == 1)
            {
                TimeSpanDaysText = TimeSpanDays + " day, ";
            }
            else
            {
                TimeSpanDaysText = TimeSpanDays + " days, ";
            }

            if (TimeSpanHours == 1)
            {
                TimeSpanHoursText = TimeSpanHours + " hour, ";
            }
            else
            {
                TimeSpanHoursText = TimeSpanHours + " hours, ";
            }

            if (TimeSpanMinutes == 1)
            {
                TimeSpanMinutesText = TimeSpanMinutes + " minute ago.";
            }
            else
            {
                TimeSpanMinutesText = TimeSpanMinutes + " minutes ago.";
            }

            String DataAge = TimeSpanDaysText + TimeSpanHoursText + TimeSpanMinutesText;

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                DateDataCollected = SessionDateTime.ToString("dddd, MMMM dd, yyyy h:mm tt")
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);

            if (TimeSpanDays <= 7)
            {
                sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                {
                    DataAge = $"<b><span style='color:green'>{DataAge}</span></b>",
                    CalculatedSessionAge = "<p>Session collected within 7 days, data freshness is good. Best case scenario for correlating this data to backend server logs.</p>"
                };

                sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
            }
            else if (TimeSpanDays > 7 && TimeSpanDays < 14)
            {
                sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                {
                    DataAge = $"<b><span style='color:orange'>{DataAge}</span></b>",
                    CalculatedSessionAge = "<p>Session collected within 14 days, data freshness is good, <b><span style='color:orange'>but not ideal</span></b>. "
                    + "Depending on the backend system, <b><span style='color:orange'>correlating this data to server logs might be possible</span></b>.</p>"
                };

                sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
            }
            else if (TimeSpanDays >= 14 && TimeSpanDays < 30)
            {
                sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                {
                    DataAge = $"<b><span style='color:orange'>{DataAge}</span></b>",
                    CalculatedSessionAge = "<p><b><span style='color:red'>Session collected between 14 and 30 days ago</span></b>. "
                    + "Correlating with any backend server logs is <b><span style='color:red'>likely impossible</span></b>. Many systems don't keep logs this long.</p>"
                };

                sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
            }
            else
            {
                sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                {
                    DataAge = $"<b><span style='color:red'>{DataAge}</span></b>",
                    CalculatedSessionAge = "<p><b><span style='color:red'>Session collected more than 30 days ago</span></b>. "
                    + "Correlating with any backend server logs is <b><span style='color:red'>very likely impossible</span></b>. Many systems don't keep logs this long.</p>"
                };

                sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
            }
        }
    }
}