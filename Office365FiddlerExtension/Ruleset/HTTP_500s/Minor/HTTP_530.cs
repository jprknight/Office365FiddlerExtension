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
    class HTTP_530 : ActivationService
    {
        private static HTTP_530 _instance;

        public static HTTP_530 Instance => _instance ?? (_instance = new HTTP_530());

        public void HTTP_530_Site_Is_Frozen(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 530 Site is frozen or Cloudflare Error returned with 1xxx error.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_530s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "530 Site is frozen or Cloudflare Error returned with 1xxx error",
                ResponseCodeDescription = "530 Site is frozen or Cloudflare Error returned with 1xxx error",
                ResponseAlert = "HTTP 530 Site is frozen or Cloudflare Error returned with 1xxx error.",
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