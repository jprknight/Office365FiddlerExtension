using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtension.Ruleset
{
    class ResponseServer : ActivationService
    {
        private static ResponseServer _instance;

        public static ResponseServer Instance => _instance ?? (_instance = new ResponseServer());

        public void SetResponseServer_Server(Session session)
        {
            this.Session = session;

            // If the response server header is null or blank then return. Otherwise, populate it into the response server value.
            if (this.Session.oResponse["Server"] == null)
            {
                return;
            }

            if (this.Session.oResponse["Server"] == "")
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} Running SetResponseServer_Server.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_Server",
                ResponseServer = this.Session.oResponse["Server"],

                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }

        public void SetResponseServer_Host(Session session) 
        {
            this.Session = session;

            // if the reponnse Host header is null or blank, return. Otherwise, populate it into the response server value.
            // Some traffic identifies a host rather than a response server.
            if (this.Session.oResponse["Host"] == null)
            {
                return;
            }

            if (this.Session.oResponse["Host"] == "")
            {
                return;
            }

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_Host",
                ResponseServer = this.Session.oResponse["Host"],

                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }

        public void SetResponseServer_PoweredBy(Session session) 
        {
            this.Session = session;

            // if the response PoweredBy header is null or blank, return. Otherwise, populate it into the response server value.
            // Some servers respond as X-Powered-By ASP.NET.
            if (this.Session.oResponse["X-Powered-By"] == null)
            {
                return;
            }

            if (this.Session.oResponse["X-Powered-By"] == "")
            {
                return;
            }

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_PoweredBy",
                ResponseServer = this.Session.oResponse["X-Powered-By"],

                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);            
        }

        public void SetResponseServer_ServedBy(Session session) 
        {
            this.Session = session;

            // If the response X-Served-By header is null or blank, return. Otherwise, populate it into the response server value.
            if (this.Session.oResponse["X-Served-By"] == null)
            {
                return;
            }

            if ((this.Session.oResponse["X-Served-By"] == ""))
            {
                return;
            }

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_ServedBy",
                ResponseServer = this.Session.oResponse["X-Served-By"],

                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);            
        }

        public void SetResponseServer_ServerName(Session session) 
        {
            this.Session = session;

            // If the response X-Served-By header is null or blank, return. Otherwise, populate it into the response server value.
            if (this.Session.oResponse["X-Server-Name"] == null)
            {
                return;
            }

            if (this.Session.oResponse["X-Server-Name"] == "")
            {
                return;
            }

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_ServerName",
                ResponseServer = "X-Server-Name: " + this.Session.oResponse["X-Server-Name"],

                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }

        public void SetResponseServer_Unknown(Session session)
        {
            this.Session = session;

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_Unknown",
                ResponseServer = "Type Unknown",

                SessionResponseServerConfidenceLevel = 1
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }
    }
}