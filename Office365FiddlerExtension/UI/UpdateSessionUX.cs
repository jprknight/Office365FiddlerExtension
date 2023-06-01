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
    internal class UpdateSessionUX : ActivationService
    {
        private static UpdateSessionUX _instance;
        public static UpdateSessionUX Instance => _instance ?? (_instance = new UpdateSessionUX());

        public void NormaliseSession(Session session)
        {
            this.session = session;

            // Extension Json Data.
            this.session["Microsoft365FiddlerExtensionJson"] = null;

            // Session colours.
            this.session["UI-BACKCOLOR"] = "#FFFFFF";
            this.session["UI-COLOR"] = "#000000";
        }

        public void EnhanceSession(Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.session);

            // Set session background colour. Default to gray if undefined.
            switch (ExtensionSessionFlags.UIBackColour.ToLower())
            {
                case "blue":
                    this.session["ui-backcolor"] = "#81BEF7";
                    break;
                case "green":
                    this.session["ui-backcolor"] = "#81F7BA";
                    break;
                case "red":
                    this.session["ui-backcolor"] = "#F06141";
                    break;
                case "orange":
                    this.session["ui-backcolor"] = "#F59758";
                    break;
                case "black":
                    this.session["ui-backcolor"] = "#000000";
                    break;
                default:
                    // Default to gray, so we know if something isn't caught.
                    this.session["ui-backcolor"] = "#BDBDBD";
                    break;
            }

            // Set session text colour. Default to black.
            switch (ExtensionSessionFlags.UITextColour.ToLower())
            {
                case "red":
                    this.session["ui-color"] = "#F06141";
                    break;
                default:
                    this.session["ui-color"] = "#000000";
                    break;
            }

            this.session.RefreshUI();
        }

        // Called by ActivationService to populate column data.
        public string ElapsedTime (Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.session);

            return ExtensionSessionFlags.ElapsedTime;
        }

        // Called by ActivationService to populate column data.
        public string SessionType(Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.session);

            return ExtensionSessionFlags.SessionType;
        }

        // Called by ActivationService to populate column data.
        public string Authentication (Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.session);

            return ExtensionSessionFlags.Authentication;
        }

        // Called by ActivationService to populate column data.
        public string ResponseServer (Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.session);

            return ExtensionSessionFlags.ResponseServer;
        }

        public string HostIP (Session session)
        {
            this.session = session;

            if (this.session["X-HostIP"] != null && this.session["X-HostIP"] != "")
            {
                return this.session["X-HostIP"];
            }
            else
            {
                return "Unknown";
            }
        }
    }
}
