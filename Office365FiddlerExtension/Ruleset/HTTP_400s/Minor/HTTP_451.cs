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
    class HTTP_451 : ActivationService
    {
        private static HTTP_451 _instance;

        public static HTTP_451 Instance => _instance ?? (_instance = new HTTP_451());

        public void HTTP_451_Unavailable_For_Legal_Reasons_or_IIS_Redirect(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 451 Unavailable For Legal Reasons (RFC 7725) or 451 IIS Redirect.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "451 Unavailable For Legal Reasons (RFC 7725) or 451 IIS Redirect",
                ResponseCodeDescription = "451 Unavailable For Legal Reasons (RFC 7725) or 451 IIS Redirect",
                ResponseAlert = "HTTP 451 Unavailable For Legal Reasons (RFC 7725) or 451 IIS Redirect.",
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