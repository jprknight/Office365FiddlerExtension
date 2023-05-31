using Fiddler;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Office365FiddlerExtension.Services
{
    public class Preferences
    {
        /*public static void Initialize()
        {
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
        public static int GetSlowRunningSessionThreshold()
        {
            return 5000;
        }

        // 2.5 seconds for warning on the time a session took.
        public static int GetWarningSessionTimeThreshold()
        {
            return 2500;
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
                // It's confusing to see the name of this menu item change only after a click of the menu item.
                // Whether the extension is enabled or disabled on loading Fiddler, it will show Enable.
                // Stopping this to simplify the UI.
                //MenuUI.Instance.MiEnabled.Text = ExtensionEnabled ? "Disable" : "Enable";
                MenuUI.Instance.ExtensionMenu.Text = ExtensionEnabled ? "Office 365 (Enabled)" : "Office 365 (Disabled)";
            }
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

        public static bool BetaRuleSet
        {
            get => BetaRuleSet = FiddlerApplication.Prefs.GetBoolPref("extensions.Office365FiddlerExtension.BetaRuleSet", false);
            set { BetaRuleSet = value; FiddlerApplication.Prefs.SetBoolPref("extensions.Office365FiddlerExtension.BetaRuleSet", value); }
        }

        public static string LoadSazFilename
        {
            get => LoadSazFilename = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.LoadSazFilename", "");
            set { LoadSazFilename = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.LoadSazFilename", value); }
        }
    }
}
