﻿using Fiddler;
using Newtonsoft.Json;
using System;
using Office365FiddlerExtensionRuleset.Services;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class ProcessName
    {
        internal Session session { get; set; }

        private static ProcessName _instance;

        public static ProcessName Instance => _instance ?? (_instance = new ProcessName());

        /// <summary>
        /// Set the process name. Used in the UI column and the response inspector.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
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
                // This won't be reflected in the ProcessName Column in the session list.
                // 3/26/2024 coded this out to set the X-ProcessName session flag on the session.
                // It got set correctly, even with a RefreshUI() the ProcessName is not shown in the column.
                // So this is just here for the inspector.
                // Typically, this is only really an issue for mobile device or remote captures anyway.
                ProcessName = RulesetLangHelper.GetString("Unknown");
            }

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = RulesetLangHelper.GetString("Process Name"),
                ProcessName = ProcessName
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
