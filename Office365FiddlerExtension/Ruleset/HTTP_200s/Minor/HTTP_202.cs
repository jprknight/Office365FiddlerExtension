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
    class HTTP_202 : ActivationService
    {
        private static HTTP_202 _instance;

        public static HTTP_202 Instance => _instance ?? (_instance = new HTTP_202());

        public void HTTP_202_Accepted(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 202 Accepted.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_202s",
                UIBackColour = "Green",
                UITextColour = "Black",

                SessionType = "",
                ResponseCodeDescription = "202 Accepted",
                ResponseAlert = "202 Accepted.",
                ResponseComments = SessionFlagHandler.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }
    }
}