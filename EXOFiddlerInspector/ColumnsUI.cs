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
        public bool bElapsedTimeColumnCreated = false;
        public bool bResponseServerColumnCreated = false;
        public bool bExchangeTypeColumnCreated = false;
        public bool bXHostIPColumnCreated = false;
        public bool bAuthColumnCreated = false;
        public bool bColumnsOrdered = false;

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

        public void AddAllEnabledColumns()
        {
            this.EnsureElapsedTimeColumn();
            this.EnsureResponseServerColumn();
            this.EnsureResponseServerColumn();
            this.EnsureXHostIPColumn();
            this.EnsureExchangeTypeColumn();
            this.EnsureAuthColumn();
        }

        /// <summary>
        /// Ensure the Response Time Column has been created, return if it has.
        /// </summary>
        public void EnsureElapsedTimeColumn()
        {
            Boolean LoadSaz = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.LoadSaz", false);

            if (bElapsedTimeColumnCreated) return;

            if (LoadSaz && bExtensionEnabled)
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
            if (bResponseServerColumnCreated) return;
            
            if (bExtensionEnabled)
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
            if (bXHostIPColumnCreated) return;

            if (bExtensionEnabled)
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
            if (bExchangeTypeColumnCreated) return;
            
            if (bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Exchange Type", 2, 150, "X-ExchangeType");
                bExchangeTypeColumnCreated = true;
            }
        }

        public void EnsureAuthColumn()
        {
            if (bAuthColumnCreated) return;
            
            if (bExtensionEnabled)
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

            this.AddAllEnabledColumns();
            this.OrderColumns();
            
        }

        public void OrderColumns()
        {
            if (bColumnsOrdered) return;

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

            bColumnsOrdered = true;

        }

        public void OnBeforeReturningError(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void OnLoad()
        {
            this.AddAllEnabledColumns();
            this.OrderColumns();
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