using System.Resources;
using System.Reflection;
using System.Globalization;
using Newtonsoft.Json;
using Fiddler;

namespace Office365FiddlerExtensionRuleset.Services
{
    /// <summary>
    /// Ruleset language service to provide resource strings for ruleset text.
    /// </summary>
    public static class RulesetLangHelper
    {
        private static ResourceManager _resourcemanager;

        /// <summary>
        /// 
        /// </summary>
        /// 
        static RulesetLangHelper()
        {
            _resourcemanager = new ResourceManager("Office365FiddlerExtensionRuleset.Language.ruleset-strings", Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Get string from the strings.resx langauge resource file.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetString(string name)
        {
            return _resourcemanager.GetString(name);
        }

        /// <summary>
        /// Change language.
        /// </summary>
        /// <param name="language"></param>
        public static void ChangeLanguage(string language)
        {
            var cultureInfo = new CultureInfo(language);

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = Office365FiddlerExtension.Services.SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            extensionSettings.PreferredLanguage = language;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Office365FiddlerExtension.Services.Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} (LanguageService): Language set to: " +
                $"{Office365FiddlerExtension.Services.SettingsJsonService.Instance.GetDeserializedExtensionSettings().PreferredLanguage}.");

        }
    }
}
