using Fiddler;
using Office365FiddlerExtension.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtension.UI
{
    /// <summary>
    /// Add columns into Fiddler application UI and populate with data.
    /// The Fiddler UI is "additive", there are methods to add to the UI, not to remove from it.
    /// </summary>
    public class ColumnUI
    {
        internal Session session { get; set; }

        private static ColumnUI _instance;
        public static ColumnUI Instance => _instance ?? (_instance = new ColumnUI());

        private bool IsInitialized { get; set; }

        private bool ElapsedTimeColumnAdded { get; set; }

        private bool SeverityColumnAdded { get; set; }

        private bool SessionTypeColumnAdded { get; set; }

        private bool ResponseServerColumnAdded { get; set; }

        private bool AuthenticationColumnAdded { get; set; }

        private bool HostIPColumnAdded { get; set; }

        public void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            AddElapsedTimeColumn();
            AddSeverityColumn();
            AddSessionTypeColumn();
            AddResponseServerColumn();
            AddAuthenticationColumn();
            AddHostIPColumn();

            IsInitialized = true;
        }

        public void AddElapsedTimeColumn()
        {

            if (ElapsedTimeColumnAdded)
            {
                return;
            }

            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            if (!extensionSettings.ElapsedTimeColumnEnabled)
            {
                return;
            }

            try
            {
                string strElapsedTime;

                if (LangHelper.GetString("Elapsed Time") == "")
                {
                    strElapsedTime = "Elapsed Time (ms)";
                }
                else
                {
                    strElapsedTime = $"{LangHelper.GetString("Elapsed Time")} (ms)";
                }

                if (!FiddlerApplication.UI.lvSessions.Columns.ContainsKey(strElapsedTime))
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Attempting to add Elapsed Time column to Fiddler UI.");
                    FiddlerApplication.UI.lvSessions.AddBoundColumn(strElapsedTime, -1, 110, true, GetElapsedTime);
                }

                ElapsedTimeColumnAdded = true;
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
        }

        public void AddSeverityColumn()
        {
            if (SeverityColumnAdded)
            {
                return;
            }

            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            if (!extensionSettings.SeverityColumnEnabled)
            {
                return;
            }

            try
            {
                string strSeverity;

                if (LangHelper.GetString("Severity") == "")
                {
                    strSeverity = "Severity";
                }
                else
                {
                    strSeverity = LangHelper.GetString("Severity");
                }
                if (!FiddlerApplication.UI.lvSessions.Columns.ContainsKey(strSeverity))
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Attempting to add Severity column to Fiddler UI.");
                    FiddlerApplication.UI.lvSessions.AddBoundColumn(strSeverity, -1, 60, true, GetSessionSeverity);
                }

                SeverityColumnAdded = true;
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
        }

        public void AddSessionTypeColumn()
        {
            if (SessionTypeColumnAdded)
            {
                return;
            }

            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            if (!extensionSettings.SessionTypeColumnEnabled)
            {
                return;
            }
            try
            {
                string strSessionType;

                if (LangHelper.GetString("Session Type") == "")
                {
                    strSessionType = "Session Type";
                }
                else
                {
                    strSessionType = LangHelper.GetString("Session Type");
                }

                if (!FiddlerApplication.UI.lvSessions.Columns.ContainsKey(strSessionType))
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Attempting to add Session Type column to Fiddler UI.");
                    FiddlerApplication.UI.lvSessions.AddBoundColumn(strSessionType, 150, GetSessionType);
                }

                SessionTypeColumnAdded = true;
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }

        }

        public void AddAuthenticationColumn()
        {
            if (AuthenticationColumnAdded)
            {
                return;
            }

            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            if (extensionSettings.AuthenticationColumnEnabled)
            {
                try
                {
                    string strAuthentication;

                    if (LangHelper.GetString("Authentication") == "")
                    {
                        strAuthentication = "Authentication";
                    }
                    else
                    {
                        strAuthentication = LangHelper.GetString("Authentication");
                    }

                    if (!FiddlerApplication.UI.lvSessions.Columns.ContainsKey(strAuthentication))
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Attempting to add Authentication column to Fiddler UI.");
                        FiddlerApplication.UI.lvSessions.AddBoundColumn(strAuthentication, 140, GetAuthentication);
                    }

                    AuthenticationColumnAdded = true;

                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
                }
            }
        }

        public void AddResponseServerColumn()
        {
            if (ResponseServerColumnAdded)
            {
                return;
            }

            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            if (extensionSettings.ResponseServerColumnEnabled)
            {
                try
                {
                    string strResponseServer;

                    if (LangHelper.GetString("Response Server") == "")
                    {
                        strResponseServer = "Response Server";
                    }
                    else
                    {
                        strResponseServer = LangHelper.GetString("Response Server");
                    }

                    if (!FiddlerApplication.UI.lvSessions.Columns.ContainsKey(strResponseServer))
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Attempting to add Response Server column to Fiddler UI.");
                        FiddlerApplication.UI.lvSessions.AddBoundColumn(strResponseServer, 130, GetResponseServer);
                    }

                    ResponseServerColumnAdded = true;
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
                }
            }
        }

        public void AddHostIPColumn()
        {
            if (HostIPColumnAdded)
            {
                return;
            }

            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            if (extensionSettings.HostIPColumnEnabled)
            {
                try
                {
                    string strHostIP;

                    if (LangHelper.GetString("Host IP") == "")
                    {
                        strHostIP = "Host IP";
                    }
                    else
                    {
                        strHostIP = LangHelper.GetString("Host IP");
                    }

                    if (!FiddlerApplication.UI.lvSessions.Columns.ContainsKey(strHostIP))
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Attempting to add Host IP column to Fiddler UI.");
                        FiddlerApplication.UI.lvSessions.AddBoundColumn(strHostIP, 110, GetHostIP);
                    }

                    HostIPColumnAdded = true;
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
                }
            }
        }

        /// <summary>
        /// Populate session severity into column added by extension.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private string GetSessionSeverity(Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            return ExtensionSessionFlags.SessionSeverity.ToString();
        }

        /// <summary>
        /// Populate elapsed time into column added by extension.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private string GetElapsedTime(Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            return ExtensionSessionFlags.ElapsedTime;
        }

        /// <summary>
        /// Populate session type into column added by extension.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private string GetSessionType(Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            return ExtensionSessionFlags.SessionType;
        }

        /// <summary>
        /// Populate authentication into column added by extension.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private string GetAuthentication(Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            return ExtensionSessionFlags.Authentication;
        }

        /// <summary>
        /// Populate response server into column added by extension.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private string GetResponseServer(Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            return ExtensionSessionFlags.ResponseServer;
        }

        /// <summary>
        /// Populate HostIP into column added by extension.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private string GetHostIP(Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            return ExtensionSessionFlags.HostIP;
        }
    }
}