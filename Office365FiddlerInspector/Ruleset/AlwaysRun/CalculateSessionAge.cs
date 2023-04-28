using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class CalculateSessionAge : ActivationService
    {
        private static CalculateSessionAge _instance;

        public static CalculateSessionAge Instance => _instance ?? (_instance = new CalculateSessionAge());

        // Calculate session age on Inspector.
        public void SessionAge(Session session)
        {
            this.session = session;

            GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "Running CalculateSessionAge.");

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

            GetSetSessionFlags.Instance.SetXDateDataCollected(this.session, SessionDateTime.ToString("dddd, MMMM dd, yyyy h:mm tt"));

            if (TimeSpanDays <= 7)
            {
                GetSetSessionFlags.Instance.SetXDataAge(this.session, $"<b><span style='color:green'>{DataAge}</span></b>");

                GetSetSessionFlags.Instance.SetXCalculatedSessionAge(this.session, "<p>Session collected within 7 days, data freshness is good. Best case scenario for correlating this data to backend server logs.</p>");
            }
            else if (TimeSpanDays > 7 && TimeSpanDays < 14)
            {
                GetSetSessionFlags.Instance.SetXDataAge(this.session, $"<b><span style='color:orange'>{DataAge}</span></b>");

                GetSetSessionFlags.Instance.SetXCalculatedSessionAge(this.session, "<p>Session collected within 14 days, data freshness is good, <b><span style='color:orange'>but not ideal</span></b>. "
                    + "Depending on the backend system, <b><span style='color:orange'>correlating this data to server logs might be possible</span></b>.</p>");
            }
            else if (TimeSpanDays >= 14 && TimeSpanDays < 30)
            {
                GetSetSessionFlags.Instance.SetXDataAge(this.session, $"<b><span style='color:orange'>{DataAge}</span></b>");

                GetSetSessionFlags.Instance.SetXCalculatedSessionAge(this.session, "<p><b><span style='color:red'>Session collected between 14 and 30 days ago</span></b>. "
                    + "Correlating with any backend server logs is <b><span style='color:red'>likely impossible</span></b>. Many systems don't keep logs this long.</p>");
            }
            else
            {
                GetSetSessionFlags.Instance.SetXDataAge(this.session, $"<b><span style='color:red'>{DataAge}</span></b>");

                GetSetSessionFlags.Instance.SetXCalculatedSessionAge(this.session, "<p><b><span style='color:red'>Session collected more than 30 days ago</span></b>. "
                    + "Correlating with any backend server logs is <b><span style='color:red'>very likely impossible</span></b>. Many systems don't keep logs this long.</p>");
            }
        }
    }
}