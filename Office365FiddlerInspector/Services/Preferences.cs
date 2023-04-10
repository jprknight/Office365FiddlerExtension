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
                // It's confusing to see the name of this menu item change only after a click of the menu item.
                // Whether the extension is enabled or disabled on loading Fiddler, it will show Enable.
                // Stopping this to simplify the UI.
                //MenuUI.Instance.MiEnabled.Text = ExtensionEnabled ? "Disable" : "Enable";
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


        // Colour codes for sessions. Softer tones, easier on the eye than standard red, orange and green.
        public static String HTMLColourBlue => "#81BEF7";
        public static String HTMLColourGreen => "#81F7BA";
        public static String HTMLColourRed => "#F06141";
        public static String HTMLColourGrey => "#BDBDBD";
        public static String HTMLColourOrange =>  "#F59758";

        private static int iSACL;

        // Session Type Confidence Level.
        private static int iSTCL;

        // Session Response Server Confidence Level.
        private static int iSRSCL;

        public static void GetSACL(Session session)
        {
            // Avoid null object exceptions by setting this session flag to something
            // rather than nothing.
            if (session["X-SACL"] == null || session["X-SACL"] == "")
            {
                session["X-SACL"] = "00";
            }
            iSACL = int.Parse(session["X-SACL"]);
        }

        // Set Session Authentication Confidence Level.
        public static void SetSACL(Session session, string SACL)
        {
            session["X-SACL"] = SACL;
        }

        // Get Session Type Confidence Level.
        public static void GetSTCL(Session session)
        {
            // Avoid null object exceptions by setting this session flag to something
            // rather than nothing.
            if (session["X-STCL"] == null || session["X-STCL"] == "")
            {
                session["X-STCL"] = "00";
            }
            iSTCL = int.Parse(session["X-STCL"]);
        }

        // Set Session Type Confidence Level.
        public static void SetSTCL(Session session, string STCL)
        {
            session["X-STCL"] = STCL;
        }

        // Get Session Response Server Confidence Level.
        public static void GetSRSCL(Session session)
        {
            // Avoid null object exceptions by setting this session flag to something
            // rather than nothing.
            if (session["X-SRSCL"] == null || session["X-SRSCL"] == "")
            {
                session["X-SRSCL"] = "00";
            }
            iSRSCL = int.Parse(session["X-SRSCL"]);
        }

        // Set Session Response Server Confidence Level.
        public static void SetSRSCL(Session session, string SRSCL)
        {
            session["X-SRSCL"] = SRSCL;
        }

        public static void SetProcess(Session session)
        {
            // Set process name, split and exclude port used.
            if (session.LocalProcess != String.Empty)
            {
                string[] ProcessName = session.LocalProcess.Split(':');
                session["X-ProcessName"] = ProcessName[0];
            }
            // No local process to split.
            else
            {
                session["X-ProcessName"] = "Remote Capture";
            }
        }
    }
}
