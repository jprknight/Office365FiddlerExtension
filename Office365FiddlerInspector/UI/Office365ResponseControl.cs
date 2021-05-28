using System.Windows.Forms;
using Fiddler;

namespace O365FiddlerInspector.UI
{
    public partial class Office365ResponseControl : UserControl
    {
        public static WebBrowser ResultsOutput { get; set; }
        public Office365ResponseControl()
        {
            InitializeComponent();
           
            ResultsOutput = webBrowserControl;
        }
        private void ResetPrefs()
        {
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.enabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.ColumnsEnableAll");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.DemoMode");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.DemoModeBreakScenarios");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.ElapsedTimeColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.ResponseServerColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.ExchangeTypeColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.AppLoggingEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.HighlightOutlookOWAOnlyEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.ExecutionCount");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.ManualCheckForUpdate");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.MenuTitle");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.HostIPColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.AuthColumnEnabled");

            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.enabled");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.ColumnsEnableAll");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.DemoMode");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.DemoModeBreakScenarios");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.ElapsedTimeColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.ResponseServerColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.ExchangeTypeColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.AppLoggingEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.HighlightOutlookOWAOnlyEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.ExecutionCount");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.ManualCheckForUpdate");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.MenuTitle");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.HostIPColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.AuthColumnEnabled");
        }

        private void webBrowserControl_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
