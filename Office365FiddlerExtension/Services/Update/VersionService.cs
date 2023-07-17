using Fiddler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtension.Services
{
    public class VersionService
    {
        private static VersionService _instance;
        public static VersionService Instance => _instance ?? (_instance = new VersionService());

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
            var ExtensionVersion = VersionJsonService.Instance.GetDeserializedExtensionVersion();

            var dirInfo = new DirectoryInfo(SettingsJsonService.AssemblyDirectory);
            string pattern = ExtensionVersion.RulesetDLLPattern;

            try
            {
                FileInfo file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(file.FullName);

                return $"{fileVersionInfo.FileMajorPart}.{fileVersionInfo.FileMinorPart}.{fileVersionInfo.FileBuildPart}"; 
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} {ex}");
            } 

            return null;
        }

        public string GetGithubDLLVersion()
        {
            var ExtensionVersion = VersionJsonService.Instance.GetDeserializedExtensionVersion();

            return $"{ExtensionVersion.ExtensionMajor}.{ExtensionVersion.ExtensionMinor}.{ExtensionVersion.ExtensionBuild}";
        }
    }
}
