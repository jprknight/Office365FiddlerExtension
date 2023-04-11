﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_506
    {
        internal Session session { get; set; }
        public void HTTP_506_Variant_Also_Negociates(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 506 Variant Also Negotiates (RFC 2295).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 506 Variant Also Negotiates (RFC 2295).");

            session["X-ResponseCodeDescription"] = "506 Variant Also Negotiates (RFC 2295)";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
