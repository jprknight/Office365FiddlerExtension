using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtension.UI
{
    /// <summary>
    /// Set colours on session foreground and background.
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

            // Set session background colour. Default to grey if undefined.
            switch (ExtensionSessionFlags.SessionSeverity)
            {
                case 10: // GREY - Uninteresting.
                    this.session["UI-BACKCOLOR"] = "#BDBDBD";
                    this.session["UI-COLOR"] = "#000000";
                    break;
                case 20: // BLUE - False Positive.
                    this.session["UI-BACKCOLOR"] = "#81BEF7";
                    this.session["UI-COLOR"] = "#000000";
                    break;
                case 30: // GREEN - Normal.
                    this.session["UI-BACKCOLOR"] = "#81F7BA";
                    this.session["UI-COLOR"] = "#000000";
                    break;
                case 40: // ORANGE - Warning.
                    this.session["UI-BACKCOLOR"] = "#F59758";
                    this.session["UI-COLOR"] = "#000000";
                    break;
                case 50: // BLACK - Concerning.
                    this.session["ui-backcolor"] = "#000000";
                    this.session["UI-COLOR"] = "#F06141";
                    break;
                case 60: // RED - Severe.
                    this.session["UI-BACKCOLOR"] = "#F06141";
                    this.session["UI-COLOR"] = "#000000";
                    break;                    
                default:
                    // Default to light pink, so we know if something isn't caught.
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                        $"({this.GetType().Name}): {this.session.id} Session severity NOT set, session background set to pink.");

                    this.session["UI-BACKCOLOR"] = "#FFB6C1";
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
                    SessionSeverity = 10
                };
                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, true);
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
                    SessionSeverity = 20
                };
                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, true);
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
                    SessionSeverity = 30
                };
                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, true);
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
                    SessionSeverity = 40
                };
                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, true);
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
                    SessionSeverity = 50
                };
                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, true);
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
                    SessionSeverity = 60
                };
                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, true);
            }
        }
    }
}
