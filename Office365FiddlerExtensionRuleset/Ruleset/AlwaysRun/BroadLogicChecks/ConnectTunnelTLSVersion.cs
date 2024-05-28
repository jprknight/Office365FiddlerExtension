using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class ConnectTunnelTLSVersion
    {
        internal Session session { get; set; }

        private static ConnectTunnelTLSVersion _instance;
        public static ConnectTunnelTLSVersion Instance => _instance ?? (_instance = new ConnectTunnelTLSVersion());

        /// <summary>
        /// Determine if the current session is a connect tunnel, if so set the TLS version.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            if (!this.session.isTunnel)
            {
                return;
            }

            string TLS;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Broad Logic Checks (connect tunnel).");

            // TLS 1.0 in request/response pair.

            // Request:
            //   Version: 3.1 (TLS/1.0)

            //Response:
            //   Secure Protocol: Tls
            //   Cipher: Aes256 256bits
            //   Hash Algorithm: Sha1 160bits

            if (RulesetUtilities.Instance.SearchForPhrase(this.session, "Secure Protocol: Tls10") || RulesetUtilities.Instance.SearchForPhrase(this.session, "(TLS/1.0)"))
            //if (this.session.utilFindInResponse("Secure Protocol: Tls10", false) > 1 || this.session.utilFindInResponse("(TLS/1.0)", false) > 1)
            {
                TLS = "1.0";
            }
            // TLS 1.1 in request/response pair.
            else if (RulesetUtilities.Instance.SearchForPhrase(this.session, "Secure Protocol: Tls11") || RulesetUtilities.Instance.SearchForPhrase(this.session, "(TLS/1.1)"))
            //else if (this.session.utilFindInResponse("Secure Protocol: Tls11", false) > 1 || this.session.utilFindInRequest("(TLS/1.1)", false) > 1)
            {
                TLS = "1.1";
            }
            // TLS 1.2 in request/response pair.
            else if (RulesetUtilities.Instance.SearchForPhrase(this.session, "Secure Protocol: Tls12") || RulesetUtilities.Instance.SearchForPhrase(this.session, "(TLS/1.2)"))
            //else if (this.session.utilFindInRequest("Secure Protocol: Tls12", false) > 1 || this.session.utilFindInRequest("(TLS/1.2)", false) > 1)
            {
                TLS = "1.2";
            }
            else if (RulesetUtilities.Instance.SearchForPhrase(this.session, "Secure Protocol: Tls13") || RulesetUtilities.Instance.SearchForPhrase(this.session, "(TLS/1.3)"))
            //else if (this.session.utilFindInRequest("Secure Protocol: Tls13", false) > 1 || this.session.utilFindInRequest("(TLS/1.3)", false) > 1)
            {
                TLS = "1.3";
            }
            else
            {
                // If we cannot determine the TLS version do nothing.
                // This can happen when live tracing traffic. The request/responses cannot be read fast enough to get accurate results.
                TLS = "TLS Unknown";
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = LangHelper.GetString("Connect Tunnel TLS Version"),

                TLSVersion = TLS
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
