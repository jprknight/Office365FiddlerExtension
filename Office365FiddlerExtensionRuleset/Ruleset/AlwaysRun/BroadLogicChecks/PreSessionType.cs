using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class PreSessionType
    {
        internal Session session { get; set; }

        private static PreSessionType _instance;

        public static PreSessionType Instance => _instance ?? (_instance = new PreSessionType());

        public void Run(Session session)
        {
            this.session = session;

            SetSessionType_Legacy_FreeBusy(this.session);
            SetSessionType_Outlook_Desktop_FreeBusy(this.session);
            SetSessionType_OWA_FreeBusy(this.session);
        }

        private void SetSessionType_Legacy_FreeBusy(Session session)
        {
            this.session = session;

            // If the session doesn't contain any of these features, return.
            if (!this.session.fullUrl.Contains("WSSecurity")
                || (!this.session.fullUrl.Contains("GetUserAvailability")
                || !(this.session.utilFindInResponse("GetUserAvailability", false) > 1)))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running PreSessionType_FreeBusy");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "SessionType_Free/Busy",

                SessionType = LangHelper.GetString("FreeBusy"),
                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        private void SetSessionType_Outlook_Desktop_FreeBusy(Session session)
        {
            // If the session doesn't contain any of these features, return.
            this.session = session;

            // If the session doesn't contain any of these features, return.
            if (!this.session.fullUrl.Contains("outlook.office.com/CalendarService/api/"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running PreSessionType_FreeBusy");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "SessionType_Free/Busy",

                SessionType = LangHelper.GetString("FreeBusy"),
                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        private void SetSessionType_OWA_FreeBusy(Session session)
        {
            // If the session doesn't contain any of these features, return.
            this.session = session;

            // If the session doesn't contain any of these features, return.
            if (!this.session.fullUrl.Contains("outlook.office.com/outlookgatewayb2/"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running PreSessionType_FreeBusy");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "SessionType_Free/Busy",

                SessionType = LangHelper.GetString("FreeBusy"),
                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
