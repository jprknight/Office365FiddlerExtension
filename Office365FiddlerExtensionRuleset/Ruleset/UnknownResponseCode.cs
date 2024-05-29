﻿using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class UnknownResponseCode
    {

        internal Session session { get; set; }

        private static UnknownResponseCode _instance;

        public static UnknownResponseCode Instance => _instance ?? (_instance = new UnknownResponseCode());

        /// <summary>
        /// Set session flags for any session which has an unknown / undefined response code in the extension.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            // Not setting colours on sessions not recognised.

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = LangHelper.GetString("Undefined"),

                SessionType = LangHelper.GetString("Undefined"),
                ResponseCodeDescription = LangHelper.GetString("Undefined"),
                ResponseAlert = LangHelper.GetString("Undefined"),
                ResponseComments = LangHelper.GetString("Response Comments No Known Issue"),

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0,
                SessionSeverity = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
