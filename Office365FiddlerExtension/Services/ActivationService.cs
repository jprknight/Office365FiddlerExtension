using Fiddler;
using System.Windows.Forms;
using System.Text;
using System;
using System.Reflection;
using Office365FiddlerExtension.UI;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Global application initializer.
    /// </summary>
    public abstract class ActivationService : IAutoTamper
    {
        internal Session session { get; set; }

        /// <summary>
        /// This should be considered the main constructor for the extension. 
        /// It's called after the UI has loaded.
        /// </summary>
        public async void OnLoad()
        {
            MenuUI.Instance.Initialize();

            UserInterface.Instance.Initialize();
        }

        public async void OnBeforeUnload()
        {
            var ExtensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();

            if (!ExtensionSettings.NeverWebCall)
            {
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: ActivationService: Closing telemetry service client connection.");
                await TelemetryService.FlushClientAsync();
            } 
        }

        /// <summary>
        /// Called for each HTTP/HTTPS request after it's complete.
        /// </summary>
        /// <param name="_session"></param>
        public void AutoTamperRequestAfter(Session _session) { }

        /// <summary>
        /// Called for each HTTP/HTTPS request before it's complete.
        /// </summary>
        /// <param name="_session"></param>
        public void AutoTamperRequestBefore(Session _session) { }

        /// <summary>
        /// Called for each HTTP/HTTPS response after it's complete.
        /// </summary>
        /// <param name="_session"></param>
        public void AutoTamperResponseAfter(Session session)
        {
            if (!SettingsHandler.Instance.ExtensionEnabled)
            {
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: Extension not enabled, returning.");
                return;
            }

            this.session = session;

            // Call the main fuction which runs through all session logic checks.
            // REVIEW THIS - Is this needed? Live trace?
            if (this.session.isFlagSet(SessionFlags.LoadedFromSAZ))
            {
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: ActivationService: Session loaded from Saz file, return.");
                return;
            }

            SessionHandler.Instance.OnPeekAtResponseHeaders(this.session);

            this.session.RefreshUI();
        }

        /// <summary>
        /// Called for each HTTP/HTTPS response before it's complete.
        /// </summary>
        /// <param name="_session"></param>
        public void AutoTamperResponseBefore(Session _session) { }

        /// <summary>
        /// Called for each HTTP/HTTPS error response before it's complete.
        /// </summary>
        /// <param name="_session"></param>
        public void OnBeforeReturningError(Session _session) { }

        public void ExtensionEnabledPrefWatcer()
        {

        }

    }

    public class UserInterface : ActivationService
    {
        private static UserInterface _instance;

        public static UserInterface Instance => _instance ?? (_instance = new UserInterface());

        private bool IsInitialized { get; set; }

        public async void Initialize()
        {
            if (!IsInitialized)
            {
                SettingsHandler.Instance.CreateExtensionSettings();
                SettingsHandler.Instance.CreateExtensionURLJsonFiddlerSetting();
                SettingsHandler.Instance.UpdateExtensionVersionFiddlerSetting();

                var ExtensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();

                // Stop if extension is not enabled.
                if (!ExtensionSettings.ExtensionEnabled)
                {
                    FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: Extension not enabled, exiting.");
                    return;
                }

                // Add Saz file event handlers.
                FiddlerApplication.OnLoadSAZ += SazFileHandler.Instance.LoadSaz;
                FiddlerApplication.OnSaveSAZ += SazFileHandler.Instance.SaveSaz;

                await Preferences.SetDefaultPreferences();

                try
                {
                    // Add columns to the UI and hock up to functions which populate data.
                    FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: ActivationService: Attempting to add columns to UI.");

                    // FiddlerApplication.UI.lvSessions.AddBoundColumn("Column Title", int position, int width, Session Flag or function for data);
                    // FiddlerApplication.UI.lvSessions.AddBoundColumn("Elapsed Time", 2, 110, UpdateSessionUX.Instance.ElapsedTime);

                    FiddlerApplication.UI.lvSessions.AddBoundColumn("Elapsed Time", 110, UpdateSessionUX.Instance.ElapsedTime);
                    FiddlerApplication.UI.lvSessions.AddBoundColumn("Session Type", 150, UpdateSessionUX.Instance.SessionType);
                    FiddlerApplication.UI.lvSessions.AddBoundColumn("Authentication", 140, UpdateSessionUX.Instance.Authentication);
                    FiddlerApplication.UI.lvSessions.AddBoundColumn("Response Server", 130, UpdateSessionUX.Instance.ResponseServer);
                    FiddlerApplication.UI.lvSessions.AddBoundColumn("Host IP", 110, UpdateSessionUX.Instance.HostIP);                
                }
                catch (Exception e)
                {
                    FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: ActivationService: {0} Exception caught." + e);
                }

                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: ActivationService: Finished adding columns to UI.");

                // If disable web calls is true dion't check for updates and don't call telemetry service.
                if (ExtensionSettings.NeverWebCall)
                {
                    FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: ActivationService: Telemetry Service NeverWebCall is true.");
                    FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: ActivationService: Not checking for updates or sending telemetry data.");
                }
                // Otherwise, check for updates and call telemetry service.
                else
                {
                    FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: ActivationService: Checking for updates.");
                    About.Instance.CheckForUpdate();
                    FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: ActivationService: Initializing telemetry service.");
                    await TelemetryService.InitializeAsync();
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
                //FiddlerApplication.UI.Refresh();

                IsInitialized = true;
            }
        }
    }
}
