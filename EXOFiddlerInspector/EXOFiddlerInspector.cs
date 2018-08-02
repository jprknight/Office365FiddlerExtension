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
        /*public override int ScoreForSession(Session oS)
        {
            if (oS.fullUrl.Contains("autodiscover"))
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
            else if (oS.fullUrl.Contains("GetUserAvailability"))
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
        }*/
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

        public void Sessions(Session oS)
        {
            if (oS.fullUrl.Contains("autodiscover-s.outlook.com")) {
                _displayControl.Text = "365 Autodiscover";
            }
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
                //_displayControl.Body = body;
            }
        }
    }

    // Response class, same as request class except for responses
    public class ResponseInspector : EXOBaseFiddlerInspector, IResponseInspector2
    {
        ResponseUserControl _displayControl;
        private HTTPResponseHeaders responseHeaders;

        public HTTPResponseHeaders headers
        {
            get { return responseHeaders; }
            set { responseHeaders = value; }
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
