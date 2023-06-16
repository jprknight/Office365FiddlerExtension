using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtension.Ruleset
{
    class SessionType : ActivationService
    {
        private static SessionType _instance;

        public static SessionType Instance => _instance ?? (_instance = new SessionType());

        public void SetSessionType_FreeBusy(Session session)
        {
            this.Session = session;

            // If the session doesn't contain any of these features, return.
            if (!this.Session.fullUrl.Contains("WSSecurity")
                || (!this.Session.fullUrl.Contains("GetUserAvailability")
                || !(this.Session.utilFindInResponse("GetUserAvailability", false) > 1)))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} Running SetSessionType_FreeBusy");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "SessionType_Free/Busy",
                SessionType = "Free/Busy",
                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }

        public void SetSessionType_Microsoft365_EWS(Session session)
        {
            this.Session = session;

            if (!this.Session.fullUrl.Contains("outlook.office365.com/EWS"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} Running SetSessionType_Microsoft365_EWS");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "SessionType_Microsoft365_EWS",
                SessionType = "Microsoft 365 Exchange Web Services",
                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }

        public void SetSessionType_EWS(Session session)
        {
            this.Session = session;

            if (!this.Session.fullUrl.Contains("/EWS"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} Running SetSessionType_EWS");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "SessionType_EWS",
                SessionType = "Exchange Web Services",
                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }

        public void SetSessionType_Microsoft365_Authentication(Session session)
        {
            this.Session = session;

            // This check needs to be inclusive, so we don't exclude sessions.
            if (this.Session.url.Contains("login.microsoftonline.com") || this.Session.HostnameIs("login.microsoftonline.com"))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} Running SetSessionType_Microsoft365_Authentication");

                var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                {
                    SectionTitle = "SessionType_Microsoft365_Authentication",
                    SessionType = "Microsoft365 Authentication",
                    SessionTypeConfidenceLevel = 10
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
            }
        }

        public void SetSessionType_ADFS_Authentication(Session session)
        {
            this.Session = session;

            if (!this.Session.fullUrl.Contains("adfs/services/trust/mex"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} Running SetSessionType_ADFS_Authentication");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "SessionType_ADFS_Authentication",
                SessionType = "ADFS Authentication",
                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }

        public void SetSessionType_General_Microsoft365(Session session)
        {
            this.Session = session;

            if (!this.Session.fullUrl.Contains("outlook.office365.com"))
            {
                return;
            }

            if (!this.Session.fullUrl.Contains("outlook.office.com"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} Running SetSessionType_General_Microsoft365");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "SessionType_General_Microsoft365",
                SessionType = "General Microsoft365",
                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }

        public void SetSessionType_Office_Applications(Session session)
        {
            this.Session = session;

            if (this.Session.LocalProcess.Contains("outlook")
                || this.Session.LocalProcess.Contains("searchprotocolhost")
                || this.Session.LocalProcess.Contains("winword")
                || this.Session.LocalProcess.Contains("excel")
                || this.Session.LocalProcess.Contains("onenote")
                || this.Session.LocalProcess.Contains("msaccess")
                || this.Session.LocalProcess.Contains("powerpnt")
                || this.Session.LocalProcess.Contains("mspub")
                || this.Session.LocalProcess.Contains("onedrive")
                || this.Session.LocalProcess.Contains("lync")
                || this.Session.LocalProcess.Contains("w3wp"))
                {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} Running SetSessionType_Office_Applications");

                var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                {
                    SectionTitle = "SessionType_Office_Applications",
                    SessionType = this.Session.LocalProcess.Split(':')[0],
                    SessionTypeConfidenceLevel = 10
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);

            }
        }

        public void SetSessionType_Internet_Browsers(Session session)
        {
            this.Session = session;

            if (this.Session.LocalProcess.Contains("iexplore")
                || this.Session.LocalProcess.Contains("chrome")
                || this.Session.LocalProcess.Contains("firefox")
                || this.Session.LocalProcess.Contains("edge")
                || this.Session.LocalProcess.Contains("safari")
                || this.Session.LocalProcess.Contains("brave"))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} Running SetSessionType_Internet_Browsers");

                var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                {
                    SectionTitle = "SessionType_Internet_Browsers",
                    SessionType = this.Session.LocalProcess.Split(':')[0],
                    SessionTypeConfidenceLevel = 10
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);

            }
        }

        public void SetSessionType_Unknown(Session session)
        {
            
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} Running SetSessionType_Unclassified");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "SetSessionType",
                SessionType = this.Session["X-ProcessName"],
                ResponseAlert = "Unclassified",
                ResponseComments = "The Office 365 Fiddler Extension does not yet have a way to classify this session."
                + "<p>If you have a suggestion for an improvement, create an issue or better yet a pull request in the project Github repository: "
                + "<a href='https://github.com/jprknight/Office365FiddlerExtension' target='_blank'>https://github.com/jprknight/Office365FiddlerExtension</a>.</p>",

                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }
    }
}