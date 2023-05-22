using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;

namespace Office365FiddlerExtension.Ruleset
{
    class ResponseServer : ActivationService
    {
        private static ResponseServer _instance;

        public static ResponseServer Instance => _instance ?? (_instance = new ResponseServer());

        public void SetResponseServer_Server(Session session)
        {
            this.session = session;

            // If the response server header is null or blank then return. Otherwise, populate it into the response server value.
            if ((this.session.oResponse["Server"] == null) || (this.session.oResponse["Server"] == ""))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} Running SetResponseServer_Server.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_Server",
                ResponseServer = this.session.oResponse["Server"],

                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        public void SetResponseServer_Host(Session session) 
        {
            this.session = session;

            // if the reponnse Host header is null or blank, return. Otherwise, populate it into the response server value.
            // Some traffic identifies a host rather than a response server.
            if ((this.session.oResponse["Host"] == null || (this.session.oResponse["Host"] == "")))
            {
                return;
            }

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_Host",
                ResponseServer = this.session.oResponse["Host"],

                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        public void SetResponseServer_PoweredBy(Session session) 
        {
            this.session = session;

            // if the response PoweredBy header is null or blank, return. Otherwise, populate it into the response server value.
            // Some servers respond as X-Powered-By ASP.NET.
            if ((this.session.oResponse["X-Powered-By"] == null) || (this.session.oResponse["X-Powered-By"] == ""))
            {
                return;
            }

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_PoweredBy",
                ResponseServer = this.session.oResponse["X-Powered-By"],

                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);            
        }

        public void SetResponseServer_ServedBy(Session session) 
        {
            this.session = session;

            // If the response X-Served-By header is null or blank, return. Otherwise, populate it into the response server value.
            if ((this.session.oResponse["X-Served-By"] == null || (this.session.oResponse["X-Served-By"] == "")))
            {
                return;
            }

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_ServedBy",
                ResponseServer = this.session.oResponse["X-Served-By"],

                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);            
        }

        public void SetResponseServer_ServerName(Session session) 
        {
            this.session = session;

            // If the response X-Served-By header is null or blank, return. Otherwise, populate it into the response server value.
            if ((this.session.oResponse["X-Server-Name"] == null || (this.session.oResponse["X-Server-Name"] == "")))
            {
                return;
            }

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_ServerName",
                ResponseServer = "X-Served-Name: " + this.session.oResponse["X-Server-Name"],

                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}