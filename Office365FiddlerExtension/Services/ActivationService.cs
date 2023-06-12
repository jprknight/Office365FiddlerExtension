using Fiddler;
using System.Windows.Forms;
using System.Text;
using System;
using System.Reflection;
using Office365FiddlerExtension.UI;
using Office365FiddlerExtension.Handlers;

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
        /// This should be considered the main constructor for the extension. 
        /// It's called after the UI has loaded.
        /// </summary>
        public void OnLoad()
        {
            if (!IsInitialized)
            {
                // Ensure Fiddler settings (settings, URLs, & verison) for the extension have been created.
                SettingsHandler.Instance.CreateExtensionSettingsFiddlerSetting();
                URLsHandler.Instance.CreateExtensionURLFiddlerSetting();
                VersionHandler.Instance.UpdateExtensionVersionFiddlerSetting();
                
                // Update / set settings as needed.
                SettingsHandler.Instance.IncrementExecutionCount();
                SettingsHandler.Instance.SetExtensionDLL();
                SettingsHandler.Instance.SetExtensionPath();

                Initializetelemetry();

                // Add extension menu.
                MenuUI.Instance.Initialize();
                // Add columns into session list in UI.
                EnhanceUX.Instance.Initialize();

                IsInitialized = true;
            }
        }

        public void OnBeforeUnload()
        {
            ShutdownTelemetry();
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

            // If session analysis on live trace is enabled, run.
            if (SettingsHandler.Instance.SessionAnalysisOnLiveTrace)
            {
                this.session = session;
                SessionHandler.Instance.OnPeekAtResponseHeaders(this.session);
                this.session.RefreshUI();
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

        private async void Initializetelemetry()
        {
            var ExtensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();

            // Stop if extension is not enabled.
            if (!ExtensionSettings.ExtensionEnabled)
            {
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: Extension not enabled, exiting.");
                return;
            }

            // If disable web calls is true dion't check for updates and don't call telemetry service.
            if (ExtensionSettings.NeverWebCall)
            {
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: Telemetry Service NeverWebCall is true.");
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: Not checking for updates or sending telemetry data.");
            }
            // Otherwise, check for updates and call telemetry service.
            else
            {
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: Checking for updates.");
                // REVIEW THIS. Call to update needs a complete rewrite.
                // Don't call this function anymore.
                // About.Instance.CheckForUpdate();
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: Initializing telemetry service.");
                await TelemetryService.InitializeAsync();
            }
        }

        private async void ShutdownTelemetry()
        {
            var ExtensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();

            if (!ExtensionSettings.NeverWebCall)
            {
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: ActivationService: Closing telemetry service client connection.");
                await TelemetryService.FlushClientAsync();
            }
        }

    }
}
