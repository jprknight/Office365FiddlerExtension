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

        private void AddExtensionColumns()
        {
            try
            {
                // Add columns to the UI and hock up to functions which populate data.
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

        private string SessionSeverity(Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            return ExtensionSessionFlags.SessionSeverity.ToString();
        }

        // Called by EnhanceUX to populate column data.
        public string ElapsedTime(Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            return ExtensionSessionFlags.ElapsedTime;
        }

        // Called by ActivationService to populate column data.
        public string SessionType(Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            return ExtensionSessionFlags.SessionType;
        }

        // Called by ActivationService to populate column data.
        public string Authentication(Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            return ExtensionSessionFlags.Authentication;
        }

        // Called by ActivationService to populate column data.
        public string ResponseServer(Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            return ExtensionSessionFlags.ResponseServer;
        }

        public string HostIP(Session session)
        {
            this.session = session;

            if (this.session["X-HostIP"] != null && this.session["X-HostIP"] != "")
            {
                // Tuple -- tupleIsPrivateIPAddress (bool), matching subnet (string).
                Tuple<bool, string> tupleIsPrivateIPAddress = NetworkingService.Instance.IsPrivateIPAddress(this.session);

                if (tupleIsPrivateIPAddress.Item1)
                {
                    return "LAN:" + this.session["X-HostIP"];
                }
                else
                {
                    // Tuple -- IsMicrosoft365IP (bool), matching subnet (string).
                    Tuple<bool, string> tupleIsMicrosoft365IPAddress = NetworkingService.Instance.IsMicrosoft365IPAddress(this.session);

                    if (tupleIsMicrosoft365IPAddress.Item1)
                    {
                        return "M365:" + this.session["X-HostIP"];
                    }
                    else
                    {
                        return this.session["X-HostIP"];
                    }
                }
            }
            else
            {
                return LangHelper.GetString("Unknown");
            }
        }
    }
}
