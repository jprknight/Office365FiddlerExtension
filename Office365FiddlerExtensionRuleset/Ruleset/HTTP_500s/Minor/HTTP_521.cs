﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtensionRuleset.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_521
    {
        internal Session session { get; set; }

        private static HTTP_521 _instance;

        public static HTTP_521 Instance => _instance ?? (_instance = new HTTP_521());

        public void HTTP_521_Web_Server_Is_Down(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 521 Cloudflare Web Server Is Down.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_521s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "521 Cloudflare Web Server Is Down",
                ResponseCodeDescription = "521 Cloudflare Web Server Is Down",
                ResponseAlert = "HTTP 521 Cloudflare Web Server Is Down.",
                ResponseComments = SessionFlagHandler.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}