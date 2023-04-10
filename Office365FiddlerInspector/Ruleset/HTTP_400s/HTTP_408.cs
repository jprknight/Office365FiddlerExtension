﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_408 : ActivationService
    {

        public void HTTP_408_Request_Timeout(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 408 Request Timeout.";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 408 Request Timeout.");

            session["X-ResponseCodeDescription"] = "408 Request Timeout";

            // Nothing meaningful here, let further processing try to pick up something.
            Preferences.SetSACL(session, "0");
            Preferences.SetSTCL(session, "0");
            Preferences.SetSRSCL(session, "0");
        }
    }
}
