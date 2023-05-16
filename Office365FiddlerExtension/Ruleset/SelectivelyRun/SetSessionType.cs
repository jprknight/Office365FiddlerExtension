using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;

namespace Office365FiddlerExtension.Ruleset
{
    class SetSessionType : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        // Set Session Type column data.
        public void SetSessionTypeData(Session session)
        {
            // Set Session Type
            
            this.session = session;

            var ExtensionSessionFlags = JsonConvert.DeserializeObject<SessionFlagProcessor.ExtensionSessionFlags>(SessionFlagProcessor.Instance.GetSessionJsonData(this.session));

            if (ExtensionSessionFlags.SessionTypeConfidenceLevel == 10)
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " SessionType already set return.");
                return;
            }

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Running SetSessionType");

            
            // FREE/BUSY.
            if (this.session.fullUrl.Contains("WSSecurity"))
            {
                // REVIEW THIS - Are these actually setting the session flags or just the temp storage (do nothing).
                ExtensionSessionFlags.SessionType = "Free/Busy";
                return;
            }

            if (this.session.fullUrl.Contains("GetUserAvailability"))
            {
                ExtensionSessionFlags.SessionType = "Free/Busy";
                return;
            }

            if (this.session.utilFindInResponse("GetUserAvailability", false) > 1)
            {
                ExtensionSessionFlags.SessionType = "Free/Busy";
                return;
            }

            // EWS.
            if (this.session.fullUrl.Contains("outlook.office365.com/EWS"))
            {
                ExtensionSessionFlags.SessionType = "M365 Exchange Web Services";
                return;
            }

            // Generic Office 365.
            if (this.session.fullUrl.Contains(".onmicrosoft.com") && (!(this.session.hostname.Contains("live.com"))))
            {
                ExtensionSessionFlags.SessionType = "Office 365 Authentication";
                return;
            }

            if (this.session.fullUrl.Contains("outlook.office365.com"))
            {
                ExtensionSessionFlags.SessionType = "Office 365";
                return;
            }

            if (this.session.fullUrl.Contains("outlook.office.com"))
            {
                ExtensionSessionFlags.SessionType = "Office 365";
                return;
            }

            // Office 365 Authentication.
            if (this.session.url.Contains("login.microsoftonline.com") || this.session.HostnameIs("login.microsoftonline.com"))
            {
                ExtensionSessionFlags.SessionType = "Office 365 Authentication";
                return;
            }

            // ADFS Authentication.
            if (this.session.fullUrl.Contains("adfs/services/trust/mex"))
            {
                ExtensionSessionFlags.SessionType = "ADFS Authentication";
                return;
            }

            // Undetermined, but related to local process.
            if (this.session.LocalProcess.Contains("outlook"))
            {
                ExtensionSessionFlags.SessionType = "Outlook";
                return;
            }

            // INTERNET EXPLORER
            if (this.session.LocalProcess.Contains("iexplore"))
            {
                ExtensionSessionFlags.SessionType = "Internet Explorer";
                return;
            }

            // CHROME
            if (this.session.LocalProcess.Contains("chrome"))
            {
                ExtensionSessionFlags.SessionType = "Chrome";
                return;
            }

            // FIREFOX
            if (this.session.LocalProcess.Contains("firefox"))
            {
                ExtensionSessionFlags.SessionType = "Firefox";
                return;
            }

            // EDGE
            if (this.session.LocalProcess.Contains("edge"))
            {
                ExtensionSessionFlags.SessionType = "Edge";
                return;
            }

            // SAFARI
            if (this.session.LocalProcess.Contains("safari"))
            {
                ExtensionSessionFlags.SessionType = "Safari";
                return;
            }

            // Everything else.

            ExtensionSessionFlags.SessionType = "Not Classified";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Session not classified in extension.");

            ExtensionSessionFlags.ResponseAlert = "Unclassified";
            ExtensionSessionFlags.ResponseComments = "The Office 365 Fiddler Extension does not yet have a way to classify this session."
                + "<p>If you have a suggestion for an improvement, create an issue or better yet a pull request in the project Github repository: "
                + "<a href='https://github.com/jprknight/Office365FiddlerExtension' target='_blank'>https://github.com/jprknight/Office365FiddlerExtension</a>.</p>";
            ExtensionSessionFlags.SessionTypeConfidenceLevel = 5;
            

            /////////////////////////////
            //
            // Session Type overrides
            //
            // If the local process is null or blank, then we are analysing traffic from a remote client such as a mobile device.
            // Fiddler was acting as remote proxy when the data was captured: https://docs.telerik.com/fiddler/Configure-Fiddler/Tasks/ConfigureForiOS

            if ((this.session.LocalProcess == null) || (this.session.LocalProcess == ""))
            {
                // Traffic has a null or blank local process value.
                SessionFlagProcessor.Instance.SetProcess(this.session);
                ExtensionSessionFlags.SessionTypeConfidenceLevel = 10;
            }
            else
            {
                // If the traffic is not related to any of the below processes call it out.
                // So if for example lync.exe is the process write that to the Session Type column.
                if (!(this.session.LocalProcess.Contains("outlook") ||
                    this.session.LocalProcess.Contains("searchprotocolhost") ||
                    this.session.LocalProcess.Contains("iexplore") ||
                    this.session.LocalProcess.Contains("chrome") ||
                    this.session.LocalProcess.Contains("firefox") ||
                    this.session.LocalProcess.Contains("edge") ||
                    this.session.LocalProcess.Contains("safari") ||
                    this.session.LocalProcess.Contains("w3wp")))
                {
                    // Everything which is not detected as related to Exchange, Outlook or OWA in some way.
                    {
                        SessionFlagProcessor.Instance.SetProcess(this.session);
                        ExtensionSessionFlags.SessionType =  this.session["X-ProcessName"];
                        ExtensionSessionFlags.SessionTypeConfidenceLevel = 10;
                    }
                }
            }
        }
    }
}