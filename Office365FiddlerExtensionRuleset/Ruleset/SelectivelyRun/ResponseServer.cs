using Office365FiddlerExtensionRuleset.Services;
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

            SetResponseServer_CDN(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionResponseServerConfidenceLevel_Ten(this.session))
            {
                return;
            }

            SetResponseServer_Unknown(this.session);
        }

        /// <summary>
        /// Set Response Server session flag as the "Server" response value.
        /// </summary>
        /// <param name="session"></param>
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

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_Server",

                ResponseServer = this.session.oResponse["Server"],

                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set Response Server session flag as the "Host" session value.
        /// </summary>
        /// <param name="session"></param>
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

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_Host",

                ResponseServer = this.session.oResponse["Host"],
                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set Response Server session flag as the "X-Powered-By" session value.
        /// </summary>
        /// <param name="session"></param>
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

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_PoweredBy",

                ResponseServer = this.session.oResponse["X-Powered-By"],
                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);            
        }

        /// <summary>
        /// Set Response Server session flag as the "X-Served-By" session value.
        /// </summary>
        /// <param name="session"></param>
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

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_ServedBy",

                ResponseServer = this.session.oResponse["X-Served-By"],
                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);            
        }

        /// <summary>
        /// Set Response Server session flag as the "X-Server-Name" session value.
        /// </summary>
        /// <param name="session"></param>
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

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_ServerName",

                ResponseServer = this.session.oResponse["X-Server-Name"],
                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set Response Server session flag as the "X-CDN-Provider" session value.
        /// </summary>
        /// <param name="session"></param>
        private void SetResponseServer_CDN(Session session)
        {
            this.session = session;

            if (this.session.oResponse["X-CDN-Provider"] == null)
            {
                return;
            }

            if (this.session.oResponse["X-CDN-Provider"] == "")
            {
                return;
            }

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_CDN",

                ResponseServer = this.session.oResponse["X-CDN-Provider"],
                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set Response Server session flag as unknown as a final fallback.
        /// </summary>
        /// <param name="session"></param>
        private void SetResponseServer_Unknown(Session session)
        {
            this.session = session;

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "ResponseServer_Unknown",

                ResponseServer = RulesetLangHelper.GetString("ResponseServer_Unknown"),
                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
