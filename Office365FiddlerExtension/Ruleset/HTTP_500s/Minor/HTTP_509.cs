﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_509 : ActivationService
    {
        private static HTTP_509 _instance;

        public static HTTP_509 Instance => _instance ?? (_instance = new HTTP_509());

        public void HTTP_509_Bandwidth_Limit_Exceeeded(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 509 Bandwidth Limit Exceeded (Apache Web Server/cPanel).");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_509s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "509 Bandwidth Limit Exceeded (Apache Web Server/cPanel)",
                ResponseCodeDescription = "509 Bandwidth Limit Exceeded (Apache Web Server/cPanel)",
                ResponseAlert = "HTTP 509 Bandwidth Limit Exceeded (Apache Web Server/cPanel).",
                ResponseComments = SessionProcessor.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}