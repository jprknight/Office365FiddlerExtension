using System;
using System.Windows.Forms;
using Fiddler;
using System.Linq;
using System.Xml;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;
using EXOFiddlerInspector.Services;

namespace EXOFiddlerInspector
{
    public class SessionProcessor : ActivationService
    {
        public SessionProcessor()
        {
            //Setting to false by default.
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.LoadSaz", false);

            // Call function to start LoadSAZ if enabled.
            if (Preferences.ExtensionEnabled)
            {
                FiddlerApplication.OnLoadSAZ += HandleLoadSaz;
            }
            // If not enabled call the function to order columns (restore process column back to position 8).
            else
            {
                ColumnsUI.Instance.OrderColumns();
            }
        }
      
        #region LoadSAZ
        /////////////////
        // 
        // Handle loading a SAZ file.
        //
        private void HandleLoadSaz(object sender, FiddlerApplication.ReadSAZEventArgs e)
        {
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.LoadSaz", true);

            ColumnsUI.Instance.AddAllEnabledColumns();
            ColumnsUI.Instance.OrderColumns();

            FiddlerApplication.UI.lvSessions.BeginUpdate();

            foreach (var session in e.arrSessions)
            {

                // Populate the ElapsedTime column on load SAZ, if the column is enabled, and the extension is enabled.
                if (Preferences.ElapsedTimeColumnEnabled)
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
                if (Preferences.ExchangeTypeColumnEnabled && Preferences.ExtensionEnabled)
                {
                    SessionRuleSet.Instance.SetExchangeType(session);
                }

                // Populate the ResponseServer column on load SAZ, if the column is enabled, and the extension is enabled
                if (Preferences.ResponseServerColumnEnabled && Preferences.ExtensionEnabled)
                {
                    SessionRuleSet.Instance.SetResponseServer(session);
                }

                // Populate the Authentication column on load SAZ, if the column is enabled, and the extension is enabled
                if (Preferences.AuthColumnEnabled && Preferences.ExtensionEnabled)
                {
                    SessionRuleSet.Instance.SetAuthentication(session);
                }

                if (Preferences.ExtensionEnabled)
                {
                    // Colourise sessions on load SAZ.
                    SessionRuleSet.Instance.OnPeekAtResponseHeaders(session); //Run whatever function you use in IAutoTamper
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

            ColumnsUI.Instance.AddAllEnabledColumns();
            ColumnsUI.Instance.OrderColumns();

            /////////////////
            //
            // Call the function to colourise sessions for live traffic capture.
            //
            // Making sure this is called after SetExchangeType and SetResponseServer, so we can use overrides
            // in OnPeekAtResponseHeaders function.
            //
            if (Preferences.ExtensionEnabled)
            {
                SessionRuleSet.Instance.OnPeekAtResponseHeaders(session);
                session.RefreshUI();
            }            
        }
        
        public void OnBeforeReturningError(Session oSession) { }
    }
}