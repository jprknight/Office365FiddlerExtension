using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HostIP
    {
        internal Session session { get; set; }

        private static HostIP _instance;

        public static HostIP Instance => _instance ?? (_instance = new HostIP());

        public void SetHostIP(Session session)
        {
            this.session = session;

            string hostIP;

            if (this.session["X-HostIP"] == null)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Session X-HostIP is null.");

                hostIP = "NOT PRESENT";
            }
            else if (this.session["X-HostIP"].Contains("Not Present"))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Session X-HostIP is 'Not Present'.");

                hostIP = "NOT PRESENT";
            }
            else if (this.session["X-HostIP"] != "")
            {
                // Tuple -- tupleIsPrivateIPAddress (bool), matching subnet (string).
                Tuple<bool, string> tupleIsPrivateIPAddress = NetworkingService.Instance.IsPrivateIPAddress(this.session);

                if (tupleIsPrivateIPAddress.Item1)
                {
                    hostIP = "LAN:" + this.session["X-HostIP"];
                }
                else
                {
                    // Tuple -- IsMicrosoft365IP (bool), matching subnet (string).
                    Tuple<bool, string> tupleIsMicrosoft365IPAddress = NetworkingService.Instance.IsMicrosoft365IPAddress(this.session);

                    if (tupleIsMicrosoft365IPAddress.Item1)
                    {
                        hostIP = "M365:" + this.session["X-HostIP"];
                    }
                    else
                    {
                        hostIP = "PUB:" + this.session["X-HostIP"];
                    }
                }
            }
            else
            {
                hostIP = "UNKNOWN";
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = LangHelper.GetString("HostIP"),
                HostIP = hostIP
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
