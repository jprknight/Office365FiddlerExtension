using System;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class CalculateSessionAge
    {
        internal Session session { get; set; }

        private static CalculateSessionAge _instance;

        public static CalculateSessionAge Instance => _instance ?? (_instance = new CalculateSessionAge());

        // Calculate session age on Inspector.
        public void SessionAge(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Running CalculateSessionAge.");

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
                TimeSpanDaysText = TimeSpanDays + $" {LangHelper.GetString("day")}, ";
            }
            else
            {
                TimeSpanDaysText = TimeSpanDays + $" {LangHelper.GetString("days")}, ";
            }

            if (TimeSpanHours == 1)
            {
                TimeSpanHoursText = TimeSpanHours + $" {LangHelper.GetString("hour")}, ";
            }
            else
            {
                TimeSpanHoursText = TimeSpanHours + $" {LangHelper.GetString("hours")}, ";
            }

            if (TimeSpanMinutes == 1)
            {
                TimeSpanMinutesText = TimeSpanMinutes + $" {LangHelper.GetString("minute ago")}.";
            }
            else
            {
                TimeSpanMinutesText = TimeSpanMinutes + $" {LangHelper.GetString("minutes ago")}.";
            }

            String DataAge = TimeSpanDaysText + TimeSpanHoursText + TimeSpanMinutesText;

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                DateDataCollected = SessionDateTime.ToString("dddd, MMMM dd, yyyy h:mm tt")
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);

            if (TimeSpanDays <= 7)
            {
                sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    DataAge = $"<b><span style='color:green'>{DataAge}</span></b>",
                    CalculatedSessionAge = $"<p>{LangHelper.GetString("Session collected within 7 days")}</p>"
                };

                sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
            else if (TimeSpanDays > 7 && TimeSpanDays < 14)
            {
                sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    DataAge = $"<b><span style='color:orange'>{DataAge}</span></b>",
                    CalculatedSessionAge = LangHelper.GetString("Session collected within 14 days")
                };

                sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
            else if (TimeSpanDays >= 14 && TimeSpanDays < 30)
            {
                sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    DataAge = $"<b><span style='color:orange'>{DataAge}</span></b>",
                    CalculatedSessionAge = LangHelper.GetString("Session collected between 14 and 30 days ago")
                };

                sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
            else
            {
                sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    DataAge = $"<b><span style='color:red'>{DataAge}</span></b>",
                    CalculatedSessionAge = LangHelper.GetString("Session collected more than 30 days ago")
                };

                sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
        }
    }
}