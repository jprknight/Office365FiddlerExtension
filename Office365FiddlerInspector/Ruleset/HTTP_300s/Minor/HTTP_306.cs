﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_306 : ActivationService
    {
        private static HTTP_306 _instance;

        public static HTTP_306 Instance => _instance ?? (_instance = new HTTP_306());

        public void HTTP_306_Switch_Proxy(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 306 Switch Proxy.");

            // Setting to gray, to be convinced these are important to Microsoft 365 traffic.
            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Gray");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "306 Switch Proxy");

            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "HTTP 306 Switch Proxy");
            GetSetSessionFlags.Instance.SetXResponseCommentsNoKnownIssue(this.session);

            // Nothing meaningful here, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}