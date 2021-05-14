using Fiddler;

namespace Office365FiddlerInspector.Services
{
    /// <summary>
    /// Global application initializer.
    /// </summary>
    public abstract class ActivationService : IAutoTamper
    {
        /// <summary>
        /// This should be consider the main constructor for the extension. It's called after the UI has loaded.
        /// </summary>
        public async void OnLoad()
        {
            MenuUI.Instance.Initialize();
            if (Preferences.ExecutionCount == 0)
            {
                await Preferences.SetDefaultPreferences();
            }

            SessionProcessor.Instance.Initialize();

            FiddlerApplication.UI.lvSessions.AddBoundColumn("Elapsed Time", 110, "X-ElapsedTime");
            FiddlerApplication.UI.lvSessions.AddBoundColumn("Session Type", 150, "X-SessionType");
            FiddlerApplication.UI.lvSessions.AddBoundColumn("Authentication", 140, "X-Authentication");
            FiddlerApplication.UI.lvSessions.AddBoundColumn("Host IP", 110, "X-HostIP");
            FiddlerApplication.UI.lvSessions.AddBoundColumn("Response Server", 130, "X-ResponseServer");
            
            // If disable web calls is true dion't check for updates and don't call telemetry service.
            if (Preferences.DisableWebCalls)
            {
                FiddlerApplication.Log.LogString($"OFFICE 365 EXTENSION: ActivationService: Telemetry Service DisableWebCalls is true.");
                
                FiddlerApplication.Log.LogString($"OFFICE 365 EXTENSION: ActivationService: The Office 365 Fiddler Extension won't check for updates.");
                FiddlerApplication.Log.LogString($"OFFICE 365 EXTENSION: ActivationService: The Office 365 Fiddler Extension won't send telemetry data.");
            }
            // Otherwise, check for updates and call telemetry service.
            else
            {
                FiddlerApplication.Log.LogString($"OFFICE 365 EXTENSION: ActivationService: The Office 365 Fiddler Extension checking for updates.");
                About.Instance.CheckForUpdate();
                FiddlerApplication.Log.LogString($"OFFICE 365 EXTENSION: ActivationService: The Office 365 Fiddler Extension initializing telemetry service.");
                await TelemetryService.InitializeAsync();
            }
        }

        public async void OnBeforeUnload()
        {
            if (Preferences.DisableWebCalls)
            {
                // Do nothing.
            }
            else
            {
                FiddlerApplication.Log.LogString($"OFFICE 365 EXTENSION: ActivationService: The Office 365 Fiddler Extension closing telemetry service client connection.");
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
            if (!Preferences.ExtensionEnabled)
            {
                return;
            }

            SessionProcessor.Instance.SetElapsedTime(session);

            SessionProcessor.Instance.SetResponseServer(session);

            SessionProcessor.Instance.SetAuthentication(session);

            SessionProcessor.Instance.SetSessionType(session);

            SessionProcessor.Instance.CalculateSessionAge(session);

            SessionProcessor.Instance.SetInspectorElapsedTime(session);

            SessionProcessor.Instance.SetServerThinkTime(session);

            SessionProcessor.Instance.SetTransitTime(session);

            SessionProcessor.Instance.OnPeekAtResponseHeaders(session);

            session.RefreshUI();
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

    }
}
