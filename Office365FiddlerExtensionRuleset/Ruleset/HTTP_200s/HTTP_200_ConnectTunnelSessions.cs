﻿using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset.HTTP_200s
{
    class HTTP_200_ConnectTunnelSessions
    {
        internal Session session { get; set; }

        private static HTTP_200_ConnectTunnelSessions _instance;

        public static HTTP_200_ConnectTunnelSessions Instance => _instance ?? (_instance = new HTTP_200_ConnectTunnelSessions());

        public void Run(Session session)
        {
            this.session = session;

            // Connect Tunnel.
            //
            // Check for connect tunnel with no usable data in the response body.
            //
            // This check does not work for sessions which have not been loaded from a SAZ file.
            // My best guess is this is a timing issue, where the data is not immediately available when this check runs.
            // SetSessionType makes exactly the same call later on down the code path and it works.
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

            if (SessionContentSearch.Instance.SearchForPhrase(this.session, "Secure Protocol: Tls10") || SessionContentSearch.Instance.SearchForPhrase(this.session, "(TLS/1.0)"))
            //if (this.session.utilFindInResponse("Secure Protocol: Tls10", false) > 1 || this.session.utilFindInResponse("(TLS/1.0)", false) > 1)
            {
                TLS = "TLS 1.0";
            }
            // TLS 1.1 in request/response pair.
            else if (SessionContentSearch.Instance.SearchForPhrase(this.session, "Secure Protocol: Tls11") || SessionContentSearch.Instance.SearchForPhrase(this.session, "(TLS/1.1)"))
            //else if (this.session.utilFindInResponse("Secure Protocol: Tls11", false) > 1 || this.session.utilFindInRequest("(TLS/1.1)", false) > 1)
            {
                TLS = "TLS 1.1";
            }
            // TLS 1.2 in request/response pair.
            else if (SessionContentSearch.Instance.SearchForPhrase(this.session,"Secure Protocol: Tls12") || SessionContentSearch.Instance.SearchForPhrase(this.session, "(TLS/1.2)"))
            //else if (this.session.utilFindInRequest("Secure Protocol: Tls12", false) > 1 || this.session.utilFindInRequest("(TLS/1.2)", false) > 1)
            {
                TLS = "TLS 1.2";
            }
            else if (SessionContentSearch.Instance.SearchForPhrase(this.session, "Secure Protocol: Tls13") || SessionContentSearch.Instance.SearchForPhrase(this.session, "(TLS/1.3)"))
            //else if (this.session.utilFindInRequest("Secure Protocol: Tls13", false) > 1 || this.session.utilFindInRequest("(TLS/1.3)", false) > 1)
            {
                TLS = "TLS 1.3";
            }
            else
            {
                // If we cannot determine the TLS version do nothing.
                // This can happen when live tracing traffic. The request/responses cannot be read fast enough to get accurate results.
                TLS = "TLS Unknown";
            }

            // 11/1/2022 -- There was some old code accompanying this comment, leaving this as it might be useful information for the future.

            // Trying to check session response body for a string value using !this.Session.bHasResponse does not impact performance, but is not reliable.
            // Using this.Session.GetResponseBodyAsString().Length == 0 kills performance. Fiddler wouldn't even load with this code in place.
            // Ideally looking to do: if (this.Session.utilFindInResponse("CONNECT tunnel, through which encrypted HTTPS traffic flows", false) > 1)
            // Only works reliably when loading a SAZ file and request/response data is immediately available to do logic checks against.

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_ConnectTunnelSessions");

                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                sessionAuthenticationConfidenceLevel = 10;
                sessionTypeConfidenceLevel = 10;
                sessionResponseServerConfidenceLevel = 10;
                sessionSeverity = 40;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s",

                SessionType = $"{LangHelper.GetString("HTTP_200_ConnectTunnel")}: {TLS}",
                ResponseCodeDescription = LangHelper.GetString("HTTP_200_ConnectTunnel"),
                ResponseServer = LangHelper.GetString("HTTP_200_ConnectTunnel"),
                ResponseAlert = LangHelper.GetString("HTTP_200_ConnectTunnel"),
                ResponseComments = LangHelper.GetString("HTTP_200_ConnectTunnel_RepsonseComments"),
                Authentication = $"{LangHelper.GetString("HTTP_200_ConnectTunnel")}: {TLS}",

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
