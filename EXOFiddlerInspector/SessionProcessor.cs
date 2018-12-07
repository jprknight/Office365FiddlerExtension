using System;
using System.Windows.Forms;
using Fiddler;
using System.Linq;
using System.Xml;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;

namespace EXOFiddlerInspector
{
    /// <summary>
    /// SessionProcessor containing:
    /// -- OnLoad
    /// -- HandleLoadSaz
    /// </summary>
    public class SessionProcessor : IAutoTamper    // Ensure class is public, or Fiddler won't see it!
    {
        /// <summary>
        /// References to other classes.
        /// </summary>
        MenuUI calledMenuUI = new MenuUI();
        ColumnsUI calledColumnsUI = new ColumnsUI();
        // Developer list is actually set in Preferences.cs.
        Preferences calledPreferences = new Preferences();
        SessionRuleSet calledSessionRuleSet = new SessionRuleSet();
        ///
        /////////////////

        internal Session session { get; set; }

        public Boolean bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.enabled", false);
        public Boolean bElapsedTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.ElapsedTimeColumnEnabled", false);
        public Boolean bResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.ResponseServerColumnEnabled", false);
        public Boolean bExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.ExchangeTypeColumnEnabled", false);
        public Boolean bHostIPColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.HostIPColumnEnabled", false);
        public Boolean bAuthColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.AuthColumnEnabled", false);
        public Boolean bAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.AppLoggingEnabled", false);
        public Boolean bHighlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.HighlightOutlookOWAOnlyEnabled", false);
        public int iExecutionCount = FiddlerApplication.Prefs.GetInt32Pref("extensions.EXOFiddlerExtension.ExecutionCount", 0);

        /////////////////

        #region OnLoad
        /////////////////
        //
        // OnLoad
        //
        public void OnLoad()
        {
            // Set this to false, LoadSaz will set it to true as needed.
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.LoadSaz", false);

            calledColumnsUI.AddAllEnabledColumns();
            // Comment out, do not think ordering columns works in OnLoad, needed in IAutoTamper.
            //this.OrderColumns();

            // Check if this is the first run of the extension and if so hight up all features.
            calledMenuUI.FirstRunEnableMenuOptions();

            // Check for update. Do this first as we alter the Exchange Online menu title according to
            // whether an update is available.
            // -- Still running into issues with checking for an update on live trace.
            // -- This could be due my system proxy changing on my corp machine.
            // -- Disabling this for now.
            //CheckForAppUpdate calledCheckForAppUpdate = new CheckForAppUpdate();
            //calledCheckForAppUpdate.CheckForUpdate();

            // Developer list is actually set in Preferences.cs.
            List<string> calledDeveloperList = calledPreferences.GetDeveloperList();
            Boolean DeveloperDemoMode = calledPreferences.GetDeveloperMode();
            Boolean DeveloperDemoModeBreakScenarios = calledPreferences.GetDeveloperDemoModeBreakScenarios();

            /////////////////
            // Make sure that even if these are mistakenly left on from debugging, production users are not impacted.
            if (calledDeveloperList.Any(Environment.UserName.Contains) && DeveloperDemoMode == true)
            {
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.DemoMode", true);
            }
            else if (calledDeveloperList.Any(Environment.UserName.Contains) && DeveloperDemoMode == false)
            {
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.DemoMode", false);
            }

            if (calledDeveloperList.Any(Environment.UserName.Contains) && DeveloperDemoModeBreakScenarios == true)
            {
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.DemoModeBreakScenarios", true);
            }
            else if (calledDeveloperList.Any(Environment.UserName.Contains) && DeveloperDemoModeBreakScenarios == false)
            {
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.DemoModeBreakScenarios", false);
            }
            ///
            /////////////////

            /////////////////
            // Throw a message box to alert demo mode is running.
            if (calledDeveloperList.Any(Environment.UserName.Contains) && DeveloperDemoMode == true)
            {
                MessageBox.Show("Developer / Demo mode is running!");
            }
            //
            /////////////////

            /////////////////
            // Call function to start LoadSAZ if enabled.
            if (bExtensionEnabled)
            {
                FiddlerApplication.OnLoadSAZ += HandleLoadSaz;
            }
            // If not enabled call the function to order columns (restore process column back to position 8).
            else
            {
                calledColumnsUI.OrderColumns();
            }
            //
            /////////////////
        }
        //
        /////////////////
        #endregion

        #region LoadSAZ
        /////////////////
        // 
        // Handle loading a SAZ file.
        //
        private void HandleLoadSaz(object sender, FiddlerApplication.ReadSAZEventArgs e)
        {
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.LoadSaz", true);

            calledColumnsUI.AddAllEnabledColumns();
            calledColumnsUI.OrderColumns();

            FiddlerApplication.UI.lvSessions.BeginUpdate();

            foreach (var session in e.arrSessions)
            {

                // Populate the ElapsedTime column on load SAZ, if the column is enabled, and the extension is enabled.
                if (bElapsedTimeColumnEnabled)
                {
                    if (session.Timers.ClientBeginRequest.ToString("H:mm:ss.fff") == "0:00:00.000" || session.Timers.ClientDoneResponse.ToString("H:mm:ss.fff") == "0:00:00.000")
                    {
                        session["X-ElapsedTime"] = "No Data";
                    }
                    /*else if (session.Timers.ServerDoneResponse.ToString("H:mm:ss.fff") == "0:00:00.000" || session.Timers.ServerDoneResponse.ToString("yyyy/MM/dd") == "0001/01/01")
                    {
                        session["X-ElapsedTime"] = "No Data";
                    }*/
                    else
                    {
                        double Milliseconds = Math.Round((session.Timers.ClientDoneResponse - session.Timers.ClientBeginRequest).TotalMilliseconds);

                        if (Milliseconds < 1000)
                        {
                            session["X-ElapsedTime"] = Milliseconds + "ms";
                        }
                        else if (Milliseconds >= 1000 && Milliseconds < 2000)
                        {
                            session["X-ElapsedTime"] = Math.Round((session.Timers.ClientDoneResponse - session.Timers.ClientBeginRequest).TotalSeconds) + " second";
                        }
                        else
                        {
                            session["X-ElapsedTime"] = Math.Round((session.Timers.ClientDoneResponse - session.Timers.ClientBeginRequest).TotalSeconds) + " seconds";
                        }
                        //session["X-ElapsedTime"] = session.oResponse.iTTLB.ToString() + "ms";
                    }
                }

                // Populate the ExchangeType column on load SAZ, if the column is enabled, and the extension is enabled
                if (bExchangeTypeColumnEnabled && bExtensionEnabled)
                {
                    calledSessionRuleSet.SetExchangeType(session);
                }

                // Populate the ResponseServer column on load SAZ, if the column is enabled, and the extension is enabled
                if (bResponseServerColumnEnabled && bExtensionEnabled)
                {
                    calledSessionRuleSet.SetResponseServer(session);
                }

                // Populate the Authentication column on load SAZ, if the column is enabled, and the extension is enabled
                if (bAuthColumnEnabled && bExtensionEnabled)
                {
                    calledSessionRuleSet.SetAuthentication(session);
                }

                if (bExtensionEnabled)
                {
                    // Colourise sessions on load SAZ.
                    calledSessionRuleSet.OnPeekAtResponseHeaders(session); //Run whatever function you use in IAutoTamper
                    session.RefreshUI();
                }
            }
            FiddlerApplication.UI.lvSessions.EndUpdate();
        }
        //
        /////////////////
        #endregion

        public void OnBeforeUnload() { }

        public void AutoTamperRequestBefore(Session oSession) { }

        public void AutoTamperRequestAfter(Session oSession) { }

        public void AutoTamperResponseBefore(Session oSession) { }

        /// <summary>
        /// Calling OnPeekAtResponseHeaders(session) to process session for live traffic capture.
        /// </summary>
        /// <param name="session"></param>
        public void AutoTamperResponseAfter(Session session)
        {
            this.session = session;

            calledColumnsUI.AddAllEnabledColumns();
            calledColumnsUI.OrderColumns();

            /////////////////
            //
            // Call the function to colourise sessions for live traffic capture.
            //
            // Making sure this is called after SetExchangeType and SetResponseServer, so we can use overrides
            // in OnPeekAtResponseHeaders function.
            //
            if (bExtensionEnabled)
            {
                calledSessionRuleSet.OnPeekAtResponseHeaders(session);
                session.RefreshUI();
            }            
        }
        
        public void OnBeforeReturningError(Session oSession) { }
    }
}