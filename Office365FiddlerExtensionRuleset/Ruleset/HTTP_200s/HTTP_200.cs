using Fiddler;
using Office365FiddlerExtensionRuleset.Ruleset.HTTP_200s;
using Office365FiddlerExtension.Services;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_200
    {
        internal Session session { get; set; }

        private static HTTP_200 _instance;

        public static HTTP_200 Instance => _instance ?? (_instance = new HTTP_200());

        public void Run(Session session)
        {
            this.session = session;

            HTTP_200_ConnectTunnelSessions.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_ClientAccessRule.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_Culture_Not_Found.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_Outlook_MAPI_Protocol_Disabled.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_Outlook_MAPI_Exchange_Online.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_Outlook_MAPI_Exchange_OnPremise.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_OWA_Notification_Channel.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_OWA.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_Outlook_RPC.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_Outlook_NSPI.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_Address_Found.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_AddressNotFound.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_Unified_Groups_Settings.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_3S_Suggestions.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_REST_People_Request.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_Exchange_OnPremise_Any_Other_EWS.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_Exchange_Online_Any_Other_EWS.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_FreeBusy.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_Json.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_Javascript.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_Lurking_Errors.Instance.Run(this.session);
            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            ///////////////////////////////

            HTTP_200_Actually_OK.Instance.Run(this.session);
        }
    }
}
