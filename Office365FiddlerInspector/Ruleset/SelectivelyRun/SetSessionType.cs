using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class SetSessionType
    {
        internal Session session { get; set; }

        // Function to set Session Type column data.
        public void SetSessionTypeData(Session session)
        {
            // SetSessionType
            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " Running SetSessionTypeColumn.");

            // Return if SessionType already has a value.
            // Quite often ResponseCodeLogic has already stamped a more specific SessionType value.
            if (session["X-SessionType"] != null)
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " SessionType already set return.");
                return;
            }

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " Running SetSessionType");

            /////////////////////////////
            ///
            /// Set Session Type
            ///


            /// FREE/BUSY.
            if (session.fullUrl.Contains("WSSecurity"))
            {
                session["X-SessionType"] = "Free/Busy";
                SessionProcessor.Instance.SetSTCL(this.session, "10");
            }

            if (session["X-SACL"] == "10" || session["X-STCL"] == "10" || session["X-SRSCL"] == "10")
            {
                return;
            }

            if (session.fullUrl.Contains("GetUserAvailability"))
            {
                session["X-SessionType"] = "Free/Busy";
                SessionProcessor.Instance.SetSTCL(this.session, "10");
            }

            if (session["X-SACL"] == "10" || session["X-STCL"] == "10" || session["X-SRSCL"] == "10")
            {
                return;
            }

            if (session.utilFindInResponse("GetUserAvailability", false) > 1)
            {
                session["X-SessionType"] = "Free/Busy";
                SessionProcessor.Instance.SetSTCL(this.session, "10");
            }

            if (session["X-SACL"] == "10" || session["X-STCL"] == "10" || session["X-SRSCL"] == "10")
            {
                return;
            }

            // EWS.
            if (session.fullUrl.Contains("outlook.office365.com/EWS"))
            {
                session["X-SessionType"] = "Exchange Web Services";
                SessionProcessor.Instance.SetSTCL(this.session, "10");
            }

            if (session["X-SACL"] == "10" || session["X-STCL"] == "10" || session["X-SRSCL"] == "10")
            {
                return;
            }

            // Generic Office 365.
            if (session.fullUrl.Contains(".onmicrosoft.com") && (!(session.hostname.Contains("live.com"))))
            {
                session["X-SessionType"] = "Office 365 Authentication";
                SessionProcessor.Instance.SetSTCL(this.session, "10");
            }

            if (session["X-SACL"] == "10" || session["X-STCL"] == "10" || session["X-SRSCL"] == "10")
            {
                return;
            }

            if (session.fullUrl.Contains("outlook.office365.com"))
            {
                session["X-SessionType"] = "Office 365";
                SessionProcessor.Instance.SetSTCL(this.session, "10");
            }

            if (session["X-SACL"] == "10" || session["X-STCL"] == "10" || session["X-SRSCL"] == "10")
            {
                return;
            }

            if (session.fullUrl.Contains("outlook.office.com"))
            {
                session["X-SessionType"] = "Office 365";
                SessionProcessor.Instance.SetSTCL(this.session, "10");
            }

            if (session["X-SACL"] == "10" || session["X-STCL"] == "10" || session["X-SRSCL"] == "10")
            {
                return;
            }

            // Office 365 Authentication.
            if (session.url.Contains("login.microsoftonline.com") || session.HostnameIs("login.microsoftonline.com"))
            {
                session["X-SessionType"] = "Office 365 Authentication";
                SessionProcessor.Instance.SetSTCL(this.session, "10");
            }

            if (session["X-SACL"] == "10" || session["X-STCL"] == "10" || session["X-SRSCL"] == "10")
            {
                return;
            }

            // ADFS Authentication.
            if (session.fullUrl.Contains("adfs/services/trust/mex"))
            {
                session["X-SessionType"] = "ADFS Authentication";
                SessionProcessor.Instance.SetSTCL(this.session, "10");
            }

            if (session["X-SACL"] == "10" || session["X-STCL"] == "10" || session["X-SRSCL"] == "10")
            {
                return;
            }

            // Undetermined, but related to local process.
            if (session.LocalProcess.Contains("outlook"))
            {
                session["X-SessionType"] = "Outlook";
                SessionProcessor.Instance.SetSTCL(this.session, "10");
            }

            if (session["X-SACL"] == "10" || session["X-STCL"] == "10" || session["X-SRSCL"] == "10")
            {
                return;
            }

            // INTERNET EXPLORER
            if (session.LocalProcess.Contains("iexplore"))
            {
                session["X-SessionType"] = "Internet Explorer";
                SessionProcessor.Instance.SetSTCL(this.session, "10");
            }

            if (session["X-SACL"] == "10" || session["X-STCL"] == "10" || session["X-SRSCL"] == "10")
            {
                return;
            }

            // CHROME
            if (session.LocalProcess.Contains("chrome"))
            {
                session["X-SessionType"] = "Chrome";
                SessionProcessor.Instance.SetSTCL(this.session, "10");
            }

            if (session["X-SACL"] == "10" || session["X-STCL"] == "10" || session["X-SRSCL"] == "10")
            {
                return;
            }

            // FIREFOX
            if (session.LocalProcess.Contains("firefox"))
            {
                session["X-SessionType"] = "Firefox";
                SessionProcessor.Instance.SetSTCL(this.session, "10");
            }

            if (session["X-SACL"] == "10" || session["X-STCL"] == "10" || session["X-SRSCL"] == "10")
            {
                return;
            }

            // EDGE
            if (session.LocalProcess.Contains("edge"))
            {
                session["X-SessionType"] = "Edge";
                SessionProcessor.Instance.SetSTCL(this.session, "10");
            }

            if (session["X-SACL"] == "10" || session["X-STCL"] == "10" || session["X-SRSCL"] == "10")
            {
                return;
            }

            // SAFARI
            if (session.LocalProcess.Contains("safari"))
            {
                session["X-SessionType"] = "Safari";
                SessionProcessor.Instance.SetSTCL(this.session, "10");
            }

            if (session["X-SACL"] == "10" || session["X-STCL"] == "10" || session["X-SRSCL"] == "10")
            {
                return;
            }

            // Everything else.
            
            session["X-SessionType"] = "Not Classified";
            // Commented out setting colours on sessions not recognised.
            // Find in Fiddler will highlight sessions as yellow, so this would make reviewing find results difficult.
            //this.session["ui-backcolor"] = "yellow";
            //this.session["ui-color"] = "black";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " Session not classified in extension.");

            session["X-ResponseAlert"] = "Unclassified";
            session["X-ResponseComments"] = "The Office 365 Fiddler Extension does not yet have a way to classify this session."
                + "<p>If you have a suggestion for an improvement, create an issue or better yet a pull request in the project Github repository: "
                + "<a href='https://github.com/jprknight/Office365FiddlerExtension' target='_blank'>https://github.com/jprknight/Office365FiddlerExtension</a>.</p>";
            SessionProcessor.Instance.SetSTCL(this.session, "5");
            

            /////////////////////////////
            //
            // Session Type overrides
            //
            // If the local process is null or blank, then we are analysing traffic from a remote client such as a mobile device.
            // Fiddler was acting as remote proxy when the data was captured: https://docs.telerik.com/fiddler/Configure-Fiddler/Tasks/ConfigureForiOS

            if ((session.LocalProcess == null) || (session.LocalProcess == ""))
            {
                // Traffic has a null or blank local process value.
                SessionProcessor.Instance.SetProcess(this.session);
                SessionProcessor.Instance.SetSTCL(this.session, "10");
            }
            else
            {
                // If the traffic is not related to any of the below processes call it out.
                // So if for example lync.exe is the process write that to the Session Type column.
                if (!(session.LocalProcess.Contains("outlook") ||
                    session.LocalProcess.Contains("searchprotocolhost") ||
                    session.LocalProcess.Contains("iexplore") ||
                    session.LocalProcess.Contains("chrome") ||
                    session.LocalProcess.Contains("firefox") ||
                    session.LocalProcess.Contains("edge") ||
                    session.LocalProcess.Contains("safari") ||
                    session.LocalProcess.Contains("w3wp")))
                {
                    // Everything which is not detected as related to Exchange, Outlook or OWA in some way.
                    {
                        SessionProcessor.Instance.SetProcess(this.session);
                        session["X-SessionType"] = session["X-ProcessName"];
                        SessionProcessor.Instance.SetSTCL(this.session, "10");
                    }
                }
            }
        }
    }
}
