using Fiddler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtension.Services
{
    public class UpgradeService
    {
        private static UpgradeService _instance;
        public static UpgradeService Instance => _instance ?? (_instance = new UpgradeService());

        public void Run()
        {
            //RemoveExistingFiles();

        }

        private void DownloadFilesFromGithub()
        {

        }

        private void RemoveExistingFiles()
        {
            string[] extensionFiles = ["Office365FiddlerExtension.dll",
                "Office365FiddlerExtension.dll.config",
                "Office365FiddlerExtension.pdb",
                "Office365FiddlerExtensionRuleset.dll",
                "Office365FiddlerExtensionRuleset.dll.config",
                "Office365FiddlerExtensionRuleset.pdb",
                "Office365FiddlerInspector.dll",
                "Office365FiddlerInspector.dll.config",
                "Office365FiddlerInspector.pdb",
                "Microsoft.ApplicationInsights.AspNetCore.dll",
                "Microsoft.ApplicationInsights.AspNetCore.xml",
                "Microsoft.ApplicationInsights.dll",
                "Microsoft.ApplicationInsights.pdb",
                "Microsoft.ApplicationInsights.xml",
                "EXOFiddlerInspector.dll",
                "EXOFiddlerInspector.dll.config",
                "EXOFiddlerInspector.pdb",
                "SessionClassification.json"];

            try
            {
                foreach (string file in extensionFiles)
                {
                    var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
                    /*
                    File extensionFile = $"{extensionSettings.ExtensionPath}\\{extensionFile}";

                    if (File.Exists(file))
                    {
                        file.Delete();
                    }
                    */
                }
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
        }

        private void InstallNewFiles()
        {

        }

    }
}
