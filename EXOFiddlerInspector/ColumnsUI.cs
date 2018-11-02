using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;

namespace EXOFiddlerInspector
{
    class ColumnsUI
    {
        /// <summary>
        /// Call ColouriseWebSessions.
        /// </summary>
        /// CALL BACK TO COLOURISEWEBSESSIONS BROKE EVERYTHING. APP BUILDS. WILL NOT EXECUTE.
        /// SUSPECT CIRCULAR CALLS ARE REALLY BAD.
        /// WILL BE FIXED ONCE RULE SET MOVES OUT AS WELL.

        //ColouriseWebSessions calledColouriseWebSessions = new ColouriseWebSessions();

        private bool bResponseTimeColumnCreated = false;
        private bool bResponseServerColumnCreated = false;
        private bool bExchangeTypeColumnCreated = false;

        internal Session session { get; set; }

        /// <summary>
        /// Ensure the Response Time Column has been created, return if it has.
        /// </summary>
        public void EnsureResponseTimeColumn()
        {
            /////////////////
            // Response Time column.
            //
            // If the column is already created exit.
            if (bResponseTimeColumnCreated) return;

            FiddlerApplication.UI.lvSessions.AddBoundColumn("Response Time", 2, 110, "X-iTTLB");
            bResponseTimeColumnCreated = true;
            //
            /////////////////
        }

        /// <summary>
        ///  Ensure the Response Server column has been created, return if it has.
        /// </summary>
        public void EnsureResponseServerColumn()
        {
            if (bResponseServerColumnCreated) return;
            FiddlerApplication.UI.lvSessions.AddBoundColumn("Response Server", 2, 130, "X-ResponseServer");
            bResponseServerColumnCreated = true;
        }

        /// <summary>
        /// Ensure the Exchange Type Column has been created, return if it has.
        /// </summary>
        public void EnsureExchangeTypeColumn()
        {
            if (bExchangeTypeColumnCreated) return;
            FiddlerApplication.UI.lvSessions.AddBoundColumn("Exchange Type", 2, 150, "X-ExchangeType");
            bExchangeTypeColumnCreated = true;
        }

        /// <summary>
        /// Function where the Response Server column is populated.
        /// </summary>
        /// <param name="session"></param>
        public void SetResponseServer(Session session)
        {
            this.session = session;

            // Populate Response Server on session in order of preference from common to obsure.

            // If the response server header is not null or blank then populate it into the response server value.
            if ((this.session.oResponse["Server"] != null) && (this.session.oResponse["Server"] != ""))
            {
                this.session["X-ResponseServer"] = this.session.oResponse["Server"];
            }
            // Else if the reponnse Host header is not null or blank then populate it into the response server value
            // Some traffic identifies a host rather than a response server.
            else if ((this.session.oResponse["Host"] != null && (this.session.oResponse["Host"] != "")))
            {
                this.session["X-ResponseServer"] = "Host: " + this.session.oResponse["Host"];
            }
            // Else if the response PoweredBy header is not null or blank then populate it into the response server value.
            // Some Office 365 servers respond as X-Powered-By ASP.NET.
            else if ((this.session.oResponse["X-Powered-By"] != null) && (this.session.oResponse["X-Powered-By"] != ""))
            {
                this.session["X-ResponseServer"] = "X-Powered-By: " + this.session.oResponse["X-Powered-By"];
            }
            // Else if the response X-Served-By header is not null or blank then populate it into the response server value.
            else if ((this.session.oResponse["X-Served-By"] != null && (this.session.oResponse["X-Served-By"] != "")))
            {
                this.session["X-ResponseServer"] = "X-Served-By: " + this.session.oResponse["X-Served-By"];
            }
            // Else if the response X-Served-By header is not null or blank then populate it into the response server value.
            else if ((this.session.oResponse["X-Server-Name"] != null && (this.session.oResponse["X-Server-Name"] != "")))
            {
                this.session["X-ResponseServer"] = "X-Served-Name: " + this.session.oResponse["X-Server-Name"];
            }
            else if (this.session.isTunnel == true)
            {
                this.session["X-ResponseServer"] = "Connect Tunnel";
            }
        }

        /// <summary>
        /// Function where the Exchange Type column is populated.
        /// </summary>
        /// <param name="session"></param>
        public void SetExchangeType(Session session)
        {
            this.session = session;

            // Outlook Connections.
            if (this.session.fullUrl.Contains("outlook.office365.com/mapi")) { this.session["X-ExchangeType"] = "EXO MAPI"; }
            // Exchange Online Autodiscover.
            else if (this.session.utilFindInRequest("autodiscover", false) > 1 && this.session.utilFindInRequest("onmicrosoft.com", false) > 1) { this.session["X-ExchangeType"] = "EXO Autodiscover"; }
            else if (this.session.fullUrl.Contains("autodiscover") && (this.session.fullUrl.Contains(".onmicrosoft.com"))) { this.session["X-ExchangeType"] = "EXO Autodiscover"; }
            else if (this.session.fullUrl.Contains("autodiscover-s.outlook.com")) { this.session["X-ExchangeType"] = "EXO Autodiscover"; }
            else if (this.session.fullUrl.Contains("onmicrosoft.com/autodiscover")) { this.session["X-ExchangeType"] = "EXO Autodiscover"; }
            // Autodiscover.     
            else if ((this.session.fullUrl.Contains("autodiscover") && (!(this.session.hostname == "outlook.office365.com")))) { this.session["X-ExchangeType"] = "On-Prem Autodiscover"; }
            else if (this.session.hostname.Contains("autodiscover")) { this.session["X-ExchangeType"] = "On-Prem Autodiscover"; }
            // Free/Busy.
            else if (this.session.fullUrl.Contains("WSSecurity"))
            {
                this.session["X-ExchangeType"] = "Free/Busy";
                // Increment HTTP200FreeBusy counter to assist with session classification further on down the line.
                //calledColouriseWebSessions.IncrementHTTP200FreeBusyCount();
            }
            else if (this.session.fullUrl.Contains("GetUserAvailability"))
            {
                this.session["X-ExchangeType"] = "Free/Busy";
                // Increment HTTP200FreeBusy counter to assist with session classification further on down the line.
                //calledColouriseWebSessions.IncrementHTTP200FreeBusyCount();
            }
            else if (this.session.utilFindInResponse("GetUserAvailability", false) > 1)
            {
                this.session["X-ExchangeType"] = "Free/Busy";
                // Increment HTTP200FreeBusy counter to assist with session classification further on down the line.
                //calledColouriseWebSessions.IncrementHTTP200FreeBusyCount();
            }
            // EWS.
            else if (this.session.fullUrl.Contains("outlook.office365.com/EWS")) { this.session["X-ExchangeType"] = "EXO EWS"; }
            // Generic Office 365.
            else if (this.session.fullUrl.Contains(".onmicrosoft.com") && (!(this.session.hostname.Contains("live.com")))) { this.session["X -ExchangeType"] = "Exchange Online"; }
            else if (this.session.fullUrl.Contains("outlook.office365.com")) { this.session["X-ExchangeType"] = "Office 365"; }
            else if (this.session.fullUrl.Contains("outlook.office.com")) { this.session["X-ExchangeType"] = "Office 365"; }
            // Office 365 Authentication.
            else if (this.session.url.Contains("login.microsoftonline.com") || this.session.HostnameIs("login.microsoftonline.com")) { this.session["X-ExchangeType"] = "Office 365 Authentication"; }
            // ADFS Authentication.
            else if (this.session.fullUrl.Contains("adfs/services/trust/mex")) { this.session["X-ExchangeType"] = "ADFS Authentication"; }
            // Undetermined, but related to local process.
            else if (this.session.LocalProcess.Contains("outlook")) { this.session["X-ExchangeType"] = "Outlook"; }
            else if (this.session.LocalProcess.Contains("iexplore")) { this.session["X-ExchangeType"] = "Internet Explorer"; }
            else if (this.session.LocalProcess.Contains("chrome")) { this.session["X-ExchangeType"] = "Chrome"; }
            else if (this.session.LocalProcess.Contains("firefox")) { this.session["X-ExchangeType"] = "Firefox"; }
            // Everything else.
            else { this.session["X-ExchangeType"] = "Not Exchange"; }

            /////////////////////////////
            //
            // Exchange Type overrides
            //
            // First off if the local process is null or blank, then we are analysing traffic from a remote client such as a mobile device.
            // Fiddler was acting as remote proxy when the data was captured: https://docs.telerik.com/fiddler/Configure-Fiddler/Tasks/ConfigureForiOS
            // So don't pay any attention to overrides for this type of traffic.
            if ((this.session.LocalProcess == null) || (this.session.LocalProcess == ""))
            {
                // Traffic has a null or blank local process value.
                this.session["X-ExchangeType"] = "Remote Capture";
            }
            else
            {
                // With that out of the way,  if the traffic is not related to any of the below processes call it out.
                // So if for example lync.exe is the process write that to the Exchange Type column.
                if (!(this.session.LocalProcess.Contains("outlook") ||
                    this.session.LocalProcess.Contains("searchprotocolhost") ||
                    this.session.LocalProcess.Contains("iexplore") ||
                    this.session.LocalProcess.Contains("chrome") ||
                    this.session.LocalProcess.Contains("firefox") ||
                    this.session.LocalProcess.Contains("edge") ||
                    this.session.LocalProcess.Contains("w3wp")))
                {
                    // Everything which is not detected as related to Exchange, Outlook or OWA in some way.
                    { this.session["X-ExchangeType"] = this.session.LocalProcess; }
                }
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

        public void AutoTamperResponseBefore(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void AutoTamperResponseAfter(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void OnBeforeReturningError(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void OnLoad()
        {
            throw new NotImplementedException();
        }

        public void OnBeforeUnload()
        {
            //throw new NotImplementedException();
        }

        // Populate the ResponseTime column on live trace, if the column is enabled.
        // Code currently not used / under review.

        // if (boolResponseTimeColumnEnabled && boolExtensionEnabled) {
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