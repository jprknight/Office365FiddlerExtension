using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerInspector.Services
{
    internal class ExtensionURLProcessor
    {
        public void CheckExtensionURLs()
        {
            // If disable web calls is set, don't look for any URL updates.
            if (Preferences.DisableWebCalls)
            {
                GetSetSessionFlags.Instance.WriteToFiddlerLogNoSession($"DisableWebCalls is enabled, no extension URLs update check performed.");
                return;
            }

            // If using the beta rule set use that last updated to determine if we check for updates or not.
            if (Preferences.BetaRuleSet)
            {
                if (DateTime.Now < Properties.Settings.Default.LocalMasterRulesetLastUpdated)
                {
                    GetSetSessionFlags.Instance.WriteToFiddlerLogNoSession($"Rules have been checked within the last 24 hours, no extension URLs update check performed.");
                    return;
                }
            }
            // If using the master rule set use that last updated to determine if we check for updates or not.
            else
            {
                if (DateTime.Now < Properties.Settings.Default.LocalBetaRulesetLastUpdated)
                {
                    GetSetSessionFlags.Instance.WriteToFiddlerLogNoSession($"Rules have been checked within the last 24 hours, no extension URLs update check performed.");
                    return;
                }
            }
                





            var ExtensionSessionFlags = JsonConvert.DeserializeObject<SessionFlagProcessor.ExtensionSessionFlags>(this.session["Microsoft365FiddlerExtensionJson"]);


        }
    }
}
