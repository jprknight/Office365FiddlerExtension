using Fiddler;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Linq;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Class to call the external rulset DLL file, passing in the session for processing.
    /// </summary>
    class RulesetService
    {
        internal Session session { get; set; }

        private static RulesetService _instance;

        public static RulesetService Instance => _instance ?? (_instance = new RulesetService());

        /// <summary>
        /// Call out to the ruleset DLL to run logic against the current session.
        /// </summary>
        /// <param name="session"></param>
        public void CallRunRuleSet(Session session)
        {
            this.session = session;

            // Avoid null object exceptions when the SessionClassification is not yet created.
            // First run after extension install scenario. Once the SessionClassification Json preference
            // is created, this won't come into play.
            if (Preferences.SessionClassification == null)
            {
                return;
            }

            var ExtensionVersion = VersionJsonService.Instance.GetDeserializedExtensionVersion();

            string pattern = ExtensionVersion.RulesetDLLPattern;
            var dirInfo = new DirectoryInfo(SettingsJsonService.AssemblyDirectory);

            try
            {
                FileInfo file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();

                Assembly rulesetDDL = Assembly.LoadFile(file.FullName);

                // type is Namespace.Class
                var type = rulesetDDL.GetType("Office365FiddlerExtensionRuleset.RunRuleSet");
                
                var obj = Activator.CreateInstance(type);

                var method = type.GetMethod("Initialize");

                method.Invoke(obj, new object[] { this.session });
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} FATAL ERROR: CANNOT LOAD RULESET DLL!");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} {ex}");
            }
        }
    }
}
