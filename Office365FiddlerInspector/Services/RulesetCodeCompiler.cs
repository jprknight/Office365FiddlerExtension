using Fiddler;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerInspector.Services
{
    internal class RulesetCodeCompiler : ActivationService
    {


        public string Output { get; private set; }

        // https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/compile-code-using-compiler

        public void CompileSomeCode(String rule)
        {
            this.session = session;



            //=======================
            string SourceString =
            @"if (GetSetSessionFlags.Instance.GetSessionTypeConfidenceLevel(this.session) == 10)
            {
                return;
            }

            // If the session hostname isn't outlook.office365.com and isn't MAPI traffic, return.
            if (!this.session.HostnameIs('outlook.office365.com') && (!this.session.uriContains('/mapi/emsmdb/?MailboxId=')))
            {
                return;
            }

            FiddlerApplication.Log.LogString('Office365FiddlerExtention: ' + this.session.id + ' HTTP 200 Outlook Exchange Online / Microsoft365 MAPI traffic.');

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, 'Green');
            GetSetSessionFlags.Instance.SetUITextColour(this.session, 'Black');

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, '200 OK');

            GetSetSessionFlags.Instance.SetSessionType(this.session, 'Outlook M365 MAPI');
            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, 'Outlook for Windows M365 MAPI traffic');
            GetSetSessionFlags.Instance.SetXResponseComments(this.session, 'This is normal Outlook MAPI over HTTP traffic to an Exchange Online / Microsoft365 mailbox.');

            // Possible something more to be found, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, '5');
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, '10');
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, '5')";

            //=======================

            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();

            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = Output;
            CompilerResults results = icc.CompileAssemblyFromSource(parameters, SourceString);

            //CompilerResults resultsFiles = icc.CompileAssemblyFromFileBatch(parameters, );
        }

        public void ReadRuleSetFiles()
        {
            // The first, is the directory of the executable of your application. Beware! It can be changed at runtime.
            //MessageBox.Show(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase));

            // The second, will be the directory of the assembly (.dll) you run the code from.
            // Assembly.GetEntryAssembly().Location

            // https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/file-io-operation

            string RuleSetDirectory = $"{Assembly.GetEntryAssembly().Location}\\Inspectors\\Ruleset";

            foreach (var file in RuleSetDirectory)
            {
                StreamReader reader = new StreamReader($"{RuleSetDirectory}\\{file}");
                try
                {
                    string rule = reader.ReadToEnd();
                    CompileSomeCode(rule);
                }
                catch (Exception ex)
                {

                    GetSetSessionFlags.Instance.WriteToFiddlerLogNoSession($"Attempting to read from RuleSetDirectory. Ran into issue: {ex}");
                }
            }

        }


        


    }
}
