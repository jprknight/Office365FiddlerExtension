using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtension.Ruleset
{
    class ProcessName : ActivationService
    {
        private static ProcessName _instance;

        public static ProcessName Instance => _instance ?? (_instance = new ProcessName());

        public void SetProcessName(Session session)
        {
            this.session = session;

            string ProcessName = "";
            // Set process name, split and exclude port used.
            if (session.LocalProcess != String.Empty)
            {
                ProcessName = session.LocalProcess.Split(':')[0];
            }
            // No local process to split.
            else
            {
                ProcessName = "Remote Capture";
            }

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "ProcessName",
                ProcessName = ProcessName
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}
