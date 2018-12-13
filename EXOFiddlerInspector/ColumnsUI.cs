using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fiddler;

namespace EXOFiddlerInspector
{
    public class ColumnsUI : IAutoTamper
    {
        Preferences calledPreferences = new Preferences();

        public bool bElapsedTimeColumnCreated = false;
        public bool bResponseServerColumnCreated = false;
        public bool bExchangeTypeColumnCreated = false;
        public bool bHostIPColumnCreated = false;
        public bool bAuthColumnCreated = false;
        public bool bColumnsOrdered = false;

        int iResponseServerColumnOrderCount = 0;
        int iHostIPColumnOrderCount = 0;
        int iAuthColumnOrderCount = 0;
        int iExchangeTypeColumnOrderCount = 0;
        int iElapsedTimeColumnOrderCount = 0;

        public Boolean bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.enabled", false);
        public Boolean bElapsedTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.ElapsedTimeColumnEnabled", false);
        public Boolean bResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.ResponseServerColumnEnabled", false);
        public Boolean bExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.ExchangeTypeColumnEnabled", false);
        public Boolean bHostIPColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.HostIPColumnEnabled", false);
        public Boolean bAuthColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.AuthColumnEnabled", false);
        public Boolean bAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.AppLoggingEnabled", false);
        public Boolean bHighlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.HighlightOutlookOWAOnlyEnabled", false);
        public int iExecutionCount = FiddlerApplication.Prefs.GetInt32Pref("extensions.EXOFiddlerExtension.ExecutionCount", 0);

        public int wordCount = 0;

        internal Session session { get; set; }

        public void AddAllEnabledColumns()
        {
            if (FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.LoadSaz", false))
            {
                this.EnsureElapsedTimeColumn();
                this.EnsureResponseServerColumn();
                this.EnsureHostIPColumn();
                this.EnsureExchangeTypeColumn();
                this.EnsureAuthColumn();
            }
        }

        /// <summary>
        /// Ensure the Response Time Column has been created, return if it has.
        /// </summary>
        public void EnsureElapsedTimeColumn()
        {
            if (!(FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.LoadSaz", false))) return;

            Boolean LoadSaz = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.LoadSaz", false);

            if (bElapsedTimeColumnCreated) return;

            if (LoadSaz && bElapsedTimeColumnEnabled && bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Elapsed Time", 110, "X-ElapsedTime");
                bElapsedTimeColumnCreated = true;
            }
            else if (bExtensionEnabled)
            {
                // live trace, don't load this column.
                // Testing.
                //FiddlerApplication.UI.lvSessions.AddBoundColumn("Elapsed Time", 2, 110, "X-ElapsedTime");
                //bElapsedTimeColumnCreated = true;
            }
        }

        /// <summary>
        ///  Ensure the Response Server column has been created, return if it has.
        /// </summary>
        public void EnsureResponseServerColumn()
        {
            if (!(FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.LoadSaz", false))) return;

            if (bResponseServerColumnCreated) return;
            
            if (bResponseServerColumnEnabled && bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Response Server", 130, "X-ResponseServer");
                bResponseServerColumnCreated = true;
            }
            
        }

        /// <summary>
        ///  Ensure the HostIP column has been created, return if it has.
        /// </summary>
        public void EnsureHostIPColumn()
        {
            if (!(FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.LoadSaz", false))) return;

            if (bHostIPColumnCreated) return;

            if (bHostIPColumnEnabled && bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Host IP", 110, "X-HostIP");
                bHostIPColumnCreated = true;
            }
        }

        /// <summary>
        /// Ensure the Exchange Type Column has been created, return if it has.
        /// </summary>
        public void EnsureExchangeTypeColumn()
        {
            if (!(FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.LoadSaz", false))) return;

            if (bExchangeTypeColumnCreated) return;
            
            if (bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Exchange Type", 150, "X-ExchangeType");
                bExchangeTypeColumnCreated = true;
            }
        }

        public void EnsureAuthColumn()
        {
            if (!(FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.LoadSaz", false))) return;

            if (bAuthColumnCreated) return;
            
            if (bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Authentication", 140, "X-Authentication");
                bAuthColumnCreated = true;
            }
        }

        public void AutoTamperRequestBefore(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void AutoTamperRequestAfter(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void AutoTamperResponseBefore(Session session)
        {
            //throw new NotImplementedException();
        }

        public void AutoTamperResponseAfter(Session session)
        {
            this.AddAllEnabledColumns();
            this.OrderColumns();
        }

        public void OrderColumns()
        {
            // The below was being used to control only one execution per session.
            // However on testing it seems like this does need to be run multiple times in IAutoTamper for proper
            // column ordering.
            // Further testing on client machine and I see the columns are jumping all over the place.
            // Throwing an execution limit around the custom columns all being added in column position "2".

            // Column ordering threshold. For some reason we need IAutoTamper AutoTamperResponseAfter to hit this function
            // more than just once to get consistent column positioning.
            // Setting a threshold here.
            int iColumnOrderingThreshold = 5;

            // 1.0.61 Require LoadSaz for session ordering.
            if (bExtensionEnabled && FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.LoadSaz", false))
            {
                // Count the columns
                int iColumnsCount = FiddlerApplication.UI.lvSessions.Columns.Count;
                
                // Keep session id and result in the standard location on the left.
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("#", 0, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Result", 1, -1);

                // Set extension added columns here all with a calue of 2 to account for some being enabled.
                if (bResponseServerColumnEnabled && bExtensionEnabled && iResponseServerColumnOrderCount <= iColumnOrderingThreshold)
                {
                    FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Response Server", 2, -1);
                    iResponseServerColumnOrderCount++;
                }
                if (bHostIPColumnEnabled && bExtensionEnabled && iHostIPColumnOrderCount <= iColumnOrderingThreshold)
                {
                    FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Host IP", 2, -1);
                    iHostIPColumnOrderCount++;
                }
                if (bAuthColumnEnabled && bExtensionEnabled && iAuthColumnOrderCount <= iColumnOrderingThreshold)
                {
                    FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Authentication", 2, -1);
                    iAuthColumnOrderCount++;
                }
                if (bExchangeTypeColumnEnabled && bExtensionEnabled && iExchangeTypeColumnOrderCount <= iColumnOrderingThreshold)
                {
                    FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Exchange Type", 2, -1);
                    iExchangeTypeColumnOrderCount++;
                }
                if (bElapsedTimeColumnEnabled && bExtensionEnabled && iElapsedTimeColumnOrderCount <= iColumnOrderingThreshold)
                {
                    FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Elapsed Time", 2, -1);
                    iElapsedTimeColumnOrderCount++;
                }
                
                // Tack the rest on the end using iColumnsCount to avoid out of bounds errors when some columns are disabled.
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Process", iColumnsCount -9, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Protocol", iColumnsCount - 8, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Host", iColumnsCount - 7, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("URL", iColumnsCount - 6, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Body", iColumnsCount - 5, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Caching", iColumnsCount - 4, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Content-Type", iColumnsCount - 3, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Comments", iColumnsCount - 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Custom", iColumnsCount - 1, -1);
            }
            // If the extension is disabled, return the UI to the defaults.
            else
            {

                // Count the columns
                int iColumnsCount = FiddlerApplication.UI.lvSessions.Columns.Count;

                // Move the process column back to its standard position when extension is not enabled.
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Process", iColumnsCount - 2, -1);
            }

            //bColumnsOrdered = true;
        }

        public void OnBeforeReturningError(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void OnLoad()
        {
            // Set this to false to start in a neutral position.
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.LoadSaz", false);
            // Now work out if we are loading a SAZ file or not.
            FiddlerApplication.OnLoadSAZ += calledPreferences.MakeLoadSaz;

            if (FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.LoadSaz", false))
            {
                this.AddAllEnabledColumns();
            }

            // Comment out, do not think ordering columns works in OnLoad, needed in IAutoTamper.
            //this.OrderColumns();
        }

        public void OnBeforeUnload()
        {
            //throw new NotImplementedException();
        }

        // Populate the ElapsedTime column on live trace, if the column is enabled.
        // Code currently not used / under review.

        // if (boolElapsedTimeColumnEnabled && boolExtensionEnabled) {
        // Realised this.session.oResponse.iTTLB.ToString() + "ms" is not the value I want to display as Response Time.
        // More desirable figure is created from:
        // Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds)
        // 
        // For some reason in AutoTamperResponseAfter this.session.Timers.ClientDoneResponse has a default timestamp of 01/01/0001 12:00
        // Messing up any math. By the time the inspector gets to loading the same math.round statement the correct value is displayed in the 
        // inspector Exchange Online tab.
        //
        // This needs more thought, read through Fiddler book some more on what could be happening and whether this can work or if the Response time
        // column is removed from the extension in favour of the response time on the inspector tab.
        //

        // *** For the moment disabled the Response Time column when live tracing. Only displayed on LoadSAZ. ***

        /*
        // Trying out delaying the process, waiting for the ClientDoneResponse to be correctly populated.
        // Did not work out, Fiddler process hangs / very slow.
        while (this.session.Timers.ClientDoneResponse.Year < 2000)
        {
            if (this.session.Timers.ClientDoneResponse.Year > 2000)
            {
                break;
            }
        }
        //session["X-iTTLB"] = this.session.oResponse.iTTLB.ToString() + "ms"; // Known to give inaccurate results.

        //MessageBox.Show("ClientDoneResponse: " + this.session.Timers.ClientDoneResponse + Environment.NewLine + "ClientBeginRequest: " + this.session.Timers.ClientBeginRequest
        //    + Environment.NewLine + "iTTLB: " + this.session.oResponse.iTTLB);
        // The below is not working in a live trace scenario. Reverting back to the previous configuration above as this works for now.
        session["X-iTTLB"] = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds) + "ms";
        */
        //}
    }
}