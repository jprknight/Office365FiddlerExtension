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
    class HTTP_413 : ActivationService
    {
        private static HTTP_413 _instance;

        public static HTTP_413 Instance => _instance ?? (_instance = new HTTP_413());

        public void HTTP_413_Payload_Too_Large(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 413 Payload Too Large (RFC 7231).");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_413s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "413 Payload Too Large (RFC 7231)",
                ResponseCodeDescription = "413 Payload Too Large (RFC 7231)",
                ResponseAlert = "HTTP 413 Payload Too Large (RFC 7231).",
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