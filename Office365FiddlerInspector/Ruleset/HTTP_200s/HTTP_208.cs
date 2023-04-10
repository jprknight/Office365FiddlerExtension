﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_208
    {

        public void HTTP_208_Already_Reported(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 208 Already Reported (WebDAV; RFC 5842).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 208 Already Reported (WebDAV; RFC 5842).");

            session["X-ResponseCodeDescription"] = "208 Already Reported (WebDAV; RFC 5842)";

            // Nothing meaningful here, let further processing try to pick up something.
            Preferences.SetSACL(session, "0");
            Preferences.SetSTCL(session, "0");
            Preferences.SetSRSCL(session, "0");
        }
    }
}