using System.Windows.Forms;
using Fiddler;

[assembly: Fiddler.RequiredVersion("4.4.5.1")]

namespace EXOFiddlerInspector
{
    public class EXOFiddlerRequestInspector : Inspector2, IRequestInspector2
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
            o.Text = "ExchangeRequest";
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
                _headers = value;
                System.Collections.Generic.Dictionary<string, string> httpHeaders =
                    new System.Collections.Generic.Dictionary<string, string>();
                foreach (var item in headers)
                {
                    httpHeaders.Add(item.Name, item.Value);
                }
                //_displayControl.Headers = httpHeaders;
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

    public class EXOFiddlerInspector : EXOFiddlerRequestInspector, IResponseInspector2
    {

        private bool _readOnly;
        HTTPResponseHeaders _headers;
        private byte[] _body;
        ResponseUserControl _displayControl2;



        public override void AddToTab(TabPage o2)
        {
            _displayControl2 = new ResponseUserControl();
            o2.Text = "ExchangeResponse";
            o2.ToolTipText = "Exchange Online Inspector";
            o2.Controls.Add(_displayControl2);
            o2.Controls[0].Dock = DockStyle.Fill;
        }

        public HTTPResponseHeaders headers
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
        }

        public override int GetOrder()
        {
            return 0;
        }
    }

}
