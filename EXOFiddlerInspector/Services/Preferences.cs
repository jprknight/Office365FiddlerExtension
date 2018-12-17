using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXOFiddlerInspector.Services
{
    public static class Preferences
    {
        public static bool DeveloperDemoMode { get; set; }
        public static bool DeveloperDemoModeBreakScenarios { get; set; }

        internal static List<string> Developers = new List<string>(new string[] { "jeknight", "brandev", "bever", "jasonsla", "nick", "jeremy" });

        public static bool IsDeveloper()
        {
            return Developers.Any(Environment.UserName.Contains);
        }

        /// <summary>
        /// Return DeveloperDemoMode value.
        /// </summary>
        /// <returns>DeveloperDemoMode</returns>
        public static bool GetDeveloperMode()
        {
            bool isdevMode = Developers.Any(Environment.UserName.Contains) && DeveloperDemoMode == true;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.DemoMode", isdevMode);

            return isdevMode;
        }

        /// <summary>
        /// Return DeveloperDemoModeBreakScenarios value.
        /// </summary>
        /// <returns>DeveloperDemoModeBreakScenarios</returns>
        public static bool GetDeveloperDemoModeBreakScenarios()
        {
            bool isdevModeBreak = Developers.Any(Environment.UserName.Contains) && DeveloperDemoModeBreakScenarios == true;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.DemoModeBreakScenarios", isdevModeBreak);

            return isdevModeBreak;
        }

        /// <summary>
        /// This is the low water mark for what is considered a slow running session, considering a number of factors.
        /// Exchange response times are typically going to be much quicker than this. In the < 300ms range.
        /// </summary>
        public static int GetSlowRunningSessionThreshold()
        {
            return 5000;
        }


        public static bool ExtensionEnabled { get; set; } = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.enabled", false);
        public static bool ElapsedTimeColumnEnabled { get; set; } = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.ElapsedTimeColumnEnabled", false);
        public static bool ResponseServerColumnEnabled { get; set; } = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.ResponseServerColumnEnabled", false);
        public static bool ExchangeTypeColumnEnabled { get; set; } = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.ExchangeTypeColumnEnabled", false);
        public static bool HostIPColumnEnabled { get; set; } = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.HostIPColumnEnabled", false);
        public static bool AuthColumnEnabled { get; set; } = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.AuthColumnEnabled", false);
        public static bool AppLoggingEnabled { get; set; } = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.AppLoggingEnabled", false);
        public static bool HighlightOutlookOWAOnlyEnabled { get; set; } = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.HighlightOutlookOWAOnlyEnabled", false);
        public static int iExecutionCount { get; set; } = FiddlerApplication.Prefs.GetInt32Pref("extensions.EXOFiddlerExtension.ExecutionCount", 0);
        public static bool IsLoadSaz { get; set; } = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.LoadSaz", false);
        public static bool ColumnsAllEnabled { get; set; } = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.ColumnsEnableAll", true);

    }
}
