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

namespace Office365FiddlerExtension.Handler
{
    class RuleSetHandler
    {
        internal Session Session { get; set; }

        private static RuleSetHandler _instance;

        public static RuleSetHandler Instance => _instance ?? (_instance = new RuleSetHandler());

        public static void RunRuleSet(Session Session)
        {
            string pattern = "Office365FiddlerExtensionRuleset_*.dll";
            var dirInfo = new DirectoryInfo(SettingsHandler.AssemblyDirectory);
            FiddlerApplication.Log.LogString($"Assembly Location: {SettingsHandler.AssemblyDirectory}");
            FiddlerApplication.Log.LogString($"Session id: {Session.id}, Session ResponseCode: {Session.responseCode}");

            try
            {
                FileInfo file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} {file}");

                Assembly rulesetDDL = Assembly.LoadFile(file.FullName);

                // 
                var type = rulesetDDL.GetType("Ruleset.FiddlerUpdateSessions");
                
                var obj = Activator.CreateInstance(type);

                var method = type.GetMethod("FUS");

                method.Invoke(obj, new object[] { Session });
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} {ex}");
            }


        }

        public void RunAssembly(Session Session)
        {
            this.Session = Session;

            Microsoft.CSharp.CSharpCodeProvider foo = new Microsoft.CSharp.CSharpCodeProvider();

            var res = foo.CompileAssemblyFromSource(
                new System.CodeDom.Compiler.CompilerParameters()
                {
                    GenerateInMemory = true
                },
                GetSessionRule()
            );

            var type = res.CompiledAssembly.GetType("FiddlerUpdateSessions");

            dynamic dyn = Activator.CreateInstance(type);
            dyn.Execute(this.Session);

            //var assembly = GetSessionRuleCompiledAssembly();

            // https://stackoverflow.com/questions/10613728/run-dynamically-compiled-c-sharp-code-at-native-speed-how

            // https://stackoverflow.com/questions/234217/is-it-possible-to-compile-and-execute-new-code-at-runtime-in-net
            /*string sourceCode = @"
                public class SomeClass {
                    public int Add42 (int parameter) {
                        return parameter += 42;
                    }
                }";
            var compParms = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = true
            };
            var csProvider = new CSharpCodeProvider();
            CompilerResults compilerResults =
                csProvider.CompileAssemblyFromSource(compParms, GetSessionRule());
            object typeInstance =
                compilerResults.CompiledAssembly.CreateInstance("FiddlerUpdateSessions");
            MethodInfo mi = typeInstance.GetType().GetMethod("FUS");

            mi.Invoke(null, new object[] { this.Session });
            */
            //int methodOutput =
            //    (int)mi.Invoke(typeInstance, new object[] { 1 });
            //Console.WriteLine(methodOutput);
            //Console.ReadLine();

            //Type myType = assembly.GetTypes()[0];
            //MethodInfo method = myType.GetMethod("FUS");
            //object myInstance = Activator.CreateInstance(myType);
            //method.Invoke(myInstance, null);

            //GetSessionRuleCompiledAssembly getSessionRuleCompiledAssembly = new GetSessionRuleCompiledAssembly.GetCompiledAssembly();

            //var assembly = result.CompiledAssembly;



            //dynamic inst = assembly.CreateInstance("FiddlerUpdateSessions");
            //string methResult = inst.HelloWorld("FUS") as string;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): POC");
        }

        private string GetSessionRule()
        {
            var Base64Source = "dXNpbmcgRmlkZGxlcjsKdXNpbmcgT2ZmaWNlMzY1RmlkZGxlckV4dGVuc2lvbi5TZXJ2aWNlczsKdXNpbmcgTmV3dG9uc29mdC5Kc29uOwoKbmFtZXNwYWNlIE9mZmljZTM2NUZpZGRsZXJFeHRlbnNpb24uUnVsZXNldAp7CiAgICBjbGFzcyBGaWRkbGVyVXBkYXRlU2Vzc2lvbnMKICAgIHsKICAgICAgICBpbnRlcm5hbCBTZXNzaW9uIFNlc3Npb24geyBnZXQ7IHNldDsgfQoKICAgICAgICBwdWJsaWMgdm9pZCBGVVMoU2Vzc2lvbiBzZXNzaW9uKQogICAgICAgIHsKICAgICAgICAgICAgdGhpcy5TZXNzaW9uID0gc2Vzc2lvbjsKCiAgICAgICAgICAgIGlmICh0aGlzLlNlc3Npb24uaG9zdG5hbWUgPT0gInd3dy5maWRkbGVyMi5jb20iICYmIHRoaXMuU2Vzc2lvbi51cmlDb250YWlucygiVXBkYXRlQ2hlY2suYXNweCIpKQogICAgICAgICAgICB7CiAgICAgICAgICAgICAgICB2YXIgc2Vzc2lvbkZsYWdzID0gbmV3IFNlc3Npb25GbGFnSGFuZGxlci5FeHRlbnNpb25TZXNzaW9uRmxhZ3MoKQogICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgIFNlY3Rpb25UaXRsZSA9ICJCcm9hZCBMb2dpYyBDaGVja3MiLAogICAgICAgICAgICAgICAgICAgIFVJQmFja0NvbG91ciA9ICJHcmF5IiwKICAgICAgICAgICAgICAgICAgICBVSVRleHRDb2xvdXIgPSAiQmxhY2siLAoKICAgICAgICAgICAgICAgICAgICBTZXNzaW9uVHlwZSA9ICJGaWRkbGVyIFVwZGF0ZSBDaGVjayIsCiAgICAgICAgICAgICAgICAgICAgUmVzcG9uc2VTZXJ2ZXIgPSAiRmlkZGxlciBVcGRhdGUgQ2hlY2siLAogICAgICAgICAgICAgICAgICAgIFJlc3BvbnNlQWxlcnQgPSAiRmlkZGxlciBVcGRhdGUgQ2hlY2siLAogICAgICAgICAgICAgICAgICAgIFJlc3BvbnNlQ29kZURlc2NyaXB0aW9uID0gIkZpZGRsZXIgVXBkYXRlIENoZWNrIiwKICAgICAgICAgICAgICAgICAgICBSZXNwb25zZUNvbW1lbnRzID0gIlRoaXMgaXMgRmlkZGxlciBpdHNlbGYgY2hlY2tpbmcgZm9yIHVwZGF0ZXMuIEl0IGhhcyBub3RoaW5nIHRvIGRvIHdpdGggdGhlIE9mZmljZSAzNjUgRmlkZGxlciBFeHRlbnNpb24uIiwKICAgICAgICAgICAgICAgICAgICBBdXRoZW50aWNhdGlvbiA9ICJGaWRkbGVyIFVwZGF0ZSBDaGVjayIsCgogICAgICAgICAgICAgICAgICAgIFNlc3Npb25BdXRoZW50aWNhdGlvbkNvbmZpZGVuY2VMZXZlbCA9IDEwLAogICAgICAgICAgICAgICAgICAgIFNlc3Npb25UeXBlQ29uZmlkZW5jZUxldmVsID0gMTAsCiAgICAgICAgICAgICAgICAgICAgU2Vzc2lvblJlc3BvbnNlU2VydmVyQ29uZmlkZW5jZUxldmVsID0gMTAKICAgICAgICAgICAgICAgIH07CgogICAgICAgICAgICAgICAgdmFyIHNlc3Npb25GbGFnc0pzb24gPSBKc29uQ29udmVydC5TZXJpYWxpemVPYmplY3Qoc2Vzc2lvbkZsYWdzKTsKICAgICAgICAgICAgICAgIFNlc3Npb25GbGFnSGFuZGxlci5JbnN0YW5jZS5VcGRhdGVTZXNzaW9uRmxhZ0pzb24odGhpcy5TZXNzaW9uLCBzZXNzaW9uRmxhZ3NKc29uKTsKICAgICAgICAgICAgfQogICAgICAgIH0KICAgIH0KfQ==";

            return Base64Decode(Base64Source);
        }

        public Assembly CompileRuleset()
        {

            // https://stackoverflow.com/questions/63215725/how-to-call-a-method-from-an-external-assembly

            var Base64Source = "dXNpbmcgRmlkZGxlcjsKdXNpbmcgT2ZmaWNlMzY1RmlkZGxlckV4dGVuc2lvbi5TZXJ2aWNlczsKdXNpbmcgTmV3dG9uc29mdC5Kc29uOwp1c2luZyBTeXN0ZW0uUmVmbGVjdGlvbjsKCm5hbWVzcGFjZSBSdWxlc2V0CnsKICAgIGNsYXNzIEZpZGRsZXJVcGRhdGVTZXNzaW9ucwogICAgewogICAgICAgIGludGVybmFsIFNlc3Npb24gU2Vzc2lvbiB7IGdldDsgc2V0OyB9CgogICAgICAgIHB1YmxpYyB2b2lkIEZVUyhTZXNzaW9uIHNlc3Npb24pCiAgICAgICAgewogICAgICAgICAgICB0aGlzLlNlc3Npb24gPSBzZXNzaW9uOwoKICAgICAgICAgICAgaWYgKHRoaXMuU2Vzc2lvbi5ob3N0bmFtZSA9PSAid3d3LmZpZGRsZXIyLmNvbSIgJiYgdGhpcy5TZXNzaW9uLnVyaUNvbnRhaW5zKCJVcGRhdGVDaGVjay5hc3B4IikpCiAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgIEZpZGRsZXJBcHBsaWNhdGlvbi5Mb2cuTG9nU3RyaW5nKEFzc2VtYmx5LkdldEV4ZWN1dGluZ0Fzc2VtYmx5KCkuR2V0TmFtZSgpLk5hbWUgCiAgICAgICAgICAgICAgICAgICAgKyB0aGlzLkdldFR5cGUoKS5OYW1lICsgdGhpcy5TZXNzaW9uLmlkICsgIiBGaWRkbGVyIFVwZGF0ZXMuIik7CgogICAgICAgICAgICAgICAgdmFyIHNlc3Npb25GbGFncyA9IG5ldyBTZXNzaW9uRmxhZ0hhbmRsZXIuRXh0ZW5zaW9uU2Vzc2lvbkZsYWdzKCkKICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICBTZWN0aW9uVGl0bGUgPSAiQnJvYWQgTG9naWMgQ2hlY2tzIiwKICAgICAgICAgICAgICAgICAgICBVSUJhY2tDb2xvdXIgPSAiR3JheSIsCiAgICAgICAgICAgICAgICAgICAgVUlUZXh0Q29sb3VyID0gIkJsYWNrIiwKCiAgICAgICAgICAgICAgICAgICAgU2Vzc2lvblR5cGUgPSAiRmlkZGxlciBVcGRhdGUgQ2hlY2siLAogICAgICAgICAgICAgICAgICAgIFJlc3BvbnNlU2VydmVyID0gIkZpZGRsZXIgVXBkYXRlIENoZWNrIiwKICAgICAgICAgICAgICAgICAgICBSZXNwb25zZUFsZXJ0ID0gIkZpZGRsZXIgVXBkYXRlIENoZWNrIiwKICAgICAgICAgICAgICAgICAgICBSZXNwb25zZUNvZGVEZXNjcmlwdGlvbiA9ICJGaWRkbGVyIFVwZGF0ZSBDaGVjayIsCiAgICAgICAgICAgICAgICAgICAgUmVzcG9uc2VDb21tZW50cyA9ICJUaGlzIGlzIEZpZGRsZXIgaXRzZWxmIGNoZWNraW5nIGZvciB1cGRhdGVzLiBJdCBoYXMgbm90aGluZyB0byBkbyB3aXRoIHRoZSBPZmZpY2UgMzY1IEZpZGRsZXIgRXh0ZW5zaW9uLiIsCiAgICAgICAgICAgICAgICAgICAgQXV0aGVudGljYXRpb24gPSAiRmlkZGxlciBVcGRhdGUgQ2hlY2siLAoKICAgICAgICAgICAgICAgICAgICBTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwgPSAxMCwKICAgICAgICAgICAgICAgICAgICBTZXNzaW9uVHlwZUNvbmZpZGVuY2VMZXZlbCA9IDEwLAogICAgICAgICAgICAgICAgICAgIFNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCA9IDEwCiAgICAgICAgICAgICAgICB9OwoKICAgICAgICAgICAgICAgIHZhciBzZXNzaW9uRmxhZ3NKc29uID0gSnNvbkNvbnZlcnQuU2VyaWFsaXplT2JqZWN0KHNlc3Npb25GbGFncyk7CiAgICAgICAgICAgICAgICBTZXNzaW9uRmxhZ0hhbmRsZXIuSW5zdGFuY2UuVXBkYXRlU2Vzc2lvbkZsYWdKc29uKHRoaXMuU2Vzc2lvbiwgc2Vzc2lvbkZsYWdzSnNvbik7CiAgICAgICAgICAgIH0KICAgICAgICB9CiAgICB9Cn0=";

            string SourceString = Base64Decode(Base64Source);

            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();

            // https://learn.microsoft.com/en-us/dotnet/api/system.codedom.compiler.compilerparameters.referencedassemblies?view=windowsdesktop-7.0

            parameters.ReferencedAssemblies.Add($"{SettingsHandler.AssemblyDirectory}\\Fiddler.exe");
            parameters.ReferencedAssemblies.Add($"{SettingsHandler.AssemblyDirectory}\\Office365FiddlerExtension.dll");
            parameters.ReferencedAssemblies.Add($"{SettingsHandler.AssemblyDirectory}\\Newtonsoft.Json.dll");
            parameters.GenerateExecutable = false;
            //parameters.GenerateInMemory = true;
            parameters.OutputAssembly = $"{SettingsHandler.AssemblyDirectory}\\" +
                $"Office365FiddlerExtensionRuleset_{DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss")}.dll";

            CompilerResults results = CodeDomProvider.CreateProvider("CSharp").CompileAssemblyFromSource(parameters, SourceString);

            if (results.Errors.Count > 0)
            {
                foreach (CompilerError CompErr in results.Errors)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}):" +
                        $"Line number {CompErr.Line}, Error Number: {CompErr.ErrorNumber}, ' {CompErr.ErrorText};");
                }
                return null;
            }
            else
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Success!");

                return results.CompiledAssembly;
            }
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        // Check the ruleset version stored in local settings is older than the version in the Github repo.
        // If it's newer call the <to be named> function to pull down updates.
        public async void RulesetVersionCheck()
        {
            var ExtensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            var URLs = URLsHandler.Instance.GetDeserializedExtensionURLs();

            if (ExtensionSettings.NeverWebCall)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): DisableWebCalls is enabled, no ruleset update check performed.");
                return;
            }

            if (DateTime.Now < ExtensionSettings.LocalMasterRulesetLastUpdated) 
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Rules have been checked within the last 24 hours, no ruleset update check performed.");
                return;
            }

            #region RulesetVersionCheck
            // Pull the version file to see if there is a version ruleset to update on.
            using (var versionCheck = new HttpClient())
            {
                try
                {
                    var response = await versionCheck.GetAsync("https://somedummyurlwhichwontwork");
                    // If we're running the beta ruleset, look to the Fiddler application preference for the URL to go to for the rulesetVersion file.
                    // This will likely be a rolling URL based on the branch name used at any time.
                    if (ExtensionSettings.UseBetaRuleSet)
                    {
                        response = await versionCheck.GetAsync(URLs.BetaRuleSet);
                    }
                    // Here we're not using the beta ruleset, so pull it from the master branch.
                    else
                    {
                        response = await versionCheck.GetAsync(URLs.MasterRuleSet);
                    }

                    //var response = await versionCheck.GetAsync("https://raw.githubusercontent.com/username/repo/master/file.json");
                    response.EnsureSuccessStatusCode();

                    var jsonString = string.Empty;

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        //Rules = await JsonSerializer.DeserializeAsync<Dictionary<string, Rule>>(stream);

                        stream.Position = 0;
                        using (var reader = new StreamReader(stream))
                        {
                            jsonString = await reader.ReadToEndAsync();
                        }

                        // jsonString came back as empty.
                        if (jsonString == null)
                        {
                            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error retrieving ruleset from Github: jsonString null.");
                        }
                        // jsonString has something in it. See if the version value on Github is newer than what we have stored locally.
                        else
                        {
                            // REVIEW THIS
                            // There's a newer ruleset published the the Github repo.
                            /*if (int.Parse(jsonString) >= int.Parse(Properties.Settings.Default.LocalMasterRulesetLastUpdated))
                            {
                                GetSetSessionFlags.Instance.WriteToFiddlerLogNoSession($"Local ruleset is behind Github ruleset.");

                                // Call a function here which does:
                                // * Pulls all rule files, decodes them and looks for differences, if differences are found store in local file overwriting any existing content.
                                // * Or checks each rule from something different and if different store in local file overwriting any existing content.
                                // * Assuming all the above finished without issue, bump the ruleset version number in Settings.
                            }
                            // There's not a newer ruleset published to the Github repo.
                            else
                            {
                                GetSetSessionFlags.Instance.WriteToFiddlerLogNoSession($"Local ruleset is up to date with Github ruleset.");
                                return;
                            }*/

                        }
                    }
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error retrieving ruleset from Github {ex}.");
                }
            }
            #endregion
        }

        //private static readonly Lazy<RulesetSingleton> _instance = new Lazy<RulesetSingleton>(() => new RulesetSingleton());
        //public static RulesetSingleton Ruleset => _instance.Value;
        
        //static async Task Main(string[] args)
        //{
            /*
            Settings file will have this already:
            eyJLQjIzMjkzODMiOnsiUnVsZVR5cGUiOjMsIlJ1bGVSZWdleCI6IiQqLlswLTldezR9Ki5eIiwiUnVsZVJlc3VsdCI6IlRoaXMgaXMgYSBidWcgeW8iLCJSdWxlQWN0aW9uIjozfSwiTmV3IEZlYXR1cmUgTVMzNDkzODQiOnsiUnVsZVR5cGUiOjAsIlJ1bGVSZWdleCI6IiQqLlthLXpBLVpdezh9Ki5eIiwiUnVsZVJlc3VsdCI6IlRoaXMgaXMgYSBuZXcgZmVhdHVyZSB5byIsIlJ1bGVBY3Rpb24iOjJ9fQ==

            Which decodes to:
            {
                "KB2329383": {
                    "RuleType": 3,
                    "RuleRegex": "$*.[0-9]{4}*.^",
                    "RuleResult": "This is a bug yo",
                    "RuleAction": 3
                },
                "New Feature MS349384": {
                    "RuleType": 0,
                    "RuleRegex": "$*.[a-zA-Z]{8}*.^",
                    "RuleResult": "This is a new feature yo",
                    "RuleAction": 2
                }
            }*/
            
            /*
            // Example of how to programatically create rules and serialize into json:
            var sampleJson = CreateSampleRules();
            Console.WriteLine($"Sample json:{Environment.NewLine}{sampleJson}");

            // Pull rules from settings happens in the lazy initialize
            Console.WriteLine($"Current ruleset contains {Ruleset.Rules.Count} rules.");

            // Update settings from github
            Console.WriteLine("Updating rules from GitHub...");
            await Ruleset.UpdateRulesAsync();
            Console.WriteLine($"Updated ruleset contains {Ruleset.Rules.Count} rules.");
            */
        //}
    /*
        static string CreateSampleRules()
        {
            var Ruleset = new Dictionary<string, Rule>();

            Ruleset.Add("KB2329383", new Rule()
            {
                RuleType = RuleType.Bug,
                RuleRegex = "$*.[0-9]{4}*.^",
                RuleResult = "This is a bug yo",
                RuleAction = RuleAction.Flag | RuleAction.Link | RuleAction.Note
            });

            Ruleset.Add("New Feature MS349384", new Rule()
            {
                RuleType = RuleType.Informational,
                RuleRegex = "$*.[a-zA-Z]{8}*.^",
                RuleResult = "This is a new feature yo",
                RuleAction = RuleAction.Note
            });

            // Serialize the ruleset to JSON
            return JsonSerializer.Serialize(Ruleset);
        }
    }

    public class RulesetSingleton
    {
        public Dictionary<string, Rule> Rules = new Dictionary<string, Rule>();

        internal RulesetSingleton()
        {
            // Pull default rules from settings
            var defaultRulesetBytes = Convert.FromBase64String(Properties.Settings.Default.DefaultRuleset);
            var defaultRulesetJson = Encoding.UTF8.GetString(defaultRulesetBytes);
            Rules = JsonSerializer.Deserialize<Dictionary<string, Rule>>(defaultRulesetJson);
        }

        public async Task UpdateRulesAsync()
        {
            // Shutdown the web calls before they begin if the preference is set.
            if (Preferences.DisableWebCalls)
            {
                GetSetSessionFlags.Instance.WriteToFiddlerLogNoSession("Web calls disabled. Not looking for ruleset updates.");
                return;
            }

            // Think about writing some code so ruleset checks are only done once per 24/48/72/168 hours.
            // Ruleset won't change that frequently, so there's probably no reason to call out to the Github repro on every Fiddler start.

           

            // Use the beta ruleset.
            if (Properties.Settings.Default.UseBetaRuleSet)
            {

            }
            // Use the master / normal / production ruleset.
            else
            {

            }

            using (var httpClient = new HttpClient())
            {
                try
                {
                    // You may want to post a separate version file so you can check if the version is newer first
                    // Alternatively if you can assume the file size is different you can pass HttpMethod.Head as the method
                    // and then pull the Content-Length from response.Headers and compare to the non-Base64 size

                    var response = await httpClient.GetAsync("https://raw.githubusercontent.com/username/repo/master/file.json");
                    response.EnsureSuccessStatusCode();

                    // You can do this two ways, but probably should do it the async way as your ruleset could get large
                    var async = true;
                    var jsonString = string.Empty;

                    // Synchronous way (not recommended but simpler, blocks thread, higher memory footprint while deserializing)
                    if (!async)
                    {
                        jsonString = await response.Content.ReadAsStringAsync();

                        // Deserialize JSON and overwrite the ruleset.  You may want to merge or do something else
                        Rules = JsonSerializer.Deserialize<Dictionary<string, Rule>>(jsonString);
                    }
                    else
                    {
                        // More complex, can probably be optimized but if you end up with a large ruleset
                        // you want to do the http get and the deserialization asynchronously
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {
                            Rules = await JsonSerializer.DeserializeAsync<Dictionary<string, Rule>>(stream);

                            // Get the string for storage
                            stream.Position = 0;
                            using (var reader = new StreamReader(stream))
                            {
                                jsonString = await reader.ReadToEndAsync();
                            }
                        }
                    }

                    // Save base64 version to settings as the new default
                    Properties.Settings.Default.DefaultRuleset = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonString));
                }
                catch (Exception ex)
                {
                    GetSetSessionFlags.Instance.WriteToFiddlerLogNoSession($"Error retrieving ruleset from Github {ex}");
                }
            }
        }

        public enum RuleType
        {
            Informational,
            Security,
            Performance,
            Bug,
            Unknown
        }
        public enum RuleAction
        {
            Ignore,
            Link,
            Note,
            Flag,
            WhoKnows
        }

        public class Rule
        {
            public RuleType RuleType { get; set; }
            public string RuleRegex { get; set; }
            public string RuleResult { get; set; }
            public RuleAction RuleAction { get; set; }
        }
    */
    }
}