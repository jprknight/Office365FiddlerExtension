﻿using Fiddler;
using Newtonsoft.Json;
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
            //ExtensionEnabled = true;

            SettingsHandler.Instance.IncrementExecutionCount();

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

        private static bool _neverWebCall;

        public static bool NeverWebCall
        {
            get => _neverWebCall = FiddlerApplication.Prefs.GetBoolPref("extensions.Office365FiddlerExtension.NeverWebCall", false);
            set { _neverWebCall = value; FiddlerApplication.Prefs.SetBoolPref("extensions.Office365FiddlerExtension.NeverWebCall", value); }
        }

        private static Int32 _executionCount;
        public static Int32 ExecutionCount
        {
            get => _executionCount = FiddlerApplication.Prefs.GetInt32Pref("extensions.Office365FiddlerExtension.ExecutionCount", 0);
            set { _executionCount = value; FiddlerApplication.Prefs.SetInt32Pref("extensions.Office365FiddlerExtension.ExecutionCount", value); }
        }

        private static string _extensionVersion;

        public static string ExtensionVersion
        {
            get => _extensionVersion = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.ExtensionVersion", null);
            set { _extensionVersion = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.ExtensionVersion", value); }
        }

        private static string _extensionSettings;

        public static string ExtensionSettings
        {
            get => _extensionSettings = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.ExtensionSettings", null);
            set { _extensionSettings = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.ExtensionSettings", value); }
        }

        public static string _extensionURLs;

        public static string ExtensionURLs
        {
            get => _extensionURLs = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.ExtensionURLs", null);
            set { _extensionURLs = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.ExtensionURLs", value); }
        }
    }
}
