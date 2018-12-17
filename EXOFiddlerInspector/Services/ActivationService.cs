using Fiddler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EXOFiddlerInspector.Services
{
    /// <summary>
    /// Global application initializer.
    /// </summary>
    public abstract class ActivationService : IAutoTamper
    {
        internal Session session { get; set; }
      
        /// <summary>
        /// This should be consider the main constructor for the app.
        /// </summary>
        public async void OnLoad()
        {
            await TelemetryService.InitializeAsync();

            MenuUI.Instance.FirstRunEnableMenuOptions();
            
            // Throw a message box to alert demo mode is running.
            if (Preferences.GetDeveloperMode())
            {
                MessageBox.Show("Developer / Demo mode is running!");
            }

        }

        public async void OnBeforeUnload()
        {
            await TelemetryService.FlushClientAsync();
        }

        public void AutoTamperRequestAfter(Session oSession) { }

        public void AutoTamperRequestBefore(Session oSession) { }

        public void AutoTamperResponseAfter(Session oSession)
        {
            ColumnsUI.Instance.AddAllEnabledColumns();
            ColumnsUI.Instance.OrderColumns();

            // Call the function to populate the session type column on live trace, if the column is enabled.
            if (Preferences.ExchangeTypeColumnEnabled && Preferences.ExtensionEnabled)
            {
                SessionRuleSet.Instance.SetExchangeType(this.session);
            }

            // Call the function to populate the session type column on live trace, if the column is enabled.
            if (Preferences.ResponseServerColumnEnabled && Preferences.ExtensionEnabled)
            {
                SessionRuleSet.Instance.SetResponseServer(this.session);
            }

            // Call the function to populate the Authentication column on live trace, if the column is enabled.
            if (Preferences.AuthColumnEnabled && Preferences.ExtensionEnabled)
            {
                SessionRuleSet.Instance.SetAuthentication(this.session);
            }
        }

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
