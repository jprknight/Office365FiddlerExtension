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

namespace EXOFiddlerInspector.UI
{
    public partial class ExchangeResponseControl : UserControl
    {
        public static TextBox ResultsOutput { get; set; }
        public ExchangeResponseControl()
        {
            InitializeComponent();
           
            ResultsOutput = ResultsDisplay;

        }
        private void ResetPrefs()
        {
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.enabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.ColumnsEnableAll");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.DemoMode");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.DemoModeBreakScenarios");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.ElapsedTimeColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.AppLoggingEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.ExecutionCount");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.ManualCheckForUpdate");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.MenuTitle");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.HostIPColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.AuthColumnEnabled");
        }
    }
}
