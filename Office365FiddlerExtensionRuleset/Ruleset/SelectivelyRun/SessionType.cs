using Office365FiddlerExtensionRuleset.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;
using System.Diagnostics;
using Office365FiddlerExtension.Services;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class SessionType
    {
        internal Session session { get; set; }

        private static SessionType _instance;

        public static SessionType Instance => _instance ?? (_instance = new SessionType());

        /// <summary>
        /// Set the session type, run towards end of ruleset processing as a final catch all.
        /// Used by the UI column and response inspector.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            // Don't do anything here if the session type confidence is already 10.
            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            var sw_SetSessionType_Microsoft365_EWS = Stopwatch.StartNew();

            SetSessionType_Microsoft365_EWS(this.session);

            sw_SetSessionType_Microsoft365_EWS.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_SetSessionType_Microsoft365_EWS");
                TelemetryService.CustomTrackMetric("RS_SetSessionType_Microsoft365_EWS", sw_SetSessionType_Microsoft365_EWS.ElapsedMilliseconds);
            }

            ///////////////////////////////

            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            var sw_SetSessionType_EWS = Stopwatch.StartNew();

            SetSessionType_EWS(this.session);

            sw_SetSessionType_EWS.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_SetSessionType_EWS");
                TelemetryService.CustomTrackMetric("RS_SetSessionType_EWS", sw_SetSessionType_EWS.ElapsedMilliseconds);
            }

            ///////////////////////////////

            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            var sw_SetSessionType_Microsoft365_Authentication = Stopwatch.StartNew();

            SetSessionType_Microsoft365_Authentication(this.session);

            sw_SetSessionType_Microsoft365_Authentication.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_SetSessionType_Microsoft365_Authentication");
                TelemetryService.CustomTrackMetric("RS_SetSessionType_Microsoft365_Authentication", sw_SetSessionType_Microsoft365_Authentication.ElapsedMilliseconds);
            }

            ///////////////////////////////

            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            var sw_SetSessionType_ADFS_Authentication = Stopwatch.StartNew();

            SetSessionType_ADFS_Authentication(this.session);

            sw_SetSessionType_ADFS_Authentication.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_SetSessionType_ADFS_Authentication");
                TelemetryService.CustomTrackMetric("RS_SetSessionType_ADFS_Authentication", sw_SetSessionType_ADFS_Authentication.ElapsedMilliseconds);
            }

            ///////////////////////////////

            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            var sw_SetSessionType_General_Microsoft365 = Stopwatch.StartNew();

            SetSessionType_General_Microsoft365(this.session);

            sw_SetSessionType_General_Microsoft365.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_SetSessionType_General_Microsoft365");
                TelemetryService.CustomTrackMetric("RS_SetSessionType_General_Microsoft365", sw_SetSessionType_General_Microsoft365.ElapsedMilliseconds);
            }

            ///////////////////////////////

            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            var sw_SetSessionType_Office_Applications = Stopwatch.StartNew();

            SetSessionType_Office_Applications(this.session);

            sw_SetSessionType_Office_Applications.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_SetSessionType_Office_Applications");
                TelemetryService.CustomTrackMetric("RS_SetSessionType_Office_Applications", sw_SetSessionType_Office_Applications.ElapsedMilliseconds);
            }

            ///////////////////////////////

            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            var sw_SetSessionType_Internet_Browsers = Stopwatch.StartNew();

            SetSessionType_Internet_Browsers(this.session);

            sw_SetSessionType_Internet_Browsers.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_SetSessionType_Internet_Browsers");
                TelemetryService.CustomTrackMetric("RS_SetSessionType_Internet_Browsers", sw_SetSessionType_Internet_Browsers.ElapsedMilliseconds);
            }

            ///////////////////////////////

            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            var sw_SetSessionType_Unknown = Stopwatch.StartNew();

            SetSessionType_Unknown(this.session);

            sw_SetSessionType_Unknown.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_SetSessionType_Unknown");
                TelemetryService.CustomTrackMetric("RS_SetSessionType_Unknown", sw_SetSessionType_Unknown.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// Set Session Type session flag to Microsoft 365 Exchange Web Services (EWS).
        /// </summary>
        /// <param name="session"></param>
        private void SetSessionType_Microsoft365_EWS(Session session)
        {
            this.session = session;

            if (!this.session.fullUrl.Contains("outlook.office365.com/EWS"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running SetSessionType_Microsoft365_EWS");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "SessionType_Microsoft365_EWS",

                SessionType = RulesetLangHelper.GetString("Exchange Web Services"),
                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set Session Type session flag to any other Exchange Web Services (EWS).
        /// </summary>
        /// <param name="session"></param>
        private void SetSessionType_EWS(Session session)
        {
            this.session = session;

            if (!this.session.fullUrl.Contains("/EWS"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running SetSessionType_EWS");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "SessionType_EWS",

                SessionType = RulesetLangHelper.GetString("Exchange Web Services"),
                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set Session Type session flag to Microsoft 365 Authentication.
        /// </summary>
        /// <param name="session"></param>
        private void SetSessionType_Microsoft365_Authentication(Session session)
        {
            this.session = session;

            // This check needs to be inclusive, so we don't exclude sessions.
            if (this.session.url.Contains("login.microsoftonline.com") || this.session.HostnameIs("login.microsoftonline.com"))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name}" +
                    $" ({this.GetType().Name}): {this.session.id} Running SetSessionType_Microsoft365_Authentication");

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "SessionType_Microsoft365_Authentication",

                    SessionType = RulesetLangHelper.GetString("Microsoft365 Authentication"),
                    SessionTypeConfidenceLevel = 10
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
        }

        /// <summary>
        /// Set Session Type session flag to Active Directory Federation Services (ADFS) authentication.
        /// </summary>
        /// <param name="session"></param>
        private void SetSessionType_ADFS_Authentication(Session session)
        {
            this.session = session;

            if (!this.session.fullUrl.Contains("adfs/services/trust/mex"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running SetSessionType_ADFS_Authentication");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "SessionType_ADFS_Authentication",

                SessionType = RulesetLangHelper.GetString("ADFS Authentication"),
                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set Session Type session flag to generalised Microsoft 365.
        /// </summary>
        /// <param name="session"></param>
        private void SetSessionType_General_Microsoft365(Session session)
        {
            this.session = session;

            if (!this.session.fullUrl.Contains("outlook.office365.com"))
            {
                return;
            }

            if (!this.session.fullUrl.Contains("outlook.office.com"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running SetSessionType_General_Microsoft365");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "SessionType_General_Microsoft365",

                SessionType = RulesetLangHelper.GetString("General Microsoft365"),
                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set Session Type session flag to the name of the process executible on the session.
        /// </summary>
        /// <param name="session"></param>
        private void SetSessionType_Office_Applications(Session session)
        {
            this.session = session;

            if (this.session.LocalProcess.Contains("outlook")
                || this.session.LocalProcess.Contains("searchprotocolhost")
                || this.session.LocalProcess.Contains("winword")
                || this.session.LocalProcess.Contains("excel")
                || this.session.LocalProcess.Contains("onenote")
                || this.session.LocalProcess.Contains("msaccess")
                || this.session.LocalProcess.Contains("powerpnt")
                || this.session.LocalProcess.Contains("mspub")
                || this.session.LocalProcess.Contains("onedrive")
                || this.session.LocalProcess.Contains("lync")
                || this.session.LocalProcess.Contains("w3wp"))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} Running SetSessionType_Office_Applications");

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "SessionType_Office_Applications",

                    SessionType = this.session.LocalProcess.Split(':')[0],
                    SessionTypeConfidenceLevel = 10
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);

            }
        }

        /// <summary>
        /// Set the Session Type session flag to the name of a browser process executible.
        /// </summary>
        /// <param name="session"></param>
        private void SetSessionType_Internet_Browsers(Session session)
        {
            this.session = session;

            if (this.session.LocalProcess.Contains("iexplore")
                || this.session.LocalProcess.Contains("chrome")
                || this.session.LocalProcess.Contains("firefox")
                || this.session.LocalProcess.Contains("edge")
                || this.session.LocalProcess.Contains("msedge")
                || this.session.LocalProcess.Contains("safari")
                || this.session.LocalProcess.Contains("brave")
                || this.session.LocalProcess.Contains("opera"))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} Running SetSessionType_Internet_Browsers");

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "SessionType_Internet_Browsers",

                    SessionType = this.session.LocalProcess.Split(':')[0],
                    SessionTypeConfidenceLevel = 10
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);

            }
        }
        
        /// <summary>
        /// Set Session Type session flag to unknown as a final fallback.
        /// </summary>
        /// <param name="session"></param>
        private void SetSessionType_Unknown(Session session)
        {
            
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running SetSessionType_Unclassified");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "SetSessionType",

                SessionType = this.session["X-ProcessName"],
                ResponseAlert = RulesetLangHelper.GetString("Unclassified"),
                ResponseComments = RulesetLangHelper.GetString("SessionType_Unknown_ResponseComments"),

                SessionTypeConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
