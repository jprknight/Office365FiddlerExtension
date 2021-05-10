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

        /// <summary>
        /// Return DeveloperDemoMode value.
        /// </summary>
        /// <returns>DeveloperDemoMode</returns>
        public static bool GetDeveloperMode()
        {
            return Debugger.IsAttached;
        }

        /// <summary>
        /// This is the low water mark for what is considered a slow running session, considering a number of factors.
        /// Exchange response times are typically going to be much quicker than this. In the < 300ms range.
        /// </summary>
        public static int GetSlowRunningSessionThreshold()
        {
            return 5000;
        }


        public static Task<bool> SetDefaultPreferences()
        {
            ExtensionEnabled = true;
            IsLoadSaz = false;

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
                MenuUI.Instance.miEnabled.Text = ExtensionEnabled ? "Disable" : "Enable";
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

        private static bool _isLoadSaz;
        public static bool IsLoadSaz
        {
            get => _isLoadSaz = FiddlerApplication.Prefs.GetBoolPref("extensions.Office365FiddlerExtension.LoadSaz", false);
            set { _isLoadSaz = value; FiddlerApplication.Prefs.SetBoolPref("extensions.Office365FiddlerExtension.LoadSaz", value); }
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

    }
}
