using Fiddler;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Office365FiddlerExtension.Services
{
    public class Preferences
    {
        /*public static void Initialize()
        {
        }*/

        /// <summary>
        /// Return string for sessions where no known issue is needed.
        /// Used across response code logic.
        /// </summary>
        /// <returns></returns>
        /*private static string ResponseCommentsNoKnownIssue()
        {
            var parsedObject = JObject.Parse(Preferences.SessionClassification);
            return parsedObject["ResponseCommentsNoKnownIssue"].ToString();
        }*/

        public static string LogPrepend()
        {
            return "Office365FiddlerExtension";
        }

        /// <summary>
        /// This is the low water mark for what is considered a slow running session, considering a number of factors.
        /// Exchange response times are typically going to be much quicker than this. In the < 300ms range.
        /// I haven't found that many Microsoft365 client issues have been resolved with Fiddler and slow session times.
        /// So it's generally one of the last things to look at. If we're into slow network connectivity, Wireshark or
        /// something like that is the better tool.
        /// </summary>
        /*public static int GetSlowRunningSessionThreshold()
        {
            return 5000;
        }*/

        // 2.5 seconds for warning on the time a session took.
        /*public static int GetWarningSessionTimeThreshold()
        {
            return 2500;
        }*/

        // Keep this as a sample of might be an async function.
        /*
        public static Task<bool> SetDefaultPreferences()
        {
            //ExtensionEnabled = true;

            return Task.FromResult(true);
        }
        */

        // Keep this around to migrate legacy settings to Json settings.
#pragma warning disable IDE0052
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

        public static string AppVersion
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();

                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                return fileVersionInfo.FileVersion;
            }
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

        // Setting to store Json version information to run update checks against. Updated from remote.
        /*private static string _extensionVersion;

        public static string ExtensionVersion
        {
            get => _extensionVersion = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.ExtensionVersion", null);
            set { _extensionVersion = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.ExtensionVersion", value); }
        }*/

        // Setting to store Json extension settings in. Updated within the extension only, no remote updates.
        private static string _extensionSettings;

        public static string ExtensionSettings
        {
            get => _extensionSettings = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.ExtensionSettings", null);
            set { _extensionSettings = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.ExtensionSettings", value); }
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

        // Setting to store Json extension URLs. Update from remote.
        /*private static string _extensionURLs;

        public static string ExtensionURLs
        {
            get => _extensionURLs = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.ExtensionURLs", null);
            set { _extensionURLs = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.ExtensionURLs", value); }
        }*/
    }
}
