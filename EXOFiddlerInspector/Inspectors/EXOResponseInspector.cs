namespace EXOFiddlerInspector.Inspectors
{
    using Fiddler;

    public class EXOResponseInspector : EXOInspector, IResponseInspector2
    {
        /// <summary>
        ///  Gets or sets the response headers from the frame
        /// </summary>
        public HTTPResponseHeaders headers
        {
            get
            {
                return this.BaseHeaders as HTTPResponseHeaders;
            }

            set
            {
                this.BaseHeaders = value;
            }
        }
    }
}
