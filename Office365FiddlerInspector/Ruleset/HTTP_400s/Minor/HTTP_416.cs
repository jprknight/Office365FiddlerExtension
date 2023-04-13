﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_416 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        public void HTTP_416_Range_Not_Satisfiable(Session session)
        {
            this.session = session;

            this.session["X-ResponseAlert"] = "HTTP 416 Range Not Satisfiable (RFC 7233).";
            getSetSessionFlags.SetXResponseCommentsNoKnownIssue(this.session);

            this.session["X-ResponseCodeDescription"] = "416 Range Not Satisfiable (RFC 7233)";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 416 Range Not Satisfiable (RFC 7233).");

            // Nothing meaningful here, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}