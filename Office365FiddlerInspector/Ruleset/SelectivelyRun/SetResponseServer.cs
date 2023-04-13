﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class SetResponseServer : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        // Populate Response Server column.
        public void SetResponseServerData(Session session)
        {
            this.session = session;
            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Running SetResponseServer.");

            // Populate Response Server on session in order of preference from common to obsure.

            // If the response server header is not null or blank then populate it into the response server value.
            if ((this.session.oResponse["Server"] != null) && (this.session.oResponse["Server"] != ""))
            {
                this.session["X-ResponseServer"] = this.session.oResponse["Server"];
                getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "10");
            }
            // Else if the reponnse Host header is not null or blank then populate it into the response server value
            // Some traffic identifies a host rather than a response server.
            else if ((this.session.oResponse["Host"] != null && (this.session.oResponse["Host"] != "")))
            {
                this.session["X-ResponseServer"] = "Host: " + this.session.oResponse["Host"];
                getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "10");
            }
            // Else if the response PoweredBy header is not null or blank then populate it into the response server value.
            // Some Office 365 servers respond as X-Powered-By ASP.NET.
            else if ((this.session.oResponse["X-Powered-By"] != null) && (this.session.oResponse["X-Powered-By"] != ""))
            {
                this.session["X-ResponseServer"] = "X-Powered-By: " + this.session.oResponse["X-Powered-By"];
                getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "10");
            }
            // Else if the response X-Served-By header is not null or blank then populate it into the response server value.
            else if ((this.session.oResponse["X-Served-By"] != null && (this.session.oResponse["X-Served-By"] != "")))
            {
                this.session["X-ResponseServer"] = "X-Served-By: " + this.session.oResponse["X-Served-By"];
                getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "10");
            }
            // Else if the response X-Served-By header is not null or blank then populate it into the response server value.
            else if ((this.session.oResponse["X-Server-Name"] != null && (this.session.oResponse["X-Server-Name"] != "")))
            {
                this.session["X-ResponseServer"] = "X-Served-Name: " + this.session.oResponse["X-Server-Name"];
                getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "10");
            }
            else if ((this.session.isTunnel))
            {
                this.session["X-ResponseServer"] = session["X-SessionType"];
                getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "10");
            }
        }
    }
}