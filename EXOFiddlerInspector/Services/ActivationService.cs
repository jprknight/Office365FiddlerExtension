using Fiddler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EXOFiddlerInspector.Services
{
    /// <summary>
    /// Global application initializer.
    /// </summary>
    public partial class ActivationService : IAutoTamper
    {
        internal Session session { get; set; }

        ColumnsUI calledColumnsUI = new ColumnsUI();

        public Boolean bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);
        public Boolean bElapsedTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ElapsedTimeColumnEnabled", false);
        public Boolean bResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
        public Boolean bExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", false);
        public Boolean bXHostIPColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.XHostIPColumnEnabled", false);
        public Boolean bAuthColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AuthColumnEnabled", false);
        public Boolean bAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", false);
        public Boolean bHighlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled", false);
        public int iExecutionCount = FiddlerApplication.Prefs.GetInt32Pref("extensions.EXOFiddlerInspector.ExecutionCount", 0);

        public async void OnLoad()
        {
            await TelemetryService.InitializeAsync();
        }

        public async void OnBeforeUnload()
        {
            await TelemetryService.FlushClientAsync();
        }

        public void AutoTamperRequestAfter(Session oSession) { }

        public void AutoTamperRequestBefore(Session oSession) { }

        public void AutoTamperResponseAfter(Session oSession) { }

        public void AutoTamperResponseBefore(Session session) { }

        public void OnBeforeReturningError(Session oSession) { }

        public static string GetAppVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersionInfo.FileVersion;
        }
    }
}
