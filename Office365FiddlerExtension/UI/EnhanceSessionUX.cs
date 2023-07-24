using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;
using Newtonsoft.Json;

namespace Office365FiddlerExtension.UI
{
    /// <summary>
    /// Set colours on session foreground and background. Called from SessionHandler once
    /// session flags have been set by extension ruleset.
    /// </summary>
    public  class EnhanceSessionUX
    {
        internal Session session { get; set; }

        private static EnhanceSessionUX _instance;
        public static EnhanceSessionUX Instance => _instance ?? (_instance = new EnhanceSessionUX());

        public void NormaliseSession(Session session)
        {
            this.session = session;

            // Session colours.
            this.session["UI-BACKCOLOR"] = "#FFFFFF";
            this.session["UI-COLOR"] = "#000000";
        }

        public void EnhanceSession(Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            // Set session background colour. Default to gray if undefined.
            switch (ExtensionSessionFlags.SessionSeverity)
            {
                case 0: // GRAY - Uninteresting.
                    this.session["UI-BACKCOLOR"] = "#BDBDBD";
                    this.session["UI-COLOR"] = "#000000";
                    break;
                case 10: // BLUE - False Positive.
                    this.session["UI-BACKCOLOR"] = "#81BEF7";
                    this.session["UI-COLOR"] = "#000000";
                    break;
                case 20: // GREEN - Normal.
                    this.session["uUI-BACKCOLOR"] = "#81F7BA";
                    this.session["UI-COLOR"] = "#000000";
                    break;
                case 30: // ORANGE - Warning.
                    this.session["UI-BACKCOLOR"] = "#F59758";
                    this.session["UI-COLOR"] = "#000000";
                    break;
                case 40: // BLACK - Concerning.
                    this.session["ui-backcolor"] = "#000000";
                    this.session["UI-COLOR"] = "#F06141";
                    break;
                case 50: // RED - Severe.
                    this.session["UI-BACKCOLOR"] = "#F06141";
                    this.session["UI-COLOR"] = "#000000";
                    break;                    
                default:
                    // Default to gray, so we know if something isn't caught.
                    this.session["UI-BACKCOLOR"] = "#BDBDBD";
                    this.session["UI-COLOR"] = "#000000";
                    break;
            }
            this.session.RefreshUI();
        }

        public void SetSessionUninteresting()
        {
            var sessions = FiddlerApplication.UI.GetSelectedSessions();

            foreach (var session in sessions)
            {
                this.session = session;
                this.session["UI-BACKCOLOR"] = "#BDBDBD";
                this.session["UI-COLOR"] = "#000000";
                this.session.RefreshUI();

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SessionSeverity = 0
                };
                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
        }

        public void SetSessionFalsePositive()
        {
            var sessions = FiddlerApplication.UI.GetSelectedSessions();

            foreach (var session in sessions)
            {
                this.session = session;
                this.session["UI-BACKCOLOR"] = "#81BEF7";
                this.session["UI-COLOR"] = "#000000";
                this.session.RefreshUI();

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SessionSeverity = 10
                };
                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
        }

        public void SetSessionNormal()
        {
            var sessions = FiddlerApplication.UI.GetSelectedSessions();

            foreach (var session in sessions)
            {
                this.session = session;
                this.session["UI-BACKCOLOR"] = "#81F7BA";
                this.session["UI-COLOR"] = "#000000";
                this.session.RefreshUI();

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SessionSeverity = 20
                };
                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
        }

        public void SetSessionWarning()
        {
            var sessions = FiddlerApplication.UI.GetSelectedSessions();

            foreach (var session in sessions)
            {
                this.session = session;
                this.session["UI-BACKCOLOR"] = "#F59758";
                this.session["UI-COLOR"] = "#000000";
                this.session.RefreshUI();

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SessionSeverity = 30
                };
                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
        }

        public void SetSessionConcerning()
        {
            var sessions = FiddlerApplication.UI.GetSelectedSessions();

            foreach (var session in sessions)
            {
                this.session = session;
                this.session["UI-BACKCOLOR"] = "#000000";
                this.session["UI-COLOR"] = "#F06141";
                this.session.RefreshUI();

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SessionSeverity = 40
                };
                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
        }

        public void SetSessionSevere()
        {
            var sessions = FiddlerApplication.UI.GetSelectedSessions();

            foreach (var session in sessions)
            {
                this.session = session;
                this.session["UI-BACKCOLOR"] = "#F06141";
                this.session["UI-COLOR"] = "#000000";
                this.session.RefreshUI();

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SessionSeverity = 50
                };
                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
        }
    }
}
