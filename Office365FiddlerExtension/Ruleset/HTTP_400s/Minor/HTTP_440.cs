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
    class HTTP_440 : ActivationService
    {
        private static HTTP_440 _instance;

        public static HTTP_440 Instance => _instance ?? (_instance = new HTTP_440());

        public void HTTP_440_IIS_Login_Timeout(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 440 IIS Login Time-out.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_440s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "440 IIS Login Time-out",
                ResponseCodeDescription = "440 IIS Login Time-out",
                ResponseAlert = "HTTP 440 IIS Login Time-out.",
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