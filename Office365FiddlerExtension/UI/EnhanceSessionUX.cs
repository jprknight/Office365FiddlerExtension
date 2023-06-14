using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;

namespace Office365FiddlerExtension.UI
{
    internal class EnhanceSessionUX : ActivationService
    {
        private static EnhanceSessionUX _instance;
        public static EnhanceSessionUX Instance => _instance ?? (_instance = new EnhanceSessionUX());

        public void NormaliseSession(Session session)
        {
            this.Session = session;

            // Extension Json Data.
            this.Session["Microsoft365FiddlerExtensionJson"] = null;

            // Session colours.
            this.Session["UI-BACKCOLOR"] = "#FFFFFF";
            this.Session["UI-COLOR"] = "#000000";
        }

        public void EnhanceSession(Session session)
        {
            this.Session = session;

            var ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);

            // Set session background colour. Default to gray if undefined.
            switch (ExtensionSessionFlags.UIBackColour.ToLower())
            {
                case "blue":
                    this.Session["ui-backcolor"] = "#81BEF7";
                    break;
                case "green":
                    this.Session["ui-backcolor"] = "#81F7BA";
                    break;
                case "red":
                    this.Session["ui-backcolor"] = "#F06141";
                    break;
                case "orange":
                    this.Session["ui-backcolor"] = "#F59758";
                    break;
                case "black":
                    this.Session["ui-backcolor"] = "#000000";
                    break;
                default:
                    // Default to gray, so we know if something isn't caught.
                    this.Session["ui-backcolor"] = "#BDBDBD";
                    break;
            }

            // Set session text colour. Default to black.
            switch (ExtensionSessionFlags.UITextColour.ToLower())
            {
                case "red":
                    this.Session["ui-color"] = "#F06141";
                    break;
                default:
                    this.Session["ui-color"] = "#000000";
                    break;
            }

            this.Session.RefreshUI();
        }
    }
}
