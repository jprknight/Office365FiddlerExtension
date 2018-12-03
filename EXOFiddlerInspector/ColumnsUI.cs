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
    class ColumnsUI : IAutoTamper
    {
        public bool bElapsedTimeColumnCreated = false;
        public bool bResponseServerColumnCreated = false;
        public bool bExchangeTypeColumnCreated = false;
        public bool bXHostIPColumnCreated = false;
        public bool bAuthColumnCreated = false;

        public Boolean bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);
        public Boolean bElapsedTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ElapsedTimeColumnEnabled", false);
        public Boolean bResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
        public Boolean bExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", false);
        public Boolean bXHostIPColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.XHostIPColumnEnabled", false);
        public Boolean bAuthColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AuthColumnEnabled", false);
        public Boolean bAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", false);
        public Boolean bHighlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled", false);
        public int iExecutionCount = FiddlerApplication.Prefs.GetInt32Pref("extensions.EXOFiddlerInspector.ExecutionCount", 0);

        public int wordCount = 0;

        internal Session session { get; set; }

        /// <summary>
        /// Ensure the Response Time Column has been created, return if it has.
        /// </summary>
        public void EnsureElapsedTimeColumn()
        {
            Boolean LoadSaz = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.LoadSaz", false);

            if (bElapsedTimeColumnCreated && bExtensionEnabled)
            {
                return;
            }
            else if (LoadSaz && bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Elapsed Time", 2, 110, "X-ElapsedTime");
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
            if (bResponseServerColumnCreated && bExtensionEnabled)
            {
                return;
            }
            else if (bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Response Server", 2, 130, "X-ResponseServer");
                bResponseServerColumnCreated = true;
            }
            
        }

        /// <summary>
        ///  Ensure the X-HostIP column has been created, return if it has.
        /// </summary>
        public void EnsureXHostIPColumn()
        {
            if (bXHostIPColumnCreated && bExtensionEnabled)
            {
                return;
            }
            else if (bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.AddBoundColumn("X-HostIP", 2, 110, "X-HostIP");
                bXHostIPColumnCreated = true;
            }
        }

        /// <summary>
        /// Ensure the Exchange Type Column has been created, return if it has.
        /// </summary>
        public void EnsureExchangeTypeColumn()
        {
            if (bExchangeTypeColumnCreated)
            {
                return;
            }
            else if (bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Exchange Type", 2, 150, "X-ExchangeType");
                bExchangeTypeColumnCreated = true;
            }
        }

        public void EnsureAuthColumn()
        {
            if (bAuthColumnCreated)
            {
                return;
            }
            else if (bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Authentication", 2, 140, "X-Authentication");
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
            this.session = session;

            /////////////////
            // Add in the Auth column. Due to these columns all being added as in with priority of 2,
            // they are added into the interface in this reverse order.
            if (bAuthColumnEnabled && bExtensionEnabled)
            {
                this.EnsureAuthColumn();
            }

            /////////////////
            // Add in the Response Server column. Due to these columns all being added as in with priority of 2,
            // they are added into the interface in this reverse order.
            if (bResponseServerColumnEnabled && bExtensionEnabled)
            {
                this.EnsureResponseServerColumn();
            }

            /////////////////
            // Add in the X-HostIP column. Due to these columns all being added as in with priority of 2,
            // they are added into the interface in this reverse order.
            if (bXHostIPColumnEnabled && bExtensionEnabled)
            {
                this.EnsureXHostIPColumn();
            }

            /////////////////
            // Add in the Exchange Type column. Due to these columns all being added as in with priority of 2,
            // they are added into the interface in this reverse order.
            if (bExchangeTypeColumnEnabled && bExtensionEnabled)
            {
                this.EnsureExchangeTypeColumn();
            }

            /////////////////
            // Add in the Elapsed Time column. Due to these columns all being added as in with priority of 2,
            // they are added into the interface in this reverse order.
            if (bElapsedTimeColumnEnabled && bExtensionEnabled)
            {
                this.EnsureElapsedTimeColumn();
            }

            // These get called on each session, seen strange behaviour on reordering on live trace due 
            // to setting each of these as ordering 2 to ensure column positions regardless of column enabled selections.
            // Use an if statement to fire these once per Fiddler application session.
            if (this.session.id == 1)
            {
                OrderColumns();
            }
        }

        public void OrderColumns()
        {
            if (bExtensionEnabled)
            {
                // Move the process column further to the left for visibility.
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Process", 2, -1);
            }
            else
            {
                // Since the extension is not enabled return the process column back to its original location.
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Process", 8, -1);
            }

            if (bExchangeTypeColumnEnabled && bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Exchange Type", 2, -1);
            }

            if (bAuthColumnEnabled && bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Authentication", 2, -1);
            }

            if (bXHostIPColumnEnabled && bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("X-HostIP", 2, -1);
            }

            if (bResponseServerColumnEnabled && bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Response Server", 2, -1);
            }

            if (bElapsedTimeColumnEnabled && bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Elapsed Time", 2, -1);
            }
        }

        public void OnBeforeReturningError(Session oSession)
        {
            //throw new NotImplementedException();
        }

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
                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: ColumnsUI.cs OnLoad Extension Return.");
                    return;
                }
            }
            */
            
            /////////////////
            /// <remarks>
            /// Response Time column function is no longer called here. Only in OnLoadSAZ.
            /// </remarks>
            /////////////////

            /////////////////
            /// <remarks>
            /// Call to function in ColumnsUI.cs to add Server Response column if the menu item is checked and if the extension is enabled.
            /// </remarks> 
            /// Refresh variable now to take account of first load code.
            //bResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
            EnsureResponseServerColumn();
            ///
            /////////////////

            /////////////////
            /// <remarks>
            /// Call to function in ColumnsUI.cs to add Exchange Type column if the menu item is checked and if the extension is enabled. 
            /// </remarks>
            /// Refresh variable now to take account of first load code.
            //bXHostIPColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.XHostIPColumnEnabled", false);
            EnsureXHostIPColumn();
            ///
            /////////////////

            /////////////////
            /// <remarks>
            /// Call to function in ColumnsUI.cs to add Exchange Type column if the menu item is checked and if the extension is enabled. 
            /// </remarks>
            /// Refresh variable now to take account of first load code.
            EnsureExchangeTypeColumn();
            ///
            /////////////////

            /////////////////
            /// <remarks>
            /// Call to function in ColumnsUI.cs to add Authentication column if the menu item is checked and if the extension is enabled. 
            /// </remarks>
            EnsureAuthColumn();
            ///
            /////////////////
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