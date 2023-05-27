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
    class HTTP_421 : ActivationService
    {
        private static HTTP_421 _instance;

        public static HTTP_421 Instance => _instance ?? (_instance = new HTTP_421());

        public void HTTP_421_Misdirected_Request(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 421 Misdirected Request (RFC 7540).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_421s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "421 Misdirected Request (RFC 7540)",
                ResponseCodeDescription = "421 Misdirected Request (RFC 7540)",
                ResponseAlert = "HTTP 421 Misdirected Request (RFC 7540).",
                ResponseComments = SessionFlagHandler.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}