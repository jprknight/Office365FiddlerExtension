﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_523 : ActivationService
    {
        private static HTTP_523 _instance;

        public static HTTP_523 Instance => _instance ?? (_instance = new HTTP_523());

        public void HTTP_523_Origin_Is_Unreachable(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} HTTP 523 Cloudflare Origin Is Unreachable.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_523s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "523 Cloudflare Origin Is Unreachable",
                ResponseCodeDescription = "523 Cloudflare Origin Is Unreachable",
                ResponseAlert = "HTTP 523 Cloudflare Origin Is Unreachable.",
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