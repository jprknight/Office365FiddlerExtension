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

        public void DressSessions(Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = JsonConvert.DeserializeObject<SessionFlagProcessor.ExtensionSessionFlags>(SessionFlagProcessor.Instance.GetSessionJsonData(this.session));

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

            // Set session flags used by columns added by the extension.
            this.session["X-ElapsedTIme"] = ExtensionSessionFlags.ElapsedTime;
            this.session["X-Authentication"] = ExtensionSessionFlags.Authentication;
            this.session["x-SessionType"] = ExtensionSessionFlags.SessionType;
            this.session["X-ResponseServer"] = ExtensionSessionFlags.ResponseServer;

            if (this.session["X-HostIP"] == null)
            {
                this.session["X-HostIP"] = "Not Present";
            }

            this.session.RefreshUI();
        }
    }
}
