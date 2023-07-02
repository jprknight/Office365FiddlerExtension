using Fiddler;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Office365FiddlerExtensionRuleset.Services
{
    public class Preferences
    {
        // Keep this around to migrate legacy settings to Json settings.
        private static bool _extensionEnabled;
        public static bool ExtensionEnabled
        {
            get => _extensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.Office365FiddlerExtension.enabled", true);
            /*set
            {
                _extensionEnabled = value;
                FiddlerApplication.Prefs.SetBoolPref("extensions.Office365FiddlerExtension.enabled", value);
                // It's confusing to see the name of this menu item change only after a click of the menu item.
                // Whether the extension is enabled or disabled on loading Fiddler, it will show Enable.
                // Stopping this to simplify the UI.
                //MenuUI.Instance.MiEnabled.Text = ExtensionEnabled ? "Disable" : "Enable";
                MenuUI.Instance.ExchangeOnlineTopMenu.Text = ExtensionEnabled ? "Office 365" : "Office 365 (Disabled)";
            }*/
        }

        // Keep this around to migrate legacy settings to Json settings.
        private static bool _neverWebCall;

        public static bool NeverWebCall
        {
            get => _neverWebCall = FiddlerApplication.Prefs.GetBoolPref("extensions.Office365FiddlerExtension.NeverWebCall", false);
            //set { _neverWebCall = value; FiddlerApplication.Prefs.SetBoolPref("extensions.Office365FiddlerExtension.NeverWebCall", value); }
        }

        // Keep this around to migrate legacy settings to Json settings.
        private static Int32 _executionCount;
        public static Int32 ExecutionCount
        {
            get => _executionCount = FiddlerApplication.Prefs.GetInt32Pref("extensions.Office365FiddlerExtension.ExecutionCount", 0);
            //set { _executionCount = value; FiddlerApplication.Prefs.SetInt32Pref("extensions.Office365FiddlerExtension.ExecutionCount", value); }
        }

        // Setting to store Json extension settings in. Updated within the extension only, no remote updates.
        private static string _extensionSettings;

        public static string ExtensionSettings
        {
            get => _extensionSettings = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.ExtensionSettings", null);
            set { _extensionSettings = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.ExtensionSettings", value); }
        }
    }
}
