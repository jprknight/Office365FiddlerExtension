using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXOFiddlerInspector
{
    /// <summary>
    /// Telemetry IAutoTamper.
    /// </summary>
    public class Telemetry : IAutoTamper // Ensure class is public, or Fiddler won't see it!
    {
        Boolean bExtensionEnabled = false;

        public void AutoTamperRequestAfter(Session oSession) { }

        public void AutoTamperRequestBefore(Session oSession) { }

        public void AutoTamperResponseAfter(Session oSession) { }

        public void AutoTamperResponseBefore(Session oSession) { }

        public void OnBeforeReturningError(Session oSession) { }

        public void OnBeforeUnload() { }

        /// <summary>
        /// OnLoad Telemetry call if extension is enabled.
        /// </summary>
        public void OnLoad()
        {
            this.bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);

            if (bExtensionEnabled)
            {
                FiddlerApplication.Log.LogString("EXOFiddlerExtention: Extension OnLoad event.");
                /// <remarks>
                /// Telemtry code lives here.
                /// </remarks>
                /// 
            }
        }
    }
}
