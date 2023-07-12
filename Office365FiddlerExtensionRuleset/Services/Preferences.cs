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
        // Setting to store Json extension settings in. Updated within the extension only, no remote updates.
        private static string _extensionSettings;

        public static string ExtensionSettings
        {
            get => _extensionSettings = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.ExtensionSettings", null);
            set { _extensionSettings = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.ExtensionSettings", value); }
        }
    }
}
