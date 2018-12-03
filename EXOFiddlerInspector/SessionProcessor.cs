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

        public Boolean bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);
        public Boolean bElapsedTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ElapsedTimeColumnEnabled", false);
        public Boolean bResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
        public Boolean bExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", false);
        public Boolean bXHostIPColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.XHostIPColumnEnabled", false);
        public Boolean bAuthColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AuthColumnEnabled", false);
        public Boolean bAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", false);
        public Boolean bHighlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled", false);
        public int iExecutionCount = FiddlerApplication.Prefs.GetInt32Pref("extensions.EXOFiddlerInspector.ExecutionCount", 0);

        /////////////////

        #region OnLoad
        /////////////////
        //
        // OnLoad
        //
        public void OnLoad()
        {
            // We need to through some code to restore vanilla Fiddler configuration.
            /*
            bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);
            
            // Kill extension if not enabled.
            if (!(bExtensionEnabled))
            {
                // If the Fiddler application preference ExecutionCount exists and has a value, then this
                // is not a first run scenario. Go ahead and return, extension is not enabled.
                if (iExecutionCount > 0)
                {
                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: ColouriseWebSessions.cs OnLoad Extension Return.");
                    return;
                }
            }
            */
            
            // Check if this is the first run of the extension and if so hight up all features.
            calledMenuUI.FirstRunEnableMenuOptions();

            // Check for update. Do this first as we alter the Exchange Online menu title according to
            // whether an update is available.
            CheckForAppUpdate calledCheckForAppUpdate = new CheckForAppUpdate();
            calledCheckForAppUpdate.CheckForUpdate();

            // Developer list is actually set in Preferences.cs.
            List<string> calledDeveloperList = calledPreferences.GetDeveloperList();
            Boolean DeveloperDemoMode = calledPreferences.GetDeveloperMode();
            Boolean DeveloperDemoModeBreakScenarios = calledPreferences.GetDeveloperDemoModeBreakScenarios();

            /////////////////
            // Make sure that even if these are mistakenly left on from debugging, production users are not impacted.
            if (calledDeveloperList.Any(Environment.UserName.Contains) && DeveloperDemoMode == true)
            {
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.DemoMode", true);
            }
            else if (calledDeveloperList.Any(Environment.UserName.Contains) && DeveloperDemoMode == false)
            {
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.DemoMode", false);
            }

            if (calledDeveloperList.Any(Environment.UserName.Contains) && DeveloperDemoModeBreakScenarios == true)
            {
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.DemoModeBreakScenarios", true);
            }
            else if (calledDeveloperList.Any(Environment.UserName.Contains) && DeveloperDemoModeBreakScenarios == false)
            {
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.DemoModeBreakScenarios", false);
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
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.LoadSaz", true);

            /////////////////
            // Add in the Auth column. Due to these columns all being added as in with priority of 2,
            // they are added into the interface in this reverse order.
            if (bAuthColumnEnabled && bExtensionEnabled)
            {
                calledColumnsUI.EnsureAuthColumn();
            }

            /////////////////
            // Add in the Response Server column. Due to these columns all being added as in with priority of 2,
            // they are added into the interface in this reverse order.
            if (bResponseServerColumnEnabled && bExtensionEnabled)
            {
                calledColumnsUI.EnsureResponseServerColumn();
            }

            /////////////////
            // Add in the X-HostIP column. Due to these columns all being added as in with priority of 2,
            // they are added into the interface in this reverse order.
            if (bXHostIPColumnEnabled && bExtensionEnabled)
            {
                calledColumnsUI.EnsureXHostIPColumn();
            }

            /////////////////
            // Add in the Exchange Type column. Due to these columns all being added as in with priority of 2,
            // they are added into the interface in this reverse order.
            if (bExchangeTypeColumnEnabled && bExtensionEnabled)
            {
                calledColumnsUI.EnsureExchangeTypeColumn();
            }

            /////////////////
            // Add in the Elapsed Time column. Due to these columns all being added as in with priority of 2,
            // they are added into the interface in this reverse order.
            if (bElapsedTimeColumnEnabled && bExtensionEnabled)
            {
                calledColumnsUI.EnsureElapsedTimeColumn();
            }

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
            //
            /////////////////

            // These get called on each session, seen strange behaviour on reordering on live trace due 
            // to setting each of these as ordering 2 to ensure column positions regardless of column enabled selections.
            // Use an if statement to fire these once per Fiddler application session.
            if (this.session.id == 1)
            {
                calledColumnsUI.OrderColumns();
            }

        }
        //
        /////////////////////////////
        
        public void OnBeforeReturningError(Session oSession) { }
    }
}