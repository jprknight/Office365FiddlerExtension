using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class SetResponseServer
    {
        internal Session session { get; set; }
        // Function where the Response Server column is populated.
        public void SetResponseServerData(Session session)
        {
            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " Running SetResponseServer.");

            // Populate Response Server on session in order of preference from common to obsure.

            // If the response server header is not null or blank then populate it into the response server value.
            if ((session.oResponse["Server"] != null) && (session.oResponse["Server"] != ""))
            {
                session["X-ResponseServer"] = session.oResponse["Server"];
                SessionProcessor.Instance.SetSRSCL(this.session, "10");
            }
            // Else if the reponnse Host header is not null or blank then populate it into the response server value
            // Some traffic identifies a host rather than a response server.
            else if ((session.oResponse["Host"] != null && (session.oResponse["Host"] != "")))
            {
                session["X-ResponseServer"] = "Host: " + session.oResponse["Host"];
                SessionProcessor.Instance.SetSRSCL(this.session, "10");
            }
            // Else if the response PoweredBy header is not null or blank then populate it into the response server value.
            // Some Office 365 servers respond as X-Powered-By ASP.NET.
            else if ((session.oResponse["X-Powered-By"] != null) && (session.oResponse["X-Powered-By"] != ""))
            {
                session["X-ResponseServer"] = "X-Powered-By: " + session.oResponse["X-Powered-By"];
                SessionProcessor.Instance.SetSRSCL(this.session, "10");
            }
            // Else if the response X-Served-By header is not null or blank then populate it into the response server value.
            else if ((session.oResponse["X-Served-By"] != null && (session.oResponse["X-Served-By"] != "")))
            {
                session["X-ResponseServer"] = "X-Served-By: " + session.oResponse["X-Served-By"];
                SessionProcessor.Instance.SetSRSCL(this.session, "10");
            }
            // Else if the response X-Served-By header is not null or blank then populate it into the response server value.
            else if ((session.oResponse["X-Server-Name"] != null && (session.oResponse["X-Server-Name"] != "")))
            {
                session["X-ResponseServer"] = "X-Served-Name: " + session.oResponse["X-Server-Name"];
                SessionProcessor.Instance.SetSRSCL(this.session, "10");
            }
            else if ((session.isTunnel))
            {
                session["X-ResponseServer"] = session["X-SessionType"];
                SessionProcessor.Instance.SetSRSCL(this.session, "10");
            }
        }
    }
}
