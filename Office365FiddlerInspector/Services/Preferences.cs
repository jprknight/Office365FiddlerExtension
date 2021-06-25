using Fiddler;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Office365FiddlerInspector.Services
{
    public static class Preferences
    {
        public static void Initialize()
        {           
        }

        public static string GetStrNoKnownIssue()
        {
            return "<p>No known issue with Office 365 and this type of session. If you have a suggestion for an improvement, "
            + "create an issue or better yet a pull request in the project Github repository: "
            + "<a href='https://aka.ms/Office365FiddlerExtension' target='_blank'>https://aka.ms/Office365FiddlerExtension</a>.</p>";
        }


        /// <summary>
        /// This is the low water mark for what is considered a slow running session, considering a number of factors.
        /// Exchange response times are typically going to be much quicker than this. In the < 300ms range.
        /// </summary>
        public static int GetSlowRunningSessionThreshold()
        {
            return 5000;
        }

        // 2.5 seconds for warning on the time a session took.
        public static int GetWarningSessionTimeThreshold()
        {
            return 2500;
        }

        // 1 second for a good time on a session.
        public static int GetGoodSessionTimeThreshold()
        {
            return 1000;
        }

        public static Task<bool> SetDefaultPreferences()
        {
            ExtensionEnabled = true;

            ExecutionCount++;

            return Task.FromResult(true);
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

        private static bool _extensionEnabled;
        public static bool ExtensionEnabled
        {
            get => _extensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.Office365FiddlerExtension.enabled", true);
            set
            {
                _extensionEnabled = value;
                FiddlerApplication.Prefs.SetBoolPref("extensions.Office365FiddlerExtension.enabled", value);
                MenuUI.Instance.MiEnabled.Text = ExtensionEnabled ? "Disable" : "Enable";
                MenuUI.Instance.ExchangeOnlineTopMenu.Text = ExtensionEnabled ? "Office 365" : "Office 365 (Disabled)";
            }
        }

        private static bool _appLoggingEnabled;
        public static bool AppLoggingEnabled
        {
            get => _appLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.Office365FiddlerExtension.AppLoggingEnabled", true);
            // Removed AppLoggingEnabled Menu Item in the simplity update v1.71. Changed this to only allow a get.
            // Disable appLoggingEnabled via the Fiddler application preference if needed.
            // After leaving this on for several versions, no known issues raised. Making app logging enabled by default.
            //set
            //{
            //    _appLoggingEnabled = value;
            //    FiddlerApplication.Prefs.SetBoolPref("extensions.Office365FiddlerExtension.AppLoggingEnabled", value);
            //    MenuUI.Instance.miAppLoggingEnabled.Checked = AppLoggingEnabled;
            //}
        }

        private static Int32 _executionCount;
        public static Int32 ExecutionCount
        {
            get => _executionCount = FiddlerApplication.Prefs.GetInt32Pref("extensions.Office365FiddlerExtension.ExecutionCount", 0);
            set { _executionCount = value; FiddlerApplication.Prefs.SetInt32Pref("extensions.Office365FiddlerExtension.ExecutionCount", value); }
        }

        private static bool _ManualCheckForUpdate;

        public static bool ManualCheckForUpdate
        {
            get => _ManualCheckForUpdate = FiddlerApplication.Prefs.GetBoolPref("extensions.Office365FiddlerExtension.ManualCheckForUpdate", false);
            set { _ManualCheckForUpdate = value; FiddlerApplication.Prefs.SetBoolPref("extensions.Office365FiddlerExtension.ManualCheckForUpdate", value); }
        }

        private static bool _DisableWebCalls;

        public static bool DisableWebCalls
        {
            get => DisableWebCalls = FiddlerApplication.Prefs.GetBoolPref("extensions.Office365FiddlerExtension.NeverWebCall", false);
            set { _DisableWebCalls = value; FiddlerApplication.Prefs.SetBoolPref("extensions.Office365FiddlerExtension.NeverWebCall", value); }
        }
    }
}
