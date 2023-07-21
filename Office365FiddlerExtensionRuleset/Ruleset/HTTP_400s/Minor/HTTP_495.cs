﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_495
    {
        internal Session session { get; set; }

        private static HTTP_495 _instance;

        public static HTTP_495 Instance => _instance ?? (_instance = new HTTP_495());

        public void HTTP_495_SSL_Certificate_Error(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 495 nginx SSL Certificate Error.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_495s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "495 nginx SSL Certificate Error",
                ResponseCodeDescription = "495 nginx SSL Certificate Error",
                ResponseAlert = "HTTP 495 nginx SSL Certificate Error.",
                ResponseComments = SessionFlagService.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}