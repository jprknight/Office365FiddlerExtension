using Fiddler;
using System;

namespace Office365FiddlerExtension.Services
{
    public class Preferences
    {
        // Setting to store Json version information to run update checks against.
        // The information stored in this application preference is not intended to be updated by the extension itself, it is only updated from Github.
        /*
        private static string _extensionVersion;

        public static string ExtensionVersion
        {
            get => _extensionVersion = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.ExtensionVersion", null);
            set { _extensionVersion = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.ExtensionVersion", value); }
        }
        */

        // Setting to store Json extension URLs. Update from remote.
        /*
        private static string _extensionURLs;

        public static string ExtensionURLs
        {
            get => _extensionURLs = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.ExtensionURLs", null);
            set { _extensionURLs = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.ExtensionURLs", value); }
        }
        */

        // Setting to store Json extension settings in. Updated within the extension only, no remote updates.
        private static string _extensionSettings;

        public static string ExtensionSettings
        {
            get => _extensionSettings = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.ExtensionSettings", null);
            set { _extensionSettings = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.ExtensionSettings", value); }
        }

        // Setting to store Json version information to run update checks against. Updated from remote.
        private static string _extensionVersion;

        public static string ExtensionVersion
        {
            get => _extensionVersion = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.ExtensionVersion", null);
            set { _extensionVersion = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.ExtensionVersion", value); }
        }

        // Setting to store Json extension URLs. Update from remote.
        private static string _extensionURLs;

        public static string ExtensionURLs
        {
            get => _extensionURLs = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.ExtensionURLs", null);
            set { _extensionURLs = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.ExtensionURLs", value); }
        }

        private static string _sessionClassification;

        public static string SessionClassification
        {
            get => _sessionClassification = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.SessionClassification", null);
            set { _sessionClassification = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.SessionClassification", value); }

        }

        private static string _microsoftURLsIPsWebService;

        public static string MicrosoftURLsIPsWebService
        {
            get => _microsoftURLsIPsWebService = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.MicrosoftURLsIPsWebService", null);
            set { _microsoftURLsIPsWebService = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.MicrosoftURLsIPsWebService", value); }
        }

        /// <summary>
        /// 
        /// LEGACY APPLICATION PREFERENCES.
        /// 
        /// The following application preferences remain with only GETS, since as the application is upgraded from v1.0.x to v2.0.x these
        /// values are read and written to their corresponding locations in Json application preferences.
        /// 
        /// To read settings from their new locations:
        /// 
        /// Example: SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall
        /// 
        /// </summary>

        private static bool _extensionEnabled;
        
        public static bool ExtensionEnabled
        {
            get => _extensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.Office365FiddlerExtension.enabled", true);
            // Set commented out and should always remain commented out. ExtensionEnabled should be set in the extension settings Json application preference.
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
            // Set commented out and should always remain commented out. NeverWebCall should be set in the extension settings Json application preference.
            // set { _neverWebCall = value; FiddlerApplication.Prefs.SetBoolPref("extensions.Office365FiddlerExtension.NeverWebCall", value); }
        }

        // Keep this around to migrate legacy settings to Json settings.
        private static Int32 _executionCount;

        public static Int32 ExecutionCount
        {
            get => _executionCount = FiddlerApplication.Prefs.GetInt32Pref("extensions.Office365FiddlerExtension.ExecutionCount", 0);
            // Set commented out and should always remain commented out. NeverWebCall should be set in the Execution Count Json application preference.
            //set { _executionCount = value; FiddlerApplication.Prefs.SetInt32Pref("extensions.Office365FiddlerExtension.ExecutionCount", value); }
        }
    }
}
