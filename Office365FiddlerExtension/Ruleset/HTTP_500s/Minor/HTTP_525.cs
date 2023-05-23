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
    class HTTP_525 : ActivationService
    {
        private static HTTP_525 _instance;

        public static HTTP_525 Instance => _instance ?? (_instance = new HTTP_525());

        public void HTTP_525_SSL_Handshake_Failed(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 525 Cloudflare SSL Handshake Failed.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_525s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "525 Cloudflare SSL Handshake Failed",
                ResponseCodeDescription = "525 Cloudflare SSL Handshake Failed",
                ResponseAlert = "HTTP 525 Cloudflare SSL Handshake Failed.",
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