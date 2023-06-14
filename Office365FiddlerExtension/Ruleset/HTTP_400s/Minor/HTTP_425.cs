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
    class HTTP_425 : ActivationService
    {
        private static HTTP_425 _instance;

        public static HTTP_425 Instance => _instance ?? (_instance = new HTTP_425());

        public void HTTP_425_Too_Early(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 425 Too Early (RFC 8470).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_425s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "425 Too Early (RFC 8470)",
                ResponseCodeDescription = "425 Too Early (RFC 8470)",
                ResponseAlert = "HTTP 425 Too Early (RFC 8470).",
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