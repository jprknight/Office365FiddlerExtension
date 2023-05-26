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
    class HTTP_302 : ActivationService
    {
        private static HTTP_302 _instance;

        public static HTTP_302 Instance => _instance ?? (_instance = new HTTP_302());

        public void HTTP_302_Redirect_AutoDiscover(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 302 AutoDiscover Found / Redirect.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_302s",
                UIBackColour = "Green",
                UITextColour = "Black",

                SessionType = "Autodiscover Redirect",
                ResponseCodeDescription = "302 Redirect / Found",
                ResponseAlert = "<b><span style='color:green'>Exchange Autodiscover redirect.</span></b>",
                ResponseComments = "This type of traffic is typically an Autodiscover redirect response from "
                    + "Exchange On-Premise sending the Outlook client to connect to Exchange Online.",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 5,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        public void HTTP_302_Redirect_AllOthers(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: " + this.session.id + " HTTP 302 Found / Redirect.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_302s",
                UIBackColour = "Green",
                UITextColour = "Black",

                SessionType = "Redirect",
                ResponseCodeDescription = "302 Redirect / Found",
                ResponseAlert = "<b><span style='color:green'>Redirect.</span></b>",
                ResponseComments = "Redirects within Office 365 client applications or servers are not unusual. "
                    + "The only potential downfall is too many of them. However if this happens you would normally see a too many "
                    + "redirects exception thrown as a server response.",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}