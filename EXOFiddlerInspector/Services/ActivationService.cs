using Fiddler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXOFiddlerInspector.Services
{
    /// <summary>
    /// Global application initializer.
    /// </summary>
    public class ActivationService : IAutoTamper
    {
        internal Session session { get; set; }

        public bool bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);
        public bool bElapsedTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ElapsedTimeColumnEnabled", false);
        public bool bResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
        public bool bExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", false);
        public bool bXHostIPColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.XHostIPColumnEnabled", false);
        public bool bAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", false);
        public bool bHighlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled", false);
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

        public void AutoTamperResponseBefore(Session session) {

            this.session = session;

            ColumnsUI calledColumnsUI = new ColumnsUI();

            /////////////////
            //
            // Call the function to populate the session type column on live trace, if the column is enabled.
            if (bExchangeTypeColumnEnabled)
            {
                calledColumnsUI.SetExchangeType(this.session);
            }

            /////////////////
            //
            // Call the function to populate the session type column on live trace, if the column is enabled.
            if (bResponseServerColumnEnabled)
            {
                calledColumnsUI.SetResponseServer(this.session);
            }

            // These get called on each session, seen strange behaviour on reordering on live trace due 
            // to setting each of these as ordering 2 to ensure column positions regardless of column enabled selections.
            // Use an if statement to fire these once per Fiddler application session.
            if (this.session.id == 1)
            {
                calledColumnsUI.OrderColumns();
            }
        }

        public void OnBeforeReturningError(Session oSession) { }

    }
}
