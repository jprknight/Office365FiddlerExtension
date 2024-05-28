using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class ResponseServer
    {
        internal Session session { get; set; }

        private static ResponseServer _instance;

        public static ResponseServer Instance => _instance ?? (_instance = new ResponseServer());

        /// <summary>
        /// Set the response server, run towards end of ruleset processing as a final catch all.
        /// Used by the UI column and response inspector.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            SetResponseServer_Server(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionResponseServerConfidenceLevel_Ten(this.session))
            {
                return;
            }

            SetResponseServer_Host(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionResponseServerConfidenceLevel_Ten(this.session))
            {
                return;
            }

            SetResponseServer_PoweredBy(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionResponseServerConfidenceLevel_Ten(this.session))
            {
                return;
            }

            SetResponseServer_ServedBy(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionResponseServerConfidenceLevel_Ten(this.session))
            {
                return;
            }

            SetResponseServer_ServerName(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionResponseServerConfidenceLevel_Ten(this.session))
            {
                return;
            }

            SetResponseServer_Akami(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionResponseServerConfidenceLevel_Ten(this.session))
            {
                return;
            }

            SetResponseServer_Unknown(this.session);
        }

        private void SetResponseServer_Server(Session session)
        {
            this.session = session;

            // If the response server header is null or blank then return. Otherwise, populate it into the response server value.
            if (this.session.oResponse["Server"] == null)
            {
                return;
            }

            if (this.session.oResponse["Server"] == "")
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running SetResponseServer_Server.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_Server",

                ResponseServer = this.session.oResponse["Server"],

                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        private void SetResponseServer_Host(Session session) 
        {
            this.session = session;

            // if the reponnse Host header is null or blank, return. Otherwise, populate it into the response server value.
            // Some traffic identifies a host rather than a response server.
            if (this.session.oResponse["Host"] == null)
            {
                return;
            }

            if (this.session.oResponse["Host"] == "")
            {
                return;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_Host",

                ResponseServer = this.session.oResponse["Host"],
                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        private void SetResponseServer_PoweredBy(Session session) 
        {
            this.session = session;

            // if the response PoweredBy header is null or blank, return. Otherwise, populate it into the response server value.
            // Some servers respond as X-Powered-By ASP.NET.
            if (this.session.oResponse["X-Powered-By"] == null)
            {
                return;
            }

            if (this.session.oResponse["X-Powered-By"] == "")
            {
                return;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_PoweredBy",

                ResponseServer = this.session.oResponse["X-Powered-By"],
                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);            
        }

        private void SetResponseServer_ServedBy(Session session) 
        {
            this.session = session;

            // If the response X-Served-By header is null or blank, return. Otherwise, populate it into the response server value.
            if (this.session.oResponse["X-Served-By"] == null)
            {
                return;
            }

            if ((this.session.oResponse["X-Served-By"] == ""))
            {
                return;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_ServedBy",

                ResponseServer = this.session.oResponse["X-Served-By"],
                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);            
        }

        private void SetResponseServer_ServerName(Session session) 
        {
            this.session = session;

            // If the response X-Served-By header is null or blank, return. Otherwise, populate it into the response server value.
            if (this.session.oResponse["X-Server-Name"] == null)
            {
                return;
            }

            if (this.session.oResponse["X-Server-Name"] == "")
            {
                return;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_ServerName",

                ResponseServer = this.session.oResponse["X-Server-Name"],
                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        private void SetResponseServer_Akami(Session session)
        {
            this.session = session;

            if (this.session.oResponse["X-CDN-Provider"] != "Akamai")
            {
                return;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_Akami",

                ResponseServer = "CDN:Akami",
                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        private void SetResponseServer_Unknown(Session session)
        {
            this.session = session;

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_Unknown",

                ResponseServer = LangHelper.GetString("ResponseServer_Unknown"),
                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
