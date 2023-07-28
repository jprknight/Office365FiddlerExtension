using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class SessionType
    {
        internal Session session { get; set; }

        private static SessionType _instance;

        public static SessionType Instance => _instance ?? (_instance = new SessionType());

        public void SetSessionType_FreeBusy(Session session)
        {
            this.session = session;

            // If the session doesn't contain any of these features, return.
            if (!this.session.fullUrl.Contains("WSSecurity")
                || (!this.session.fullUrl.Contains("GetUserAvailability")
                || !(this.session.utilFindInResponse("GetUserAvailability", false) > 1)))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Running SetSessionType_FreeBusy");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "SessionType_Free/Busy",
                SessionType = "Free/Busy",
                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        public void SetSessionType_Microsoft365_EWS(Session session)
        {
            this.session = session;

            if (!this.session.fullUrl.Contains("outlook.office365.com/EWS"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Running SetSessionType_Microsoft365_EWS");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "SessionType_Microsoft365_EWS",
                SessionType = "Microsoft 365 Exchange Web Services",
                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        public void SetSessionType_EWS(Session session)
        {
            this.session = session;

            if (!this.session.fullUrl.Contains("/EWS"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Running SetSessionType_EWS");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "SessionType_EWS",
                SessionType = "Exchange Web Services",
                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        public void SetSessionType_Microsoft365_Authentication(Session session)
        {
            this.session = session;

            // This check needs to be inclusive, so we don't exclude sessions.
            if (this.session.url.Contains("login.microsoftonline.com") || this.session.HostnameIs("login.microsoftonline.com"))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Running SetSessionType_Microsoft365_Authentication");

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "SessionType_Microsoft365_Authentication",
                    SessionType = "Microsoft365 Authentication",
                    SessionTypeConfidenceLevel = 10
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
        }

        public void SetSessionType_ADFS_Authentication(Session session)
        {
            this.session = session;

            if (!this.session.fullUrl.Contains("adfs/services/trust/mex"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Running SetSessionType_ADFS_Authentication");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "SessionType_ADFS_Authentication",
                SessionType = "ADFS Authentication",
                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        public void SetSessionType_General_Microsoft365(Session session)
        {
            this.session = session;

            if (!this.session.fullUrl.Contains("outlook.office365.com"))
            {
                return;
            }

            if (!this.session.fullUrl.Contains("outlook.office.com"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Running SetSessionType_General_Microsoft365");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "SessionType_General_Microsoft365",
                SessionType = "General Microsoft365",
                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        public void SetSessionType_Office_Applications(Session session)
        {
            this.session = session;

            if (this.session.LocalProcess.Contains("outlook")
                || this.session.LocalProcess.Contains("searchprotocolhost")
                || this.session.LocalProcess.Contains("winword")
                || this.session.LocalProcess.Contains("excel")
                || this.session.LocalProcess.Contains("onenote")
                || this.session.LocalProcess.Contains("msaccess")
                || this.session.LocalProcess.Contains("powerpnt")
                || this.session.LocalProcess.Contains("mspub")
                || this.session.LocalProcess.Contains("onedrive")
                || this.session.LocalProcess.Contains("lync")
                || this.session.LocalProcess.Contains("w3wp"))
                {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Running SetSessionType_Office_Applications");

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "SessionType_Office_Applications",
                    SessionType = this.session.LocalProcess.Split(':')[0],
                    SessionTypeConfidenceLevel = 10
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);

            }
        }

        public void SetSessionType_Internet_Browsers(Session session)
        {
            this.session = session;

            if (this.session.LocalProcess.Contains("iexplore")
                || this.session.LocalProcess.Contains("chrome")
                || this.session.LocalProcess.Contains("firefox")
                || this.session.LocalProcess.Contains("edge")
                || this.session.LocalProcess.Contains("safari")
                || this.session.LocalProcess.Contains("brave"))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Running SetSessionType_Internet_Browsers");

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "SessionType_Internet_Browsers",
                    SessionType = this.session.LocalProcess.Split(':')[0],
                    SessionTypeConfidenceLevel = 10
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);

            }
        }

        public void SetSessionType_Unknown(Session session)
        {
            
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Running SetSessionType_Unclassified");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "SetSessionType",
                SessionType = this.session["X-ProcessName"],
                ResponseAlert = "Unclassified",
                ResponseComments = "The Office 365 Fiddler Extension does not yet have a way to classify this session."
                + "<p>If you have a suggestion for an improvement, create an issue or better yet a pull request in the project Github repository: "
                + "<a href='https://github.com/jprknight/Office365FiddlerExtension' target='_blank'>https://github.com/jprknight/Office365FiddlerExtension</a>.</p>",

                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}