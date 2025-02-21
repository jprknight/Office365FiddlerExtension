using Fiddler;
using Office365FiddlerExtension.Services.Fiddler;
using Office365FiddlerExtension.UI;
using Office365FiddlerExtension.UI.Forms;
using System.Reflection;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Global application initializer.
    /// </summary>
    public abstract class ActivationService : IAutoTamper
    {
        private PreferenceBag.PrefWatcher oWatcher;

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

                // Control whether Fiddler captures sessions on startup. Useful as I only mostly review traces and data in Fiddler rather
                // than capture from my own machine.
                if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().CaptureTraffic)
                {
                    FiddlerApplication.UI.actAttachProxy();
                }
                else
                {
                    FiddlerApplication.UI.actDetachProxy();
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
                UpdateService.Instance.NotifyUserIfExtensionUpdateIsAvailable();
                UpdateService.Instance.NotifyUserIfRulesetUpdateIsAvailable();

                // Add extension menu.
                MenuUI.Instance.Initialize();

                // Add context menu.
                ContextMenuUI.Instance.initialize();

                // Add columns into session list in UI.
                ColumnUI.Instance.Initialize();

                // Register available Fiddler events.
                FiddlerApplication.OnLoadSAZ += SazFileService.Instance.LoadSaz;
                FiddlerApplication.OnSaveSAZ += SazFileService.Instance.SaveSaz;
                //FiddlerApplication.UI.lvSessions.OnSessionsAdded += ImportService.Instance.ImportSessions;
                FiddlerApplication.UI.lvSessions.OnSessionsAdded += OnSessionsAddedService.Instance.ProcessSessions;

                string sPrefixToMatch = "extensions.Office365FiddlerExtension.ExtensionSettings";

                oWatcher = FiddlerApplication.Prefs.AddWatcher(sPrefixToMatch, ExtensionPreferenceChangeNotification);

                IsInitialized = true;
            }
        }

        /// <summary>
        /// Called as Fiddler shuts down.
        /// </summary>
        public void OnBeforeUnload()
        {
            // Remove the preference change watcher.
            FiddlerApplication.Prefs.RemoveWatcher(oWatcher);
            // Shutdown telemetery.
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

            LiveTraceService.Instance.ProcessStreamedSessions(this.session);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExtensionPreferenceChangeNotification(object sender, PrefChangeEventArgs e)
        {
            // Update the menu UI when the preference changes.
            MenuUI.Instance.UpdateUIControls();

            // Update the context menu UI when the preference changes.
            ContextMenuUI.Instance.UpdateUIControls();

            // REVIEW THIS 2.20.2025: The below doesn't work. Only a direct interaction with the tabpage updates it.
            // Currently sitting in Office365TabPage.ExtensionEnabledCheckBox_CheckedChanged 
            // to make it work.
            //Office365TabPage.Instance.UpdateUIControls();
            //Office365FiddlerExtensionTabPage.Instance.UpdateOPage();
        }
    }
}
