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
            try
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Adding columns to UI.");

                // FiddlerApplication.UI.lvSessions.AddBoundColumn("Column Title", int position, int width, Session Flag or function for data);
                // FiddlerApplication.UI.lvSessions.AddBoundColumn("Elapsed Time", 2, 110, UpdateSessionUX.Instance.ElapsedTime);

                FiddlerApplication.UI.lvSessions.AddBoundColumn($"{LangHelper.GetString("Elapsed Time")} (ms)", -1, 110, true, ElapsedTime);
                FiddlerApplication.UI.lvSessions.AddBoundColumn(LangHelper.GetString("Severity"), -1, 60, true, SessionSeverity);
                FiddlerApplication.UI.lvSessions.AddBoundColumn(LangHelper.GetString("Session Type"), 150, SessionType);
                FiddlerApplication.UI.lvSessions.AddBoundColumn(LangHelper.GetString("Authentication"), 140, Authentication);
                FiddlerApplication.UI.lvSessions.AddBoundColumn(LangHelper.GetString("Response Server"), 130, ResponseServer);
                FiddlerApplication.UI.lvSessions.AddBoundColumn(LangHelper.GetString("Host IP"), 110, HostIP);
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
