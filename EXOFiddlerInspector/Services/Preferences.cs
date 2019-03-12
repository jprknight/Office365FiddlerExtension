using Fiddler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EXOFiddlerInspector.Services
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
            AppLoggingEnabled = true;
            HighlightOutlookOWAOnlyEnabled = true;
            IsLoadSaz = false;
            //ColumnsAllEnabled = true;

            FiddlerApplication.Prefs.SetInt32Pref("extensions.EXOFiddlerInspector.ExecutionCount", 1);

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
            get => _extensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", true);
            set  { _extensionEnabled = value; FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.enabled", value); }
        }

        private static bool _appLoggingEnabled;
        public static bool AppLoggingEnabled
        {
            get => _appLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", true);
            set { _appLoggingEnabled = value; FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", value); }
        }

        private static bool _highlightOutlookOWAOnlyEnabled;
        public static bool HighlightOutlookOWAOnlyEnabled
        {
            get => _highlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled", true);
            set { _highlightOutlookOWAOnlyEnabled = value; FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled", value); }
        }

        private static bool _isLoadSaz;
        public static bool IsLoadSaz
        {
            get => _isLoadSaz = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.LoadSaz", false);
            set { _isLoadSaz = value; FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.LoadSaz", value); }
        }

        //private static bool _columnsAllEnabled;
        //public static bool ColumnsAllEnabled
        //{
        //    get => _columnsAllEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", _columnsAllEnabled);
        //    set { _columnsAllEnabled = value; FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.enabled", value); }
        //}

        private static Int32 _executionCount;
        public static Int32 ExecutionCount
        {
            get => _executionCount = FiddlerApplication.Prefs.GetInt32Pref("extensions.EXOFiddlerInspector.ExecutionCount", 0);
            set { _executionCount = value; FiddlerApplication.Prefs.SetInt32Pref("extensions.EXOFiddlerInspector.ExecutionCount", value); }
        }

    }
}
