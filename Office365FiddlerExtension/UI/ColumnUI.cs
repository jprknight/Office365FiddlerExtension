using Fiddler;
using Office365FiddlerExtension.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtension.UI
{
    /// <summary>
    /// Add columns into Fiddler application UI and populate with data.
    /// </summary>
    public class ColumnUI
    {
        internal Session session { get; set; }

        private static ColumnUI _instance;
        public static ColumnUI Instance => _instance ?? (_instance = new ColumnUI());

        private bool IsInitialized { get; set; }

        public void Initialize()
        {
            if (!IsInitialized)
            {
                AddExtensionColumns();

                IsInitialized = true;
            }
        }

        /// <summary>
        /// Add columns to the UI and hock up to functions which populate data.
        /// </summary>
        private void AddExtensionColumns()
        {
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Adding columns to Fiddler UI.");

            // FiddlerApplication.UI.lvSessions.AddBoundColumn("Column Title", int position, int width, Session Flag or function for data);
            // FiddlerApplication.UI.lvSessions.AddBoundColumn("Elapsed Time", 2, 110, UpdateSessionUX.Instance.ElapsedTime);

            // If column names are blank or empty strings they don't seem to be added in the Fiddler UI.
            // Make sure each column has a valid string to add to the UI with.

            // Elapsed Time.

            try
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Attempting to add Elapsed Time column to Fiddler UI.");

                string strElapsedTime;

                if (LangHelper.GetString("Elapsed Time") == "")
                {
                    strElapsedTime = "Elapsed Time (ms)";
                }
                else
                {
                    strElapsedTime = $"{LangHelper.GetString("Elapsed Time")} (ms)";
                }

                FiddlerApplication.UI.lvSessions.AddBoundColumn(strElapsedTime, -1, 110, true, ElapsedTime);
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }

            // Severity.

            try
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Attempting to add Severity column to Fiddler UI.");

                string strSeverity;

                if (LangHelper.GetString("Severity") == "")
                {
                    strSeverity = "Severity";
                }
                else
                {
                    strSeverity = LangHelper.GetString("Severity");
                }

                FiddlerApplication.UI.lvSessions.AddBoundColumn(strSeverity, -1, 60, true, SessionSeverity);
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }

            // Session Type.

            try
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Attempting to add Session Type column to Fiddler UI.");

                string strSessionType;

                if (LangHelper.GetString("Session Type") == "")
                {
                    strSessionType = "Session Type";
                }
                else
                {
                    strSessionType = LangHelper.GetString("Session Type");
                }

                FiddlerApplication.UI.lvSessions.AddBoundColumn(strSessionType, 150, SessionType);
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }

            // Authentication.

            try
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Attempting to add Authentication column to Fiddler UI.");

                string strAuthentication;

                if (LangHelper.GetString("Authentication") == "")
                {
                    strAuthentication = "Authentication";
                }
                else
                {
                    strAuthentication = LangHelper.GetString("Authentication");
                }

                FiddlerApplication.UI.lvSessions.AddBoundColumn(strAuthentication, 140, Authentication);
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }

            // Response Server.

            try
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Attempting to add Response Server column to Fiddler UI.");

                string strResponseServer;

                if (LangHelper.GetString("Response Server") == "")
                {
                    strResponseServer = "Response Server";
                }
                else
                {
                    strResponseServer = LangHelper.GetString("Response Server");
                }

                FiddlerApplication.UI.lvSessions.AddBoundColumn(strResponseServer, 130, ResponseServer);
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }

            try
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Attempting to add Host IP column to Fiddler UI.");

                string strHostIP;

                if (LangHelper.GetString("Host IP") == "")
                {
                    strHostIP = "Host IP";
                }
                else
                {
                    strHostIP = LangHelper.GetString("Host IP");
                }

                FiddlerApplication.UI.lvSessions.AddBoundColumn(strHostIP, 110, HostIP);
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
        }

        /// <summary>
        /// Populate session severity into column added by extension.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private string SessionSeverity(Session session)
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
        private string ElapsedTime(Session session)
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
        private string SessionType(Session session)
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
        private string Authentication(Session session)
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
        private string ResponseServer(Session session)
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
        private string HostIP(Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            return ExtensionSessionFlags.HostIP;
        }
    }
}
