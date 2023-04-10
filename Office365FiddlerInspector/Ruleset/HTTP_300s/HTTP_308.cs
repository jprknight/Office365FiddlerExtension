﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_308 : ActivationService
    {

        public void HTTP_308_Permenant_Redirect(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 308 Permanent Redirect (RFC 7538)";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 308 Permanent Redirect (RFC 7538).");

            session["X-ResponseCodeDescription"] = "308 Permanent Redirect (RFC 7538)";

            // Nothing meaningful here, let further processing try to pick up something.
            Preferences.SetSACL(session, "0");
            Preferences.SetSTCL(session, "0");
            Preferences.SetSRSCL(session, "0");
        }
    }
}