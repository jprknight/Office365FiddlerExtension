using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fiddler;

namespace EXOFiddlerInspector
{
    class ColumnsUI : IAutoTamper
    {
        public bool bElapsedTimeColumnCreated = false;
        public bool bResponseServerColumnCreated = false;
        public bool bExchangeTypeColumnCreated = false;
        public bool bXHostIPColumnCreated = false;
        public bool bAuthColumnCreated = false;

        public Boolean bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);
        public Boolean bElapsedTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ElapsedTimeColumnEnabled", false);
        public Boolean bResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
        public Boolean bExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", false);
        public Boolean bXHostIPColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.XHostIPColumnEnabled", false);
        public Boolean bAuthColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AuthColumnEnabled", false);
        public Boolean bAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", false);
        public Boolean bHighlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled", false);
        public int iExecutionCount = FiddlerApplication.Prefs.GetInt32Pref("extensions.EXOFiddlerInspector.ExecutionCount", 0);

        public int wordCount = 0;

        internal Session session { get; set; }

        /// <summary>
        /// Ensure the Response Time Column has been created, return if it has.
        /// </summary>
        public void EnsureElapsedTimeColumn()
        {
            Boolean LoadSaz = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.LoadSaz", false);

            if (bElapsedTimeColumnCreated && bExtensionEnabled)
            {
                return;
            }
            else if (LoadSaz && bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Elapsed Time", 2, 110, "X-ElapsedTime");
                bElapsedTimeColumnCreated = true;
            }
            else if (bExtensionEnabled)
            {
                // live trace, don't load this column.
                // Testing.
                //FiddlerApplication.UI.lvSessions.AddBoundColumn("Elapsed Time", 2, 110, "X-ElapsedTime");
                //bElapsedTimeColumnCreated = true;
            }
        }

        /// <summary>
        ///  Ensure the Response Server column has been created, return if it has.
        /// </summary>
        public void EnsureResponseServerColumn()
        {
            if (bResponseServerColumnCreated && bExtensionEnabled)
            {
                return;
            }
            else if (bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Response Server", 2, 130, "X-ResponseServer");
                bResponseServerColumnCreated = true;
            }
            
        }

        /// <summary>
        ///  Ensure the X-HostIP column has been created, return if it has.
        /// </summary>
        public void EnsureXHostIPColumn()
        {
            if (bXHostIPColumnCreated && bExtensionEnabled)
            {
                return;
            }
            else if (bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.AddBoundColumn("X-HostIP", 2, 110, "X-HostIP");
                bXHostIPColumnCreated = true;
            }
        }

        /// <summary>
        /// Ensure the Exchange Type Column has been created, return if it has.
        /// </summary>
        public void EnsureExchangeTypeColumn()
        {
            if (bExchangeTypeColumnCreated)
            {
                return;
            }
            else if (bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Exchange Type", 2, 150, "X-ExchangeType");
                bExchangeTypeColumnCreated = true;
            }
        }

        public void EnsureAuthColumn()
        {
            if (bAuthColumnCreated)
            {
                return;
            }
            else if (bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.AddBoundColumn("Authentication", 2, 140, "X-Authentication");
                bAuthColumnCreated = true;
            }
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

        /// <summary>
        /// Used specifically for Authentication sessions.
        /// Inclusion of '"' may not be compatible with say HTTP 503 response body word split.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="searchTerm"></param>
        /// <returns>wordCount</returns>
        public int SearchSessionForWord(Session session, string searchTerm)
        {
            this.session = session;

            // Count the occurrences of common search terms match up to certain HTTP response codes to highlight certain scenarios.
            //
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-count-occurrences-of-a-word-in-a-string-linq
            //

            string text = this.session.ToString();

            //Convert the string into an array of words  
            string[] source = text.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',', '"' }, StringSplitOptions.RemoveEmptyEntries);

            // Create the query. Use ToLowerInvariant to match "data" and "Data"   
            var matchQuery = from word in source
                             where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                             select word;

            // Count the matches, which executes the query.  
            int wordCount = matchQuery.Count();
            
            //MessageBox.Show(this.session.id + " " + searchTerm + " " + wordCount);

            return wordCount;
        }

        public void SAMLParserFieldsNoData()
        {
            this.session["X-Issuer"] = "No SAML Data in session";
            this.session["X-AttributeNameUPNTextBox"] = "No SAML Data in session";
            this.session["X-NameIdentifierFormatTextBox"] = "No SAML Data in session";
            this.session["X-AttributeNameImmutableIDTextBox"] = "No SAML Data in session";
        }

        /// <summary>
        /// Set Authentication column values.
        /// </summary>
        /// <param name="session"></param>
        public void SetAuthentication(Session session)
        {
            Boolean OverrideFurtherAuthChecking = false;

            this.session = session;

            // Determine if this session contains a SAML response.
            if (this.session.utilFindInResponse("Issuer=", false) > 1 && 
                this.session.utilFindInResponse("Attribute AttributeName=", false) > 1 &&
                this.session.utilFindInResponse("NameIdentifier Format=", false) > 1 &&
                this.session.utilFindInResponse("Attribute AttributeName=", false) > 1)
            {
                this.session["X-Authentication"] = "SAML Request/Response";
                this.session["X-AuthenticationDesc"] = "See below for SAML response parser.";

                // Pull issuer data from response.
                string SessionBody = this.session.ToString();
                int IssuerStartIndex = SessionBody.IndexOf("Issuer=");
                int IssuerEndIndex = SessionBody.IndexOf("IssueInstant=");
                int IssuerLength = IssuerEndIndex - IssuerStartIndex;
                string Issuer = SessionBody.Substring(IssuerStartIndex, IssuerLength);
                Issuer = Issuer.Replace("&quot;", "\"");

                // AttributeNameUPN.
                int AttributeNameUPNStartIndex = SessionBody.IndexOf("&lt;saml:Attribute AttributeName=&quot;UPN");
                int AttributeNameUPNEndIndex = SessionBody.IndexOf("&lt;/saml:Attribute>");
                int AttributeNameUPNLength = AttributeNameUPNEndIndex - AttributeNameUPNStartIndex;
                string AttributeNameUPN = SessionBody.Substring(AttributeNameUPNStartIndex, AttributeNameUPNLength);
                AttributeNameUPN = AttributeNameUPN.Replace("&quot;", "\"");
                AttributeNameUPN = AttributeNameUPN.Replace("&lt;", "<");
                // Now split the two lines with a new line for easier reading in the user control.
                int SplitAttributeNameUPNStartIndex = AttributeNameUPN.IndexOf("><") + 1;
                string AttributeNameUPNFirstLine = AttributeNameUPN.Substring(0, SplitAttributeNameUPNStartIndex);
                string AttributeNameUPNSecondLine = AttributeNameUPN.Substring(SplitAttributeNameUPNStartIndex);
                AttributeNameUPN = AttributeNameUPNFirstLine + Environment.NewLine + AttributeNameUPNSecondLine;

                // NameIdentifierFormat.

                int NameIdentifierFormatStartIndex = SessionBody.IndexOf("&lt;saml:NameIdentifier Format");
                int NameIdentifierFormatEndIndex = SessionBody.IndexOf("&lt;saml:SubjectConfirmation>");
                int NameIdentifierFormatLength = NameIdentifierFormatEndIndex - NameIdentifierFormatStartIndex;
                string NameIdentifierFormat = SessionBody.Substring(NameIdentifierFormatStartIndex, NameIdentifierFormatLength);
                NameIdentifierFormat = NameIdentifierFormat.Replace("&quot;", "\"");
                NameIdentifierFormat = NameIdentifierFormat.Replace("&lt;", "<");

                // AttributeNameImmutableID.
                int AttributeNameImmutableIDStartIndex = SessionBody.IndexOf("AttributeName=&quot;ImmutableID");
                int AttributeNameImmutibleIDEndIndex = SessionBody.IndexOf("&lt;/saml:AttributeStatement>");
                int AttributeNameImmutibleIDLength = AttributeNameImmutibleIDEndIndex - AttributeNameImmutableIDStartIndex;
                string AttributeNameImmutibleID = SessionBody.Substring(AttributeNameImmutableIDStartIndex, AttributeNameImmutibleIDLength);
                AttributeNameImmutibleID = AttributeNameImmutibleID.Replace("&quot;", "\"");
                AttributeNameImmutibleID = AttributeNameImmutibleID.Replace("&lt;", "<");
                // Now split out response with a newline for easier reading.
                int SplitAttributeNameImmutibleIDStartIndex = AttributeNameImmutibleID.IndexOf("<saml:AttributeValue>") + 21; // Add 21 characters to shift where the newline is placed.
                string AttributeNameImmutibleIDFirstLine = AttributeNameImmutibleID.Substring(0, SplitAttributeNameImmutibleIDStartIndex);
                string AttributeNameImmutibleIDSecondLine = AttributeNameImmutibleID.Substring(SplitAttributeNameImmutibleIDStartIndex);
                AttributeNameImmutibleID = AttributeNameImmutibleIDFirstLine + Environment.NewLine + AttributeNameImmutibleIDSecondLine;
                // Second split
                SplitAttributeNameImmutibleIDStartIndex = AttributeNameImmutibleID.IndexOf("</saml:AttributeValue></saml:Attribute>");
                AttributeNameImmutibleIDFirstLine = AttributeNameImmutibleID.Substring(0, SplitAttributeNameImmutibleIDStartIndex);
                AttributeNameImmutibleIDSecondLine = AttributeNameImmutibleID.Substring(SplitAttributeNameImmutibleIDStartIndex);
                AttributeNameImmutibleID = AttributeNameImmutibleIDFirstLine + Environment.NewLine + AttributeNameImmutibleIDSecondLine;

                this.session["X-Issuer"] = Issuer;
                this.session["X-AttributeNameUPNTextBox"] = AttributeNameUPN;
                this.session["X-NameIdentifierFormatTextBox"] = NameIdentifierFormat;
                this.session["X-AttributeNameImmutableIDTextBox"] = AttributeNameImmutibleID;
            }
            // Determine if Modern Authentication is enabled in Exchange Online.
            else if (this.session.oRequest["Authorization"] == "Bearer" || this.session.oRequest["Authorization"] == "Basic")
            {
                SAMLParserFieldsNoData();

                // Looking for the following in a response body:

                // x-ms-diagnostics: 4000000;reason="Flighting is not enabled for domain 'user@contoso.com'.";error_category="oauth_not_available"

                int KeywordFourMillion = SearchSessionForWord(this.session, "4000000");
                int KeywordFlighting = SearchSessionForWord(this.session, "Flighting");
                int Keywordenabled = SearchSessionForWord(this.session, "enabled");
                int Keyworddomain = SearchSessionForWord(this.session, "domain");
                int Keywordoauth_not_available = SearchSessionForWord(this.session, "oauth_not_available");

                // Check if all the above checks have a value of at least 1. 
                // If they do, then Exchange Online is configured with Modern Authentication disabled.
                if (KeywordFourMillion > 0 && KeywordFlighting > 0 && Keywordenabled > 0 &&
                    Keyworddomain > 0 && Keywordoauth_not_available > 0 && this.session.HostnameIs("autodiscover-s.outlook.com"))
                {
                    this.session["X-Authentication"] = "EXO Modern Auth Disabled";

                    this.session["X-AuthenticationDesc"] = "EXO Modern Auth Disabled" +
                        Environment.NewLine +
                        Environment.NewLine +
                        "Exchange Online has Modern Authentication disabled. " +
                        "This is not necessarily a bad thing, but something to make note of during troubleshooting." +
                        Environment.NewLine +
                        "MutiFactor Authentication will not work as expected while Modern Authentication " +
                        "is disabled in Exchange Online" +
                        Environment.NewLine +
                        Environment.NewLine +
                        "Outlook 2010 and older do not support Modern Authentication and by extension MutliFactor Authentication." +
                        Environment.NewLine +
                        "Outlook 2013 supports modern authentication with updates and the EnableADAL registry key set to 1." +
                        Environment.NewLine +
                        "See https://support.microsoft.com/en-us/help/4041439/modern-authentication-configuration-requirements-for-transition-from-o" +
                        Environment.NewLine +
                        "Outlook 2016 or newer. No updates or registry keys needed for Modern Authentication.";

                    // Set the OverrideFurtherAuthChecking to true; EXO Modern Auth Disabled is a more important message in these sessions,
                    // than Outlook client auth capabilities. Other sessions are expected to show client auth capabilities.
                    OverrideFurtherAuthChecking = true;

                    if (bAppLoggingEnabled)
                    {
                        FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " EXO Modern Auth Disabled.");
                    }
                }
                else
                {
                    // Do nothing right now.
                }

                // Now get specific to find out what the client can do.
                // If the session request header Authorization equals Bearer this is a Modern Auth capable client.
                if (this.session.oRequest["Authorization"] == "Bearer" && !(OverrideFurtherAuthChecking))
                {
                    this.session["X-Authentication"] = "Outlook Modern Auth";

                    this.session["X-AuthenticationDesc"] = "Outlook Modern Auth" +
                        Environment.NewLine +
                        Environment.NewLine +
                        "Outlook is stating it can do Modern Authentication. " +
                        "Whether it is used or not will depend on whether Modern Authentication is enabled in Exchange Online.";

                    if (bAppLoggingEnabled)
                    {
                        FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " Outlook Modern Auth.");
                    }
                }
                // If the session request header Authorization equals Basic this is a Basic Auth capable client.
                else if (this.session.oRequest["Authorization"] == "Basic" && !(OverrideFurtherAuthChecking))
                {
                    this.session["X-Authentication"] = "Outlook Basic Auth";

                    this.session["X-AuthenticationDesc"] = "Outlook Basic Auth" +
                        Environment.NewLine +
                        Environment.NewLine +
                        "Outlook is stating it can do Basic Authentication. " +
                        "Whether or not Modern Authentication is enabled in Exchange Online this client session will use Basic Authentication." +
                        Environment.NewLine +
                        "In all likelihood this is an Outlook 2013 (updated prior to Modern Auth), Outlook 2010 or an older Outlook client, " +
                        "which does not support Modern Authentication." +
                        "MutiFactor Authentication will not work as expected with Basic Authentication only capable Outlook clients";

                    if (bAppLoggingEnabled)
                    {
                        FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " Outlook Basic Auth.");
                    }
                }
            }
            // Now we can check for Authorization headers which contain Bearer or Basic, signifying security tokens are being passed
            // from the Outlook client to Office 365 for resource access.
            //
            // Bearer == Modern Authentication.
            else if (this.session.oRequest["Authorization"].Contains("Bearer"))
            {
                SAMLParserFieldsNoData();

                this.session["X-Authentication"] = "Modern Auth Token";

                this.session["X-AuthenticationDesc"] = "Modern Auth Token" +
                        Environment.NewLine +
                        Environment.NewLine +
                        "Outlook accessing resources with a Modern Authentication security token.";

                if (bAppLoggingEnabled)
                {
                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " Modern Auth Token.");
                }
            }
            // Basic == Basic Authentication.
            else if (this.session.oRequest["Authorization"].Contains("Basic"))
            {
                SAMLParserFieldsNoData();

                this.session["X-Authentication"] = "Basic Auth Token";

                this.session["X-AuthenticationDesc"] = "Basic Auth Token" +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Outlook accessing resources with a Basic Authentication security token.";

                if (bAppLoggingEnabled)
                {
                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " Basic Auth Token.");
                }
            }
            else
            {
                SAMLParserFieldsNoData();

                this.session["X-Authentication"] = "--No Auth Headers";
                this.session["X-AuthenticationDesc"] = "--No Auth Headers";
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

        public void AutoTamperResponseBefore(Session session)
        {
            //throw new NotImplementedException();
        }

        public void AutoTamperResponseAfter(Session session)
        {
            this.session = session;

            /////////////////
            // Add in the Auth column. Due to these columns all being added as in with priority of 2,
            // they are added into the interface in this reverse order.
            if (bAuthColumnEnabled && bExtensionEnabled)
            {
                this.EnsureAuthColumn();
            }

            /////////////////
            // Add in the Response Server column. Due to these columns all being added as in with priority of 2,
            // they are added into the interface in this reverse order.
            if (bResponseServerColumnEnabled && bExtensionEnabled)
            {
                this.EnsureResponseServerColumn();
            }

            /////////////////
            // Add in the X-HostIP column. Due to these columns all being added as in with priority of 2,
            // they are added into the interface in this reverse order.
            if (bXHostIPColumnEnabled && bExtensionEnabled)
            {
                this.EnsureXHostIPColumn();
            }

            /////////////////
            // Add in the Exchange Type column. Due to these columns all being added as in with priority of 2,
            // they are added into the interface in this reverse order.
            if (bExchangeTypeColumnEnabled && bExtensionEnabled)
            {
                this.EnsureExchangeTypeColumn();
            }

            /////////////////
            // Add in the Elapsed Time column. Due to these columns all being added as in with priority of 2,
            // they are added into the interface in this reverse order.
            if (bElapsedTimeColumnEnabled && bExtensionEnabled)
            {
                this.EnsureElapsedTimeColumn();
            }

            // These get called on each session, seen strange behaviour on reordering on live trace due 
            // to setting each of these as ordering 2 to ensure column positions regardless of column enabled selections.
            // Use an if statement to fire these once per Fiddler application session.
            if (this.session.id == 1)
            {
                OrderColumns();
            }
        }

        public void OrderColumns()
        {
            if (bExtensionEnabled)
            {
                // Move the process column further to the left for visibility.
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Process", 2, -1);
            }
            else
            {
                // Since the extension is not enabled return the process column back to its original location.
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Process", 8, -1);
            }

            if (bExchangeTypeColumnEnabled && bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Exchange Type", 2, -1);
            }

            if (bAuthColumnEnabled && bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Authentication", 2, -1);
            }

            if (bXHostIPColumnEnabled && bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("X-HostIP", 2, -1);
            }

            if (bResponseServerColumnEnabled && bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Response Server", 2, -1);
            }

            if (bElapsedTimeColumnEnabled && bExtensionEnabled)
            {
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Elapsed Time", 2, -1);
            }
        }

        public void OnBeforeReturningError(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void OnLoad()
        {

            // We need to through some code to restore vanilla Fiddler configuration.
            /*
            bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);

            // Kill extension if not enabled.
            if (!(bExtensionEnabled))
            {
                // If the Fiddler application preference ExecutionCount exists and has a value, then this
                // is not a first run scenario. Go ahead and return, extension is not enabled.
                if (iExecutionCount > 0)
                {
                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: ColumnsUI.cs OnLoad Extension Return.");
                    return;
                }
            }
            */
            
            /////////////////
            /// <remarks>
            /// Response Time column function is no longer called here. Only in OnLoadSAZ.
            /// </remarks>
            /////////////////

            /////////////////
            /// <remarks>
            /// Call to function in ColumnsUI.cs to add Server Response column if the menu item is checked and if the extension is enabled.
            /// </remarks> 
            /// Refresh variable now to take account of first load code.
            //bResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
            EnsureResponseServerColumn();
            ///
            /////////////////

            /////////////////
            /// <remarks>
            /// Call to function in ColumnsUI.cs to add Exchange Type column if the menu item is checked and if the extension is enabled. 
            /// </remarks>
            /// Refresh variable now to take account of first load code.
            //bXHostIPColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.XHostIPColumnEnabled", false);
            EnsureXHostIPColumn();
            ///
            /////////////////

            /////////////////
            /// <remarks>
            /// Call to function in ColumnsUI.cs to add Exchange Type column if the menu item is checked and if the extension is enabled. 
            /// </remarks>
            /// Refresh variable now to take account of first load code.
            EnsureExchangeTypeColumn();
            ///
            /////////////////

            /////////////////
            /// <remarks>
            /// Call to function in ColumnsUI.cs to add Authentication column if the menu item is checked and if the extension is enabled. 
            /// </remarks>
            EnsureAuthColumn();
            ///
            /////////////////
        }

        public void OnBeforeUnload()
        {
            //throw new NotImplementedException();
        }

        // Populate the ElapsedTime column on live trace, if the column is enabled.
        // Code currently not used / under review.

        // if (boolElapsedTimeColumnEnabled && boolExtensionEnabled) {
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