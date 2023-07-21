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
    class HTTP_523
    {
        internal Session session { get; set; }

        private static HTTP_523 _instance;

        public static HTTP_523 Instance => _instance ?? (_instance = new HTTP_523());

        public void HTTP_523_Origin_Is_Unreachable(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 523 Cloudflare Origin Is Unreachable.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_523s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "523 Cloudflare Origin Is Unreachable",
                ResponseCodeDescription = "523 Cloudflare Origin Is Unreachable",
                ResponseAlert = "HTTP 523 Cloudflare Origin Is Unreachable.",
                ResponseComments = SessionFlagService.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}