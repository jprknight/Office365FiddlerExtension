﻿using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_0 : ActivationService
    {
        private static HTTP_0 _instance;

        public static HTTP_0 Instance => _instance ?? (_instance = new HTTP_0());

        public void HTTP_0_NoSessionResponse(Session session)
        {
            //  HTTP 0: No Response.

            this.Session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} HTTP 0 No response.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_0s",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "!NO RESPONSE!",
                ResponseServer = "!NO RESPONSE!",
                ResponseCodeDescription = "0 No Response",
                ResponseAlert = "<b><span style='color:red'>HTTP 0 - No Response</span></b>",
                ResponseComments = "The quantity of these types of server errors need to be considered in context with what you are "
                + "troubleshooting and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could "
                + "be cause for concern."
                + "<p>If you are not seeing expected client traffic, consider if network traces should be collected. Review if there is an underlying "
                + "network issue such as congestion on routers, which could be causing issues. The Network Connection Status Indicator (NCSI) on the "
                + "client computer might also be an area to investigate.</p>",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }   
    }
}