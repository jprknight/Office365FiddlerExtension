using Fiddler;
using Office365FiddlerExtension.Services;
using Office365FiddlerExtensionRuleset.Ruleset.HTTP_200s;
using System.Diagnostics;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    /// <summary>
    /// Main for sessions with a HTTP 200 response code.
    /// Many types of sessions come back with a HTTP 200 "OK" response from the server,
    /// but actually contain some error condition in the response.
    /// The classes called here highlight HTTP 200 sessions which are not "OK" and
    /// clears those that are.
    /// This is intended to be the only class what pulls from the namespace ending 
    /// in .HTTP_200s.
    /// </summary>
    /// <param name="session"></param>
    class HTTP_200
    {
        internal Session session { get; set; }

        private static HTTP_200 _instance;

        public static HTTP_200 Instance => _instance ?? (_instance = new HTTP_200());

        /// <summary>
        /// Run the HTTP 200 rulesets against the current session.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            // Do not modify this function. Add new ruleset calls into the private functions below according to when they should be called.
            // Broken the code here out to functions as the ruleset is growing and will continue to grow.

            // First: Mark up connect tunnel sessions as soon as possible in the ruleset.
            ConnectTunnel(this.session);

            // Second: Known scenarios (False Negatives) -- Ruleset identifies traffic which is a known issue to highlight.
            KnownProblemScenarios(this.session);

            // Third: Run known non-problem scenarios (Qualifying HTTP 200 traffic).
            KnownNonProblemScenarios(this.session);

            // Fourth: Identify clients -- Identifies M365 clients.
            IdentifyClients(this.session);

            // Fifth: Actually OK and lurking errors.
            LurkingErrors(this.session);

            // Last: If all other rulesets have not already identified the session, mark the session as actually OK.
            ActuallyOK(this.session);
        }

        /// <summary>
        /// Mark up connect tunnel sessions as soon as possible in the ruleset.
        /// </summary>
        /// <param name="session"></param>
        private void ConnectTunnel(Session session)
        {
            this.session = session;

            var sw_HTTP_200_ConnectTunnelSessions = Stopwatch.StartNew();

            HTTP_200_ConnectTunnelSessions.Instance.Run(this.session);

            sw_HTTP_200_ConnectTunnelSessions.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_ConnectTunnelSessions");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_ConnectTunnelSessions", sw_HTTP_200_ConnectTunnelSessions.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// These rulesets run first in priority to identify known problem scenarios.
        /// </summary>
        /// <param name="session"></param>
        private void KnownProblemScenarios(Session session)
        {
            this.session = session;

            ///////////////////////////////

            var sw_HTTP_200_ClientAccessRule = Stopwatch.StartNew();

            HTTP_200_ClientAccessRule.Instance.Run(this.session);

            sw_HTTP_200_ClientAccessRule.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_ClientAccessRule");
                TelemetryService.CustomTrackEvent("RS_KnownProblemInSession");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_ClientAccessRule", sw_HTTP_200_ClientAccessRule.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_Culture_Not_Found = Stopwatch.StartNew();

            // Call this before any MAPI sessions. If we have a culture error this take precedence.
            HTTP_200_Culture_Not_Found.Instance.Run(this.session);

            sw_HTTP_200_Culture_Not_Found.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_Culture_Not_Found");
                TelemetryService.CustomTrackEvent("RS_KnownProblemInSession");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_Culture_Not_Found", sw_HTTP_200_Culture_Not_Found.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_Outlook_MAPI_Protocol_Disabled = Stopwatch.StartNew();

            HTTP_200_Outlook_MAPI_Protocol_Disabled.Instance.Run(this.session);

            sw_HTTP_200_Outlook_MAPI_Protocol_Disabled.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_Outlook_MAPI_Protocol_Disabled");
                TelemetryService.CustomTrackEvent("RS_KnownProblemInSession");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_Outlook_MAPI_Protocol_Disabled", sw_HTTP_200_Outlook_MAPI_Protocol_Disabled.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_AddressNotFound = Stopwatch.StartNew();

            HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_AddressNotFound.Instance.Run(this.session);

            sw_HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_AddressNotFound.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_AddressNotFound");
                TelemetryService.CustomTrackEvent("RS_KnownProblemInSession");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_AddressNotFound", sw_HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_AddressNotFound.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_FreeBusy = Stopwatch.StartNew();

            HTTP_200_FreeBusy.Instance.Run(this.session);

            sw_HTTP_200_FreeBusy.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_FreeBusy");
                // Do not flag KnownProblemInSession here, there are calls within the FreeBusy functions to do this.
                TelemetryService.CustomTrackMetric("RS_HTTP_200_FreeBusy", sw_HTTP_200_FreeBusy.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_Json = Stopwatch.StartNew();

            HTTP_200_Json.Instance.Run(this.session);

            sw_HTTP_200_Json.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_Json");
                // Do not flag KnownProblemInSession here, there are calls within the Json functions to do this.
                TelemetryService.CustomTrackMetric("RS_HTTP_200_Json", sw_HTTP_200_Json.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// These rulesets run second in priority to identify known non-problem scenarios.
        /// </summary>
        /// <param name="session"></param>
        private void KnownNonProblemScenarios(Session session)
        {
            this.session = session;

            ///////////////////////////////

            var sw_HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_Address_Found = Stopwatch.StartNew();

            HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_Address_Found.Instance.Run(this.session);

            sw_HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_Address_Found.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_Address_Found");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_Address_Found", sw_HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_Address_Found.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun = Stopwatch.StartNew();

            HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun.Instance.Run(this.session);

            sw_HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun", sw_HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun = Stopwatch.StartNew();

            HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun.Instance.Run(this.session);

            sw_HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun", sw_HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_Unified_Groups_Settings = Stopwatch.StartNew();

            HTTP_200_Unified_Groups_Settings.Instance.Run(this.session);

            sw_HTTP_200_Unified_Groups_Settings.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_Unified_Groups_Settings");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_Unified_Groups_Settings", sw_HTTP_200_Unified_Groups_Settings.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_3S_Suggestions = Stopwatch.StartNew();

            HTTP_200_3S_Suggestions.Instance.Run(this.session);

            sw_HTTP_200_3S_Suggestions.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_3S_Suggestions");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_3S_Suggestions", sw_HTTP_200_3S_Suggestions.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_REST_People_Request = Stopwatch.StartNew();

            HTTP_200_REST_People_Request.Instance.Run(this.session);

            sw_HTTP_200_REST_People_Request.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_REST_People_Request");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_REST_People_Request", sw_HTTP_200_REST_People_Request.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_Exchange_OnPremise_Any_Other_EWS = Stopwatch.StartNew();

            HTTP_200_Exchange_OnPremise_Any_Other_EWS.Instance.Run(this.session);

            sw_HTTP_200_Exchange_OnPremise_Any_Other_EWS.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_Exchange_OnPremise_Any_Other_EWS");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_Exchange_OnPremise_Any_Other_EWS", sw_HTTP_200_Exchange_OnPremise_Any_Other_EWS.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_Exchange_Online_Any_Other_EWS = Stopwatch.StartNew();

            HTTP_200_Exchange_Online_Any_Other_EWS.Instance.Run(this.session);

            sw_HTTP_200_Exchange_Online_Any_Other_EWS.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_Exchange_Online_Any_Other_EWS");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_Exchange_Online_Any_Other_EWS", sw_HTTP_200_Exchange_Online_Any_Other_EWS.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_NewOutlook_GetMailTips = Stopwatch.StartNew();

            HTTP_200_New_Outlook_GetMailTips.Instance.Run(this.session);

            sw_HTTP_200_NewOutlook_GetMailTips.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_sw_HTTP_200_NewOutlook_GetMailTips");
                TelemetryService.CustomTrackMetric("RS_sw_HTTP_200_NewOutlook_GetMailTips", sw_HTTP_200_NewOutlook_GetMailTips.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_Outlook_GetMailTips = Stopwatch.StartNew();

            HTTP_200_Outlook_GetMailTips.Instance.Run(this.session);

            sw_HTTP_200_Outlook_GetMailTips.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_sw_HTTP_200_Outlook_GetMailTips");
                TelemetryService.CustomTrackMetric("RS_sw_HTTP_200_Outlook_GetMailTips", sw_HTTP_200_Outlook_GetMailTips.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_Javascript = Stopwatch.StartNew();

            HTTP_200_Javascript.Instance.Run(this.session);

            sw_HTTP_200_Javascript.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_Javascript");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_Javascript", sw_HTTP_200_Javascript.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// These rulesets run third in priority to identify the client making the request.
        /// </summary>
        /// <param name="session"></param>
        private void IdentifyClients(Session session)
        {
            this.session = session;

            ///////////////////////////////

            var sw_HTTP_200_Outlook_MAPI_Exchange_Online = Stopwatch.StartNew();

            HTTP_200_Outlook_MAPI_Exchange_Online.Instance.Run(this.session);

            sw_HTTP_200_Outlook_MAPI_Exchange_Online.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_Outlook_MAPI_Exchange_Online");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_Outlook_MAPI_Exchange_Online", sw_HTTP_200_Outlook_MAPI_Exchange_Online.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_Outlook_MAPI_Exchange_OnPremise = Stopwatch.StartNew();

            HTTP_200_Outlook_MAPI_Exchange_OnPremise.Instance.Run(this.session);

            sw_HTTP_200_Outlook_MAPI_Exchange_OnPremise.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_Outlook_MAPI_Exchange_OnPremise");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_Outlook_MAPI_Exchange_OnPremise", sw_HTTP_200_Outlook_MAPI_Exchange_OnPremise.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_Outlook_RPC = Stopwatch.StartNew();

            HTTP_200_Outlook_RPC.Instance.Run(this.session);

            sw_HTTP_200_Outlook_RPC.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_Outlook_RPC");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_Outlook_RPC", sw_HTTP_200_Outlook_RPC.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_Outlook_NSPI = Stopwatch.StartNew();

            HTTP_200_Outlook_NSPI.Instance.Run(this.session);

            sw_HTTP_200_Outlook_NSPI.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_Outlook_NSPI");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_Outlook_NSPI", sw_HTTP_200_Outlook_NSPI.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_OWA_Notification_Channel = Stopwatch.StartNew();

            HTTP_200_OWA_Notification_Channel.Instance.Run(this.session);

            sw_HTTP_200_OWA_Notification_Channel.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_OWA_Notification_Channel");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_OWA_Notification_Channel", sw_HTTP_200_OWA_Notification_Channel.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_OWA = Stopwatch.StartNew();

            HTTP_200_OWA.Instance.Run(this.session);

            sw_HTTP_200_OWA.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_OWA");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_OWA", sw_HTTP_200_OWA.ElapsedMilliseconds);
            }

            ///////////////////////////////

            var sw_HTTP_200_OWA_Attachments = Stopwatch.StartNew();

            HTTP_200_OWA_Attachments.Instance.Run(this.session);

            sw_HTTP_200_OWA_Attachments.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_OWA_Attachments");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_OWA_Attachments", sw_HTTP_200_OWA_Attachments.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// These rulesets run fourth in priority to identify lurking errors in the session.
        /// </summary>
        /// <param name="session"></param>
        private void LurkingErrors(Session session)
        {
            this.session = session;

            var sw_HTTP_200_Lurking_Errors = Stopwatch.StartNew();

            HTTP_200_Lurking_Errors.Instance.Run(this.session);

            sw_HTTP_200_Lurking_Errors.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_Lurking_Errors");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_Lurking_Errors", sw_HTTP_200_Lurking_Errors.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// These rulesets run last in priority to mark the session as "actually ok" if it has not been identified as a known problem or lurking error.
        /// </summary>
        /// <param name="session"></param>
        private void ActuallyOK(Session session)
        {
            this.session = session;

            var sw_HTTP_200_Actually_OK = Stopwatch.StartNew();

            HTTP_200_Actually_OK.Instance.Run(this.session);

            sw_HTTP_200_Actually_OK.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_200_Actually_OK");
                TelemetryService.CustomTrackMetric("RS_HTTP_200_Actually_OK", sw_HTTP_200_Actually_OK.ElapsedMilliseconds);
            }
        }
    }
}
