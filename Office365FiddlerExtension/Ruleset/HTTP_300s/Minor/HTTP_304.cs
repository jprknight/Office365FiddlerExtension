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
    class HTTP_304 : ActivationService
    {
        private static HTTP_304 _instance;

        public static HTTP_304 Instance => _instance ?? (_instance = new HTTP_304());

        public void HTTP_304_Not_Modified(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 304 Not modified.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_304s",
                UIBackColour = "Green",
                UITextColour = "Black",

                SessionType = "304 Not Modified (RFC 7232)",
                ResponseCodeDescription = "304 Not Modified (RFC 7232)",
                ResponseAlert = "304 Not Modified (RFC 7232).",
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