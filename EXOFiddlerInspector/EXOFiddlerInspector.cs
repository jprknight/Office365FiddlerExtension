using System.Windows.Forms;
using Fiddler;

namespace EXOFiddlerInspector
{
    // Base class, generic inspector, common between request and response
    public class EXOBaseFiddlerInspector : Inspector2
    {
        private byte[] _body;
        private bool _readOnly;

        internal byte[] rawBody;

        internal Session session { get; set; }

        public bool bDirty
        {
            get { return false; }
        }

        public bool bReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;
            }
        }

        public override void AddToTab(TabPage o)
        {
            throw new System.NotImplementedException();
        }

        public override int GetOrder()
        {
            throw new System.NotImplementedException();
        }

        // Double click on a session to highlight inpsector.
        public override int ScoreForSession(Session oS)
        {
            this.session = oS;

            if (oS.url.Contains("autodiscover"))
            {
                return 100;
                
            }
            else if (oS.hostname.Contains("autodiscover"))
            {
                return 100;
            }
            else if (oS.url.Contains("outlook"))
            {
                return 100;
            }
            else if (oS.url.Contains("GetUserAvailability"))
            {
                return 100;
            }
            else if (oS.LocalProcess.Contains("outlook")){
                return 100;
            }
            else
            {
                return 0;
            }
        }
        public override void AssignSession(Session oS)
        {
            this.session = oS;

            base.AssignSession(oS);
        }

    }

    // Request class, inherits the generic class above, only defines things specific or different from the base class
    public class RequestInspector : EXOBaseFiddlerInspector, IRequestInspector2
    {

        //    oS.utilDecodeRequest();
        //    oS.utilDecodeResponse();

        private bool _readOnly;
        HTTPRequestHeaders _headers;
        private byte[] _body;
        RequestUserControl _displayControl;

        public override void AddToTab(TabPage o)
        {
            _displayControl = new RequestUserControl();
            o.Text = "Exchange Request";
            o.ToolTipText = "Exchange Online Inspector";
            o.Controls.Add(_displayControl);
            o.Controls[0].Dock = DockStyle.Fill;
        }

        public HTTPRequestHeaders headers
        {
            get
            {
                return _headers;

            }
            set
            {
                /*_headers = value;
                System.Collections.Generic.Dictionary<string, string> httpHeaders =
                    new System.Collections.Generic.Dictionary<string, string>();
                foreach (var item in headers)
                {
                    httpHeaders.Add(item.Name, item.Value);
                }*/
                //_displayControl.Headers = httpHeaders;

            }
        }

 /*       public void Sessions(Session oS)
        {
            if (oS.fullUrl.Contains("autodiscover-s.outlook.com")) {
                _displayControl.Text = "365 Autodiscover";
            }
        }
*/
        public void SetRequestValues(Session oS)
        {
            // Write HTTP Status Code Text box, convert int to string.
            _displayControl.SetRequestHostTextBox(oS.hostname);

            // Write Request URL Text box.
            _displayControl.SetRequestURLTextBox(oS.url);

            if (oS.fullUrl.Contains("outlook.office365.com/mapi")) { _displayControl.SetRequestTypeTextBox("EXO MAPI"); }
            else if (oS.fullUrl.Contains("outlook.office365.com/EWS")) { _displayControl.SetRequestTypeTextBox("EXO EWS"); }
            else if (oS.fullUrl.Contains("autodiscover-s.outlook.com")) { _displayControl.SetRequestTypeTextBox("EXO Autodiscover"); }
            else if (oS.fullUrl.Contains("onmicrosoft.com/autodiscover")) { _displayControl.SetRequestTypeTextBox("EXO Autodiscover"); }
            else if (oS.utilFindInRequest("autodiscover", false) > 1 && oS.utilFindInRequest("onmicrosoft.com", false) > 1) { _displayControl.SetRequestTypeTextBox("EXO Autodiscover"); }
            else if (oS.fullUrl.Contains("autodiscover") && (oS.fullUrl.Contains(".onmicrosoft.com"))) { _displayControl.SetRequestTypeTextBox("EXO Autodiscover"); }
            else if (oS.fullUrl.Contains("autodiscover")) { _displayControl.SetRequestTypeTextBox("Autodiscover"); }
            else if (oS.fullUrl.Contains("GetUserAvailability")) { _displayControl.SetRequestTypeTextBox("Free/Busy"); }
            else if (oS.fullUrl.Contains(".onmicrosoft.com")) { _displayControl.SetRequestTypeTextBox("Office 365"); }
            else if (oS.fullUrl.Contains("outlook.office365.com")) { _displayControl.SetRequestTypeTextBox("Office 365"); }
            else if (oS.fullUrl.Contains("outlook.office.com")) { _displayControl.SetRequestTypeTextBox("Office 365"); }
            else if (oS.LocalProcess.Contains("outlook")) { _displayControl.SetRequestTypeTextBox("Something Outlook"); }
            else if (oS.LocalProcess.Contains("iexplore")) { _displayControl.SetRequestTypeTextBox("Something Internet Explorer"); }
            else if (oS.LocalProcess.Contains("chrome")) { _displayControl.SetRequestTypeTextBox("Something Chrome"); }
            else if (oS.LocalProcess.Contains("firefox")) { _displayControl.SetRequestTypeTextBox("Something Firefox"); }
            else { _displayControl.SetRequestTypeTextBox("Not Exchange"); }

            // Set Request Process Textbox.
            _displayControl.SetRequestProcessTextBox(oS.LocalProcess);
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public override int GetOrder()
        {
            return 0;
        }

        public bool bDirty
        {
            get { return false; }
        }

        public bool bReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;
            }
        }

        public byte[] body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
                SetRequestValues(this.session);
                //_displayControl.Body = body;
            }
        }
    }

    // Response class, same as request class except for responses
    public class ResponseInspector : EXOBaseFiddlerInspector, IResponseInspector2
    {
        ResponseUserControl _displayControl;
        private HTTPResponseHeaders responseHeaders;
        //private int oResponseCode;

        public HTTPResponseHeaders headers
        {
            get { return responseHeaders; }
            set { responseHeaders = value;
            }
        }

        public void SetResponseComments (Session oS)
        {
            // Write HTTP Status Code Text box, convert int to string.
            _displayControl.SetHTTPResponseCodeTextBoxText(oS.responseCode.ToString());

            // Write Client Begin Request into textbox
            _displayControl.SetRequestBeginTimeTextBox(oS.Timers.ClientBeginRequest.ToString("H:mm:ss.ffff"));

            // Write Client End Request into textbox
            _displayControl.SetRequestEndTimeTextBox(oS.Timers.ClientDoneResponse.ToString("H:mm:ss.ffff"));

            // Write Elapsed Time into textbox.
            _displayControl.SetElapsedTimeTextBox(oS.oResponse.iTTLB + "ms");

            // Clear any previous data.
            _displayControl.SetResponseCommentsTextBoxText("");
            _displayControl.SetResponseAlertTextBox("");

            // Write Response Alert into Textbox.

            if (oS.responseCode == 403)
            {
                if (oS.utilFindInResponse("Access Denied", false) > 1)
                {
                    _displayControl.SetResponseAlertTextBox("Panic Stations!!!");
                    _displayControl.SetResponseCommentsTextBoxText("Is your firewall is blocking Outlook?.");
                }
            }
            else if (oS.responseCode == 502)
            {
                if (oS.utilFindInResponse("autodiscover", false) > 1)
                {
                    if (oS.utilFindInResponse("target machine actively refused it", false) > 1)
                    {
                        if (oS.utilFindInResponse(":443", false) > 1)
                        {
                            //oS["ui-backcolor"] = "green";
                            //oS["ui-color"] = "black";
                            _displayControl.SetResponseAlertTextBox("These aren't the droids your looking for.");
                            _displayControl.SetResponseCommentsTextBoxText("False Positive: By design Office 365 Autodiscover does not respond to say autodiscover.contoso.onmicrosoft.com on port 443. Validate this message by confirming this is an Office 365 IP address and a telnet to the IP address on port 80.");
                        }
                    }
                }
            }
        }

        public override void AddToTab(TabPage o)
        {
            _displayControl = new ResponseUserControl();
            o.Text = "Exchange Response";
            o.ToolTipText = "Exchange Online Inspector";
            o.Controls.Add(_displayControl);
            o.Controls[0].Dock = DockStyle.Fill;
        }



        /*public HTTPResponseHeaders headers
        {
            get
            {
                return _headers;
            }
            set
            {
                
                _headers = value;
                System.Collections.Generic.Dictionary<string, string> httpHeaders =
                    new System.Collections.Generic.Dictionary<string, string>();
                foreach (var item in headers)
                {
                    httpHeaders.Add(item.Name, item.Value);
                }
                //_displayControl.Headers = httpHeaders;
            }
        }*/

        //HTTPResponseHeaders IResponseInspector2.headers { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        //byte[] IBaseInspector2.body { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public byte[] body
        {
            get { return rawBody; }
            set
            {

                SetResponseComments(this.session);
                

                /*if (isAlchemyRequest(responseHeaders) && Convert.ToUInt32(responseHeaders["X-ResponseCode"]) == 0)
                {
                    AlchemyTab.Clear();
                    AlchemyTab.AppendLine("X-RequestType:  " + responseHeaders["X-RequestType"]);
                    AlchemyTab.AppendLine("X-ResponseCode: " + responseHeaders["X-ResponseCode"]);
                    AlchemyTab.AppendLine("\r\n" + ropHandler.handleResponse(value));
                }
                else
                {
                    AlchemyTab.SetText("X-RequestType: " + responseHeaders["X-RequestType"] + "\r\n\r\nRequest type not yet implemented.");
                }*/
            }
        }


        //bool IBaseInspector2.bReadOnly { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public override int GetOrder()
        {
            return 0;
        }

        void IBaseInspector2.Clear()
        {
            throw new System.NotImplementedException();
        }
    }

}
