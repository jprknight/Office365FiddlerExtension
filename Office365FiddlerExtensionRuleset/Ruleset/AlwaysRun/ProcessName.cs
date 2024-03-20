using Fiddler;
using Newtonsoft.Json;
using System;
using Office365FiddlerExtension.Services;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class ProcessName
    {
        internal Session session { get; set; }

        private static ProcessName _instance;

        public static ProcessName Instance => _instance ?? (_instance = new ProcessName());

        public void SetProcessName(Session session)
        {
            this.session = session;

            string ProcessName;
            // Set process name, split and exclude port used.
            if (session.LocalProcess != String.Empty)
            {
                ProcessName = session.LocalProcess.Split(':')[0];
            }
            // No local process to split.
            else
            {
                ProcessName = LangHelper.GetString("Remote Capture");
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = LangHelper.GetString("Process Name"),
                ProcessName = ProcessName
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
