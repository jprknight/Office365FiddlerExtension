using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;

namespace Office365FiddlerExtension.Ruleset
{
    class SetSessionType : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        // Set Session Type column data.
        public void SetSessionTypeData(Session session)
        {
            this.session = session;
            // SetSessionType
            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Running SetSessionTypeColumn.");

            // Return if SessionType already has a value.
            // Quite often ResponseCodeLogic has already stamped a more specific SessionType value.
            // REVIEW THIS, should this be Session Type confidence level?
            if (getSetSessionFlags.GetSessionType(this.session) != null)
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " SessionType already set return.");
                return;
            }

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Running SetSessionType");

            /////////////////////////////
            ///
            /// Set Session Type
            ///
            
            /// FREE/BUSY.
            if (this.session.fullUrl.Contains("WSSecurity"))
            {
                getSetSessionFlags.SetSessionType(this.session, "Free/Busy");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
            }

            if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
            {
                return;
            }

            if (this.session.fullUrl.Contains("GetUserAvailability"))
            {
                getSetSessionFlags.SetSessionType(this.session, "Free/Busy");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
            }

            if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
            {
                return;
            }

            if (this.session.utilFindInResponse("GetUserAvailability", false) > 1)
            {
                getSetSessionFlags.SetSessionType(this.session, "Free/Busy");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
            }

            if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
            {
                return;
            }

            // EWS.
            if (this.session.fullUrl.Contains("outlook.office365.com/EWS"))
            {
                getSetSessionFlags.SetSessionType(this.session, "Exchange Web Services");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
            }

            if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
            {
                return;
            }

            // Generic Office 365.
            if (this.session.fullUrl.Contains(".onmicrosoft.com") && (!(this.session.hostname.Contains("live.com"))))
            {
                getSetSessionFlags.SetSessionType(this.session, "Office 365 Authentication");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
            }

            if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
            {
                return;
            }

            if (this.session.fullUrl.Contains("outlook.office365.com"))
            {
                getSetSessionFlags.SetSessionType(this.session, "Office 365");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
            }

            if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
            {
                return;
            }

            if (this.session.fullUrl.Contains("outlook.office.com"))
            {
                getSetSessionFlags.SetSessionType(this.session, "Office 365");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
            }

            if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
            {
                return;
            }

            // Office 365 Authentication.
            if (this.session.url.Contains("login.microsoftonline.com") || this.session.HostnameIs("login.microsoftonline.com"))
            {
                getSetSessionFlags.SetSessionType(this.session, "Office 365 Authentication");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
            }

            if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
            {
                return;
            }

            // ADFS Authentication.
            if (this.session.fullUrl.Contains("adfs/services/trust/mex"))
            {
                getSetSessionFlags.SetSessionType(this.session, "ADFS Authentication");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
            }

            if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
            {
                return;
            }

            // Undetermined, but related to local process.
            if (this.session.LocalProcess.Contains("outlook"))
            {
                getSetSessionFlags.SetSessionType(this.session, "Outlook");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
            }

            if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
            {
                return;
            }

            // INTERNET EXPLORER
            if (this.session.LocalProcess.Contains("iexplore"))
            {
                getSetSessionFlags.SetSessionType(this.session, "Internet Explorer");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
            }

            if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
            {
                return;
            }

            // CHROME
            if (this.session.LocalProcess.Contains("chrome"))
            {
                getSetSessionFlags.SetSessionType(this.session, "Chrome");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
            }

            if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
            {
                return;
            }

            // FIREFOX
            if (this.session.LocalProcess.Contains("firefox"))
            {
                getSetSessionFlags.SetSessionType(this.session, "Firefox");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
            }

            if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
            {
                return;
            }

            // EDGE
            if (this.session.LocalProcess.Contains("edge"))
            {
                getSetSessionFlags.SetSessionType(this.session, "Edge");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
            }

            if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
            {
                return;
            }

            // SAFARI
            if (this.session.LocalProcess.Contains("safari"))
            {
                getSetSessionFlags.SetSessionType(this.session, "Safari");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
            }

            if (getSetSessionFlags.GetAnySessionConfidenceLevelTen(this.session))
            {
                return;
            }

            // Everything else.

            getSetSessionFlags.SetSessionType(this.session, "Not Classified");

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Session not classified in extension.");

            getSetSessionFlags.SetXResponseAlert(this.session, "Unclassified");
            getSetSessionFlags.SetXResponseComments(this.session, "The Office 365 Fiddler Extension does not yet have a way to classify this session."
                + "<p>If you have a suggestion for an improvement, create an issue or better yet a pull request in the project Github repository: "
                + "<a href='https://github.com/jprknight/Office365FiddlerExtension' target='_blank'>https://github.com/jprknight/Office365FiddlerExtension</a>.</p>");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "5");
            

            /////////////////////////////
            //
            // Session Type overrides
            //
            // If the local process is null or blank, then we are analysing traffic from a remote client such as a mobile device.
            // Fiddler was acting as remote proxy when the data was captured: https://docs.telerik.com/fiddler/Configure-Fiddler/Tasks/ConfigureForiOS

            if ((this.session.LocalProcess == null) || (this.session.LocalProcess == ""))
            {
                // Traffic has a null or blank local process value.
                getSetSessionFlags.SetProcess(this.session);
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
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
                        getSetSessionFlags.SetProcess(this.session);
                        getSetSessionFlags.SetSessionType(this.session, session["X-ProcessName"]);
                        getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
                    }
                }
            }
        }
    }
}