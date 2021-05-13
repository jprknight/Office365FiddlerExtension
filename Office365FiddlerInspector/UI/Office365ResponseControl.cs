﻿using System.Windows.Forms;
using Fiddler;

namespace O365FiddlerInspector.UI
{
    public partial class Office365ResponseControl : UserControl
    {
        //public static TextBox ResultsOutput { get; set; }
        public static WebBrowser ResultsOutput { get; set; }
        public Office365ResponseControl()
        {
            InitializeComponent();
           
            //ResultsOutput = ResultsDisplay;
            ResultsOutput = webBrowserControl;
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

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}