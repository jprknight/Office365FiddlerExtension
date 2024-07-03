using Fiddler;
using Office365FiddlerExtension.UI;
using System;
using System.Reflection;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Global application initializer.
    /// </summary>
    public abstract class ActivationService : IAutoTamper
    {
        internal Session session { get; set; }

        private bool IsInitialized { get; set; }

        /// <summary>
        /// 
        /// MAIN
        /// 
        /// This should be considered the main constructor for the extension. 
        /// It's called after the Fiddler UI has loaded.
        /// </summary>
        public void OnLoad()
        {
            if (!IsInitialized)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} (ActivationService): " +
                    $"Starting v" +
                    $"{Assembly.GetExecutingAssembly().GetName().Version.Major}." +
                    $"{Assembly.GetExecutingAssembly().GetName().Version.Minor}." +
                    $"{Assembly.GetExecutingAssembly().GetName().Version.Build}");

                // Ensure Fiddler settings (settings, URLs, & verison) for the extension have been created.
                // Avoid null exceptions.
                SettingsJsonService.Instance.CreateExtensionSettingsFiddlerApplicationPreference();
                URLsJsonService.Instance.CreateExtensionURLFiddlerApplicationPreference();
                VersionJsonService.Instance.CreateExtensionVersionFiddlerApplicationPreference();
                SessionClassificationService.Instance.CreateSessionClassificationFiddlerApplicationPreference();

                if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().DebugMode)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} (ActivationService): " +
                        $"Debug Mode set to true.");
                }

                // Set extension language based on preferred language.
                LangHelper.ChangeLanguage(SettingsJsonService.Instance.GetDeserializedExtensionSettings().PreferredLanguage);

                // Set Fiddler settings as needed.
                SettingsJsonService.Instance.SetExtensionDLL();
                SettingsJsonService.Instance.SetExtensionPath();
                SettingsJsonService.Instance.IncrementExecutionCount();
                                
                InitializeTelemetry();

                // Update as needed. -- Web update calls live here.
                UpdateService.Instance.Initialize();

                // Notify user if updates are available. -- More web calls here.
                VersionService.Instance.NotifyUserIfExtensionUpdateIsAvailable();
                VersionService.Instance.NotifyUserIfRulesetUpdateIsAvailable();

                // Add extension menu.
                MenuUI.Instance.Initialize();

                // Add context menu.
                ContextMenuUI.Instance.initialize();

                // Add columns into session list in UI.
                ColumnUI.Instance.Initialize();

                // Register available Fiddler events.
                FiddlerApplication.OnLoadSAZ += SazFileService.Instance.LoadSaz;
                FiddlerApplication.OnSaveSAZ += SazFileService.Instance.SaveSaz;

                IsInitialized = true;
            }
        }

        /// <summary>
        /// Called as Fiddler shuts down.
        /// </summary>
        public void OnBeforeUnload()
        {
            ShutdownTelemetry();
        }

        /// <summary>
        /// Called for each HTTP/HTTPS request after it's complete.
        /// </summary>
        /// <param name="_session"></param>
        public void AutoTamperRequestAfter(Session session) { }

        /// <summary>
        /// Called for each HTTP/HTTPS request before it's complete.
        /// </summary>
        /// <param name="_session"></param>
        public void AutoTamperRequestBefore(Session session) { }

        /// <summary>
        /// Called for each HTTP/HTTPS response after it's complete.
        /// </summary>
        /// <param name="_session"></param>
        public void AutoTamperResponseAfter(Session session)
        {
            this.session = session;

            if (!SettingsJsonService.Instance.ExtensionSessionProcessingEnabled)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Extension not enabled, returning.");
                return;
            }

            // If session analysis on live trace is enabled, run.
            if (SettingsJsonService.Instance.SessionAnalysisOnLiveTrace)
            {
                try
                {
                    SessionService.Instance.OnPeekAtResponseHeaders(this.session);
                    this.session.RefreshUI();
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
                }
            }
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

        /// <summary>
        /// Initialize telemetry if NeverWebCall is false.
        /// </summary>
        private async void InitializeTelemetry()
        {
            await TelemetryService.InitializeAsync();
        }

        /// <summary>
        /// Shutdown telemetry.
        /// </summary>
        private async void ShutdownTelemetry()
        {
            await TelemetryService.FlushClientAsync();
        }
    }
}
