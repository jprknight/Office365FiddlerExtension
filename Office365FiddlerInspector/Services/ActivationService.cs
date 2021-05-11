using Fiddler;

namespace Office365FiddlerInspector.Services
{
    /// <summary>
    /// Global application initializer.
    /// </summary>
    public abstract class ActivationService : IAutoTamper
    {
        /// <summary>
        /// This should be consider the main constructor for the app. It's called after the UI has loaded.
        /// </summary>
        public async void OnLoad()
        {
            MenuUI.Instance.Initialize();
            if (Preferences.ExecutionCount == 0)
            {
                await Preferences.SetDefaultPreferences();
            }

            SessionProcessor.Instance.Initialize();

            About.Instance.CheckForUpdate();

            FiddlerApplication.UI.lvSessions.AddBoundColumn("Elapsed Time", 110, "X-ElapsedTime");
            FiddlerApplication.UI.lvSessions.AddBoundColumn("Session Type", 150, "X-SessionType");
            FiddlerApplication.UI.lvSessions.AddBoundColumn("Authentication", 140, "X-Authentication");
            FiddlerApplication.UI.lvSessions.AddBoundColumn("Host IP", 110, "X-HostIP");
            FiddlerApplication.UI.lvSessions.AddBoundColumn("Response Server", 130, "X-ResponseServer");
            
            // Throw a message box to alert demo mode is running.
            //if (Preferences.GetDeveloperMode())
            //{
            //    MessageBox.Show("Developer / Demo mode is running!");
            //}
            //else
            //{

            await TelemetryService.InitializeAsync();

            //}
        }

        public async void OnBeforeUnload()
        {
            await TelemetryService.FlushClientAsync();
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
        public void AutoTamperResponseAfter(Session _session)
        {
            if (!Preferences.ExtensionEnabled)
            {
                return;
            }

            // Only do this on loadSAZ?
            SessionProcessor.Instance.SetElapsedTime(_session);

            SessionProcessor.Instance.OnPeekAtResponseHeaders(_session);

            //SessionProcessor.Instance.SetSessionType(_session);

            SessionProcessor.Instance.SetAuthentication(_session);

            SessionProcessor.Instance.SetResponseServer(_session);

            _session.RefreshUI();
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

        //public void SetColumns()
        //{

        //    if (Preferences.ExtensionEnabled)
        //    {
        //        FiddlerApplication.UI.lvSessions.BeginUpdate();

        //        // Only on LoadSAZ add all the columns.
        //        if (FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.LoadSaz", false))
        //        {
        //            FiddlerApplication.UI.lvSessions.AddBoundColumn("Elapsed Time", 110, "X-ElapsedTime");
        //            FiddlerApplication.UI.lvSessions.AddBoundColumn("Response Server", 0, 130, "X-ResponseServer");
        //            FiddlerApplication.UI.lvSessions.AddBoundColumn("Host IP", 0, 110, "X-HostIP");
        //            FiddlerApplication.UI.lvSessions.AddBoundColumn("Session Type", 0, 150, "X-SessionType");
        //            FiddlerApplication.UI.lvSessions.AddBoundColumn("Authentication", 0, 140, "X-Authentication");
        //        }
        //        // On live trace just add in the Host IP column.
        //        else
        //        {
        //            FiddlerApplication.UI.lvSessions.AddBoundColumn("Host IP", 110, "X-HostIP");
        //        }

        //    }
        //    else
        //    {
        //        int iColumnsCount = FiddlerApplication.UI.lvSessions.Columns.Count;
        //        FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Process", iColumnsCount - 2, -1);
        //    }
        //    FiddlerApplication.UI.lvSessions.BeginUpdate();
        //}
    }
}
