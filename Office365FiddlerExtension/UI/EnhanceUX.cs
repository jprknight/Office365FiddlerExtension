using Fiddler;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Handler;

namespace Office365FiddlerExtension.UI
{
    public class EnhanceUX : ActivationService
    {
        private static EnhanceUX _instance;
        public static EnhanceUX Instance => _instance ?? (_instance = new EnhanceUX());

        private bool IsInitialized { get; set; }

        public void Initialize()
        {
            if (!IsInitialized)
            {
                AddSazFileEventHandlers();
                AddExtensionColumns();

                IsInitialized = true;
            }
        }

        private void AddSazFileEventHandlers()
        {
            FiddlerApplication.OnLoadSAZ += SazFileHandler.Instance.LoadSaz;
            FiddlerApplication.OnSaveSAZ += SazFileHandler.Instance.SaveSaz;
        }

        private void AddExtensionColumns()
        {
            try
            {
                // Add columns to the UI and hock up to functions which populate data.
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: AddExtensionColumns: Attempting to add columns to UI.");

                // FiddlerApplication.UI.lvSessions.AddBoundColumn("Column Title", int position, int width, Session Flag or function for data);
                // FiddlerApplication.UI.lvSessions.AddBoundColumn("Elapsed Time", 2, 110, UpdateSessionUX.Instance.ElapsedTime);

                FiddlerApplication.UI.lvSessions.AddBoundColumn("Elapsed Time", 110, ElapsedTime);
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Session Type", 150, SessionType);
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Authentication", 140, Authentication);
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Response Server", 130, ResponseServer);
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Host IP", 110, HostIP);
            }
            catch (Exception e)
            {
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: AddExtensionColumns: {0} Exception caught." + e);
            }

            // REVIEW THIS. Despite adding / ordering columns here nothing happens.
            // Commenting out these seem to do nothing.
            // Wondering if the user preferences / reordering comes into play and conflicts with this.
            /*FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Custom", 15, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Comments", 14, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Content-Type", 13, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Caching", 12, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Body", 11, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("URL", 10, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Host", 9, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Protocol", 8, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Process", 7, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Elapsed Time", 6, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Session Type", 5, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Authentication", 4, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Host IP", 3, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Response Server", 2, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Result", 1, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("#", 0, -1);*/

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: AddExtensionColumns: Finished adding columns to UI.");
        }

        // Called by EnhanceUX to populate column data.
        public string ElapsedTime(Session session)
        {
            this.Session = session;

            var ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);

            return ExtensionSessionFlags.ElapsedTime;
        }

        // Called by ActivationService to populate column data.
        public string SessionType(Session session)
        {
            this.Session = session;

            var ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);

            return ExtensionSessionFlags.SessionType;
        }

        // Called by ActivationService to populate column data.
        public string Authentication(Session session)
        {
            this.Session = session;

            var ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);

            return ExtensionSessionFlags.Authentication;
        }

        // Called by ActivationService to populate column data.
        public string ResponseServer(Session session)
        {
            this.Session = session;

            var ExtensionSessionFlags = SessionFlagHandler.Instance.GetDeserializedSessionFlags(this.Session);

            return ExtensionSessionFlags.ResponseServer;
        }

        public string HostIP(Session session)
        {
            this.Session = session;

            if (this.Session["X-HostIP"] != null && this.Session["X-HostIP"] != "")
            {
                return this.Session["X-HostIP"];
            }
            else
            {
                return "Unknown";
            }
        }
    }
}
