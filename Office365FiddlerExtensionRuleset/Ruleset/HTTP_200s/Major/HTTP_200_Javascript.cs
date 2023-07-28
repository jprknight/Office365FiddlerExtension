﻿using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_200_Javascript
    {
        internal Session session { get; set; }

        private static HTTP_200_Javascript _instance;

        public static HTTP_200_Javascript Instance => _instance ?? (_instance = new HTTP_200_Javascript());

        public void Run(Session session)
        {
            this.session = session;

            if (!this.session.ResponseHeaders["Content-Type"].Contains("javascript"))
            {
                return;
            }

            var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP200s|HTTP_200_Javascript");

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Javascript");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_Javascript",

                SessionType = "HTTP 200 OK with Javascript",
                ResponseCodeDescription = "HTTP 200 OK with Javascript.",
                ResponseAlert = "HTTP 200 OK with Javascript.",
                ResponseComments = "<p>HTTP 200 OK response with javascript.</p>",

                SessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel,
                SessionSeverity = sessionClassificationJson.SessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
