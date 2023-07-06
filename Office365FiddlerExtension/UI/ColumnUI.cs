using Fiddler;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Handler;
using System.Reflection;

namespace Office365FiddlerExtension.UI
{
    /// <summary>
    /// Add Fiddler application event handlers, additional columns to UI, and populate data in them. 
    /// </summary>
    public class ColumnUI
    {
        internal Session Session { get; set; }

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

                FiddlerApplication.UI.lvSessions.AddBoundColumn("Elapsed Time (ms)", -1, 110, true, ElapsedTime);
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Session Type", 150, SessionType);
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Authentication", 140, Authentication);
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Response Server", 130, ResponseServer);
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Host IP", 110, HostIP);
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }

            /*
            // REVIEW THIS. Despite adding / ordering columns here nothing happens.
            // Commenting out these seem to do nothing.
            // It's possible Fiddler's user preferences / reordering comes into play and conflicts with these.
            
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Custom", 15, -1);
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
