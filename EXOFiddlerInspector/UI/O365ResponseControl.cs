using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fiddler;

namespace O365FiddlerInspector.UI
{
    public partial class O365ResponseControl : UserControl
    {
        public static TextBox ResultsOutput { get; set; }
        public O365ResponseControl()
        {
            InitializeComponent();
           
            ResultsOutput = ResultsDisplay;

        }
        private void ResetPrefs()
        {
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
    }
}
