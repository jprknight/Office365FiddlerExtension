using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtension.Services
{
    public class VersionService
    {
        public string GetExtensionDLLVersion()
        {
            return $"{Assembly.GetExecutingAssembly().GetName().Version.Major}." +
                $"{Assembly.GetExecutingAssembly().GetName().Version.Minor}." +
                $"{Assembly.GetExecutingAssembly().GetName().Version.Build}";
        }

        public string GetExtensionRulesetDLLVersion()
        {
            // Get Ruleset file from assembly path.
            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            string RulesetPath = extensionSettings.ExtensionPath;


            return "stuff";
        }
    }
}
