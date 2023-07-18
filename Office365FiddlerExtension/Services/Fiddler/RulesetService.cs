using Fiddler;
using FiddlerCore.Utilities.SmartAssembly.Attributes;
using Microsoft.CSharp;
using Microsoft.Win32;
using Office365FiddlerExtension.Properties;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.Extensions.FileSystemGlobbing.Internal;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Class to call the external rulset DLL file, passing in the session for processing.
    /// </summary>
    class RulesetService
    {
        public static void RunRuleSet(Session session)
        {
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

                method.Invoke(obj, new object[] { session });
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} {ex}");
            }
        }
    }
}