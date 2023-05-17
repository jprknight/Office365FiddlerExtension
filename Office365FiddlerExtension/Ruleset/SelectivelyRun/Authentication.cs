using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Office365FiddlerExtension.Ruleset
{
    class Authentication : ActivationService
    {
        private static Authentication _instance;

        public static Authentication Instance => _instance ?? (_instance = new Authentication());

        public void SetAuthentication_NoAuthHeaders(Session session)
        {
            this.session = session;

            if (this.session.oRequest["Authorization"].Contains("Bearer") 
                || this.session.oRequest["Authorization"].Contains("Basic")
                || this.session.uriContains("adfs/ls"))
            {
                // Do nothing here, this is a session which is detected to have auth headers.
                return;
            }

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} Running SetAuthentication_NoAuthHeaders.");

            SessionFlagProcessor.Instance.SetProcess(this.session);

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication",
                Authentication = "No Auth Headers",
                AuthenticationDescription = "No Auth Headers",
                AuthenticationType = "No Auth Headers",

                SessionAuthenticationConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        public void SetAuthentication_SAML_Parser(Session session)
        {
            this.session = session;

            // Determine if this session contains a SAML response.
            if (this.session.utilFindInResponse("Issuer=", false) > 1 &&
                this.session.utilFindInResponse("Attribute AttributeName=", false) > 1 &&
                this.session.utilFindInResponse("NameIdentifier Format=", false) > 1 &&
                this.session.utilFindInResponse("Attribute AttributeName=", false) > 1)
            {
                FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} ADFS SAML Request/Response.");

                // wrap all of this in a check to see if the SAML token came back from an ADFS endpoint.
                // If it didn't we don't have the labs setup to validate how 3rd-party IDPs format things
                // out for SAML tokens.
                if (this.session.uriContains("adfs/ls"))
                {

                    var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
                    {
                        SectionTitle = "Authentication",
                        SessionType = "SAML Request/Response",
                        ResponseComments = "ADFS SAML response found. See below for SAML response parser.",

                        AuthenticationType = "SAMLResponseParser",
                        AuthenticationDescription = "ADFS SAML response found. See below for SAML response parser.",

                        SessionAuthenticationConfidenceLevel = 10,
                        SessionTypeConfidenceLevel = 10
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);

                    // JK 6/30/2021
                    // All the below logic was build with an ADFS SAML token from a lab environment.
                    // Discovered the makeup of SAML tokens from other providers do not follow the exact
                    // same structure.
                    // Added try catch statements and validation checks on string lengths prior to attempting
                    // substring operations to prevent running into "Length cannot be less than zero" exceptions.

                    SetTokenIssuer(this.session);

                    SetSigningCertificate(this.session);

                    SetAttributeNameUPN(this.session);

                    SetNameIdentifierFormat(this.session);

                    SetAttributeNameImmutableID(this.session);
                }
                else
                {
                    FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} Third-party SAML response found. SAML response parser not running.");

                    var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
                    {
                        SectionTitle = "Authentication",

                        SessionType = "3rd-party SAMLResponseParser",
                        ResponseComments = "Third-party SAML response found. SAML response parser not running.",

                        AuthenticationType = "Third-party SAMLResponseParser",
                        AuthenticationDescription = "Third-party SAML response found. SAML response parser not running.",

                        SamlTokenIssuer = "SAML token issued by third-party IDP. SAML response parser not running.",
                        SamlTokenSigningCertificate = "SAML token issued by third-party IDP. SAML response parser not running.",
                        SamlTokenAttributeNameUPN = "SAML token issued by third-party IDP. SAML response parser not running.",
                        SamlTokenNameIdentifierFormat = "SAML token issued by third-party IDP. SAML response parser not running.",
                        SamlTokenAttributeNameImmutibleID = "SAML token issued by third-party IDP. SAML response parser not running.",

                        SessionAuthenticationConfidenceLevel = 10
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
                }
            }
        }

        public void SetAuthentication_Basic_Modern_Auth_Disabled(Session session)
        {
            this.session = session;

            if (this.session.oRequest["Authorization"] != "Basic") {
                return;
            }

            SAMLParserFieldsNoData(this.session);

            int KeywordFourMillion = SearchSessionForWord(this.session, "4000000");
            int KeywordFlighting = SearchSessionForWord(this.session, "Flighting");
            int Keywordenabled = SearchSessionForWord(this.session, "enabled");
            int Keyworddomain = SearchSessionForWord(this.session, "domain");
            int Keywordoauth_not_available = SearchSessionForWord(this.session, "oauth_not_available");

            if (KeywordFourMillion == 0 && KeywordFlighting == 0 && Keywordenabled == 0 &&
                Keyworddomain == 0 && Keywordoauth_not_available == 0)
            {
                FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} Modern Auth Disabled.");

                DateTime today = DateTime.Today;

                var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
                {
                    SectionTitle = "Authentication",
                    ResponseComments = SessionProcessor.Instance.ResponseCommentsNoKnownIssue(),

                    Authentication = "Modern Auth Disabled",
                    AuthenticationDescription = "Microsoft365 workload has Modern Authentication disabled. "
                        + $"At this point in {today:yyyy} it's highly unusual for this to be the case."
                        + "<p>MutiFactor Authentication will not work as expected while Modern Authentication "
                        + "is disabled in the Microsoft365 workload."
                        + "For Exchange Online, the following is important for Outlook connectivity:</p>"
                        + "<p>Outlook 2010 and older do not support Modern Authentication and by extension MutliFactor Authentication.</p>"
                        + "<p>Outlook 2013 supports modern authentication with updates and the EnableADAL registry key set to 1.</p>"
                        + "<p>See https://support.microsoft.com/en-us/help/4041439/modern-authentication-configuration-requirements-for-transition-from-o </p>"
                        + "<p>Outlook 2016 or newer. No updates or registry keys needed for Modern Authentication.</p>",

                    SessionAuthenticationConfidenceLevel = 10
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
        }

        public void SetAuthentication_Modern_Auth_Capable_Client(Session session)
        {
            this.session = session;

            if (this.session.oRequest["Authorization"] != "Bearer")
            {
                return;
            }

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} Client Modern Auth.");

            var ExtensionSessionFlags = JsonConvert.DeserializeObject<SessionFlagProcessor.ExtensionSessionFlags>(SessionFlagProcessor.Instance.GetSessionJsonData(this.session));

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication",
                Authentication = "Client Modern Auth Capable",
                AuthenticationDescription = ExtensionSessionFlags.ProcessName + " is stating it is Modern Authentication capable. "
                + "Whether it is used or not will depend on whether Modern Authentication is enabled in the Office 365 service.",
                SessionAuthenticationConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        public void SetAuthentication_Basic_Auth_Capable_Client(Session session)
        {
            this.session = session;

            if (this.session.oRequest["Authorization"] != "Basic")
            {
                return;
            }

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} Client Basic Auth.");

            var ExtensionSessionFlags = JsonConvert.DeserializeObject<SessionFlagProcessor.ExtensionSessionFlags>(SessionFlagProcessor.Instance.GetSessionJsonData(this.session));

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication",

                Authentication = "Client Basic Auth Capable",
                AuthenticationDescription = ExtensionSessionFlags.ProcessName + " is stating it is Basic Authentication capable. "
                        + "Whether it is used or not will depend on whether Basic Authentication is enabled in the Office 365 service."
                        + "<p>If this is Outlook, in all likelihood this is an Outlook 2013 (updated prior to Modern Auth), Outlook 2010 or an "
                        + "older Outlook client, which does not support Modern Authentication.<br />"
                        + "MutiFactor Authentication will not work as expected with Basic Authentication only capable Outlook clients</p>",
                SessionAuthenticationConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        public void SetAuthentication_Modern_Auth_Client_Using_Token(Session session)
        {
            this.session = session;

            if (this.session.oRequest["Authorization"] != "Bearer")
            {
                return;
            }

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} Client Modern Auth Token.");

            var ExtensionSessionFlags = JsonConvert.DeserializeObject<SessionFlagProcessor.ExtensionSessionFlags>(SessionFlagProcessor.Instance.GetSessionJsonData(this.session));

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication",
                Authentication = "Modern Auth Token",
                AuthenticationDescription = ExtensionSessionFlags.ProcessName + " accessing resources with a Modern Authentication security token.",
                SessionAuthenticationConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);

        }

        public void SetAuthentication_Basic_Auth_Client_Using_Token(Session session)
        {
            this.session = session;

            if (this.session.oRequest["Authorization"] != "Basic")
            {
                return;
            }

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} Client Basic Auth Token.");
            
            var ExtensionSessionFlags = JsonConvert.DeserializeObject<SessionFlagProcessor.ExtensionSessionFlags>(SessionFlagProcessor.Instance.GetSessionJsonData(this.session));

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication",

                Authentication = "Client Basic Auth Token",
                AuthenticationDescription = ExtensionSessionFlags.ProcessName + " accessing resources with a Basic Authentication security token.< br /> "
                    + "<b><span style='color:red'>It's time to think about Modern Authentication!</span></b>",
                SessionAuthenticationConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        private void SetTokenIssuer(Session session)
        {
            this.session = session;

            if (!(this.session.utilFindInResponse("Issuer=", false) > 1) && !(this.session.utilFindInResponse("IssueInstant=", false) > 1))
            {
                var sessionFlagsNotDetermined = new SessionFlagProcessor.ExtensionSessionFlags()
                {
                    SectionTitle = "Authentication (SetTokenIssuer)",
                    Authentication = "Issuer in SAML token could not be determined."
                };

                var sessionFlagsJsonNotDetermined = JsonConvert.SerializeObject(sessionFlagsNotDetermined);
                SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJsonNotDetermined);

                return;
            }

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} SAML token issuer.");

            string Issuer;
            try
            {
                // Pull issuer data from response.
                string IssuerSessionBody = this.session.ToString();
                int IssuerStartIndex = IssuerSessionBody.IndexOf("Issuer=");
                int IssuerEndIndex = IssuerSessionBody.IndexOf("IssueInstant=");
                int IssuerLength = IssuerEndIndex - IssuerStartIndex;
                if (IssuerLength > 0)
                {
                    Issuer = IssuerSessionBody.Substring(IssuerStartIndex, IssuerLength);
                    Issuer = Issuer.Replace("&quot;", "");
                    Issuer = Issuer.Replace("Issuer=", "");
                }
                else
                {
                    Issuer = "Issuer in SAML token could not be determined.";
                }

            }
            catch (Exception e)
            {
                FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} SAML token issuer could not be determined. {e}");
                Issuer = "Issuer in SAML token could not be determined.";
            }

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication (SetTokenIssuer)",
                SamlTokenIssuer = Issuer
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        private void SetSigningCertificate(Session session)
        {
            this.session = session;

            if (!(this.session.utilFindInResponse("&lt;X509Certificate>", false) > 1) && !(this.session.utilFindInResponse("&lt;/X509Certificate>", false) > 1))
            {
                var sessionFlagsNotDetermined = new SessionFlagProcessor.ExtensionSessionFlags()
                {
                    SectionTitle = "Authentication (SigningCertificate)",
                    SamlTokenSigningCertificate = "Data points not found for SigningCertificate."
                };

                var sessionFlagsJsonNotDetermined = JsonConvert.SerializeObject(sessionFlagsNotDetermined);
                SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJsonNotDetermined);

                return;
            }

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} SAML signing certificate.");

            string x509SigningCertificate;
            try
            {
                string x509SigningCertSessionBody = this.session.ToString();
                int x509SigningCertificateStartIndex = x509SigningCertSessionBody.IndexOf("&lt;X509Certificate>") + 20; // 20 to shift to start of the selection.
                int x509SigningCertificateEndIndex = x509SigningCertSessionBody.IndexOf("&lt;/X509Certificate>");
                int x509SigningCertificateLength = x509SigningCertificateEndIndex - x509SigningCertificateStartIndex;
                if (x509SigningCertificateLength > 0)
                {
                    x509SigningCertificate = x509SigningCertSessionBody.Substring(x509SigningCertificateStartIndex, x509SigningCertificateLength);
                }
                else
                {
                    x509SigningCertificate = "SAML signing certificate could not be determined.";
                }

            }
            catch (Exception e)
            {
                FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} SAML signing certificate could not be determined. {e}");
                x509SigningCertificate = "SAML signing certificate could not be determined.";
            }

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication (SigningCertificate)",
                SamlTokenSigningCertificate = x509SigningCertificate
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        private void SetAttributeNameUPN(Session session)
        {
            this.session = session;

            if (!(this.session.utilFindInResponse("&lt;saml:Attribute AttributeName=&quot;UPN", false) > 1) &&
                !(this.session.utilFindInResponse("&lt;/saml:Attribute>", false) > 1))
            {

                var sessionFlagsNotDetermined = new SessionFlagProcessor.ExtensionSessionFlags()
                {
                    SectionTitle = "Authentication (AttributeNameUPN)",
                    SamlTokenAttributeNameUPN = "Data points not found for AttributeNameUPN"
                };

                var sessionFlagsJsonNotDetermined = JsonConvert.SerializeObject(sessionFlagsNotDetermined);
                SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJsonNotDetermined);

                return;
            }

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} SAML AttributeNameUPN.");

            string AttributeNameUPN;
            try
            {
                string AttributeNameUPNSessionBody = this.session.ToString();
                int AttributeNameUPNStartIndex = AttributeNameUPNSessionBody.IndexOf("&lt;saml:Attribute AttributeName=&quot;UPN");
                int AttributeNameUPNEndIndex = AttributeNameUPNSessionBody.IndexOf("&lt;/saml:Attribute>");
                int AttributeNameUPNLength = AttributeNameUPNEndIndex - AttributeNameUPNStartIndex;
                if (AttributeNameUPNLength > 0)
                {
                    AttributeNameUPN = AttributeNameUPNSessionBody.Substring(AttributeNameUPNStartIndex, AttributeNameUPNLength);
                    AttributeNameUPN = AttributeNameUPN.Replace("&quot;", "\"");
                    AttributeNameUPN = AttributeNameUPN.Replace("&lt;", "<");
                    // Now split the two lines with a new line for easier reading in the user control.
                    int SplitAttributeNameUPNStartIndex = AttributeNameUPN.IndexOf("<saml:AttributeValue>") + 21;

                    int SplitAttributeNameUPNEndIndex = AttributeNameUPN.IndexOf("</saml:AttributeValue>");
                    int SplitAttributeNameLength = SplitAttributeNameUPNEndIndex - SplitAttributeNameUPNStartIndex;

                    //string AttributeNameUPNFirstLine = AttributeNameUPN.Substring(0, SplitAttributeNameUPNStartIndex);
                    //string AttributeNameUPNSecondLine = AttributeNameUPN.Substring(SplitAttributeNameUPNStartIndex);

                    if (SplitAttributeNameLength > 0)
                    {
                        AttributeNameUPN = AttributeNameUPN.Substring(SplitAttributeNameUPNStartIndex, SplitAttributeNameLength);
                    }
                    else
                    {
                        AttributeNameUPN = "SAML AttributeNameUPN could not be determined.";
                    }
                }
                else
                {
                    AttributeNameUPN = "SAML AttributeNameUPN could not be determined.";
                }

            }
            catch (Exception e)
            {
                FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} SAML AttributeNameUPN could not be determined. {e}");
                AttributeNameUPN = "SAML AttributeNameUPN could not be determined.";
            }

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication (AttributeNameUPN)",
                SamlTokenAttributeNameUPN = AttributeNameUPN,
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        private void SetNameIdentifierFormat(Session session)
        {
            this.session = session;

            if (!(this.session.utilFindInResponse("&lt;saml:NameIdentifier Format", false) > 1) &&
                !(this.session.utilFindInResponse("&lt;saml:SubjectConfirmation>", false) > 1))
            {

                var sessionFlagsNotDetermined = new SessionFlagProcessor.ExtensionSessionFlags()
                {
                    SectionTitle = "Authentication (NameIdentifierFormat)",
                    SamlTokenNameIdentifierFormat = "Data points not found for NameIdentifierFormat"
                };

                var sessionFlagsJsonNotDetermined = JsonConvert.SerializeObject(sessionFlagsNotDetermined);
                SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJsonNotDetermined);

                return;
            }

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} SAML NameIdentifierFormat.");

            string NameIdentifierFormat;
            try
            {
                string NameIdentifierFormatSessionBody = this.session.ToString();
                int NameIdentifierFormatStartIndex = NameIdentifierFormatSessionBody.IndexOf("&lt;saml:NameIdentifier Format");
                int NameIdentifierFormatEndIndex = NameIdentifierFormatSessionBody.IndexOf("&lt;saml:SubjectConfirmation>");
                int NameIdentifierFormatLength = NameIdentifierFormatEndIndex - NameIdentifierFormatStartIndex;
                if (NameIdentifierFormatLength > 0)
                {
                    NameIdentifierFormat = NameIdentifierFormatSessionBody.Substring(NameIdentifierFormatStartIndex, NameIdentifierFormatLength);
                    NameIdentifierFormat = NameIdentifierFormat.Replace("&quot;", "\"");
                    NameIdentifierFormat = NameIdentifierFormat.Replace("&lt;", "<");
                }
                else
                {
                    NameIdentifierFormat = "SAML NameIdentifierFormat could not be determined.";
                }
            }
            catch (Exception e)
            {
                FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} SAML NameIdentifierFormat could not be determined. {e}");
                NameIdentifierFormat = "SAML NameIdentifierFormat could not be determined.";
            }

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication (NameIdentifierFormat)",
                SamlTokenNameIdentifierFormat = NameIdentifierFormat,
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        private void SetAttributeNameImmutableID(Session session)
        {
            // AttributeNameImmutableID

            this.session = session;

            if (!(this.session.utilFindInResponse("&lt;saml:NameIdentifier Format", false) > 1) &&
                !(this.session.utilFindInResponse("&lt;saml:SubjectConfirmation>", false) > 1))
            {
                var sessionFlagsNotDetermined = new SessionFlagProcessor.ExtensionSessionFlags()
                {
                    SectionTitle = "Authentication (TokenAttributeNameImmutibleID)",
                    SamlTokenAttributeNameImmutibleID = "Data points not found for TokenAttributeNameImmutibleID"
                };

                var sessionFlagsJsonNotDetermined = JsonConvert.SerializeObject(sessionFlagsNotDetermined);
                SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJsonNotDetermined);

                return;
            }

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} SAML AttributeNameImmutableID.");

            string AttributeNameImmutibleID;
            try
            {
                string AttributeNameImmutableIDSessionBody = this.session.ToString();
                int AttributeNameImmutableIDStartIndex = AttributeNameImmutableIDSessionBody.IndexOf("AttributeName=&quot;ImmutableID");
                int AttributeNameImmutibleIDEndIndex = AttributeNameImmutableIDSessionBody.IndexOf("&lt;/saml:AttributeStatement>");
                int AttributeNameImmutibleIDLength = AttributeNameImmutibleIDEndIndex - AttributeNameImmutableIDStartIndex;

                if (AttributeNameImmutibleIDLength > 0)
                {
                    AttributeNameImmutibleID = AttributeNameImmutableIDSessionBody.Substring(AttributeNameImmutableIDStartIndex, AttributeNameImmutibleIDLength);
                    AttributeNameImmutibleID = AttributeNameImmutibleID.Replace("&quot;", "\"");
                    AttributeNameImmutibleID = AttributeNameImmutibleID.Replace("&lt;", "<");
                    // Now split out response with a newline for easier reading.
                    int SplitAttributeNameImmutibleIDStartIndex = AttributeNameImmutibleID.IndexOf("<saml:AttributeValue>") + 21;
                    // Add 21 characters to shift where the newline is placed.
                    //string AttributeNameImmutibleIDFirstLine = AttributeNameImmutibleID.Substring(0, SplitAttributeNameImmutibleIDStartIndex);
                    //string AttributeNameImmutibleIDSecondLine = AttributeNameImmutibleID.Substring(SplitAttributeNameImmutibleIDStartIndex);
                    //AttributeNameImmutibleID = AttributeNameImmutibleIDFirstLine + Environment.NewLine + AttributeNameImmutibleIDSecondLine;
                    // Second split
                    int SplitAttributeNameImmutibleIDEndIndex = AttributeNameImmutibleID.IndexOf("</saml:AttributeValue></saml:Attribute>");
                    int SubstringLength = SplitAttributeNameImmutibleIDEndIndex - SplitAttributeNameImmutibleIDStartIndex;

                    if (SubstringLength > 0)
                    {
                        AttributeNameImmutibleID = AttributeNameImmutibleID.Substring(SplitAttributeNameImmutibleIDStartIndex, SubstringLength);
                    }
                    else
                    {
                        AttributeNameImmutibleID = "SAML AttributeNameImmutibleID could not be determined.";
                    }
                    //AttributeNameImmutibleIDFirstLine = AttributeNameImmutibleID.Substring(0, SplitAttributeNameImmutibleIDStartIndex);
                    //AttributeNameImmutibleIDSecondLine = AttributeNameImmutibleID.Substring(SplitAttributeNameImmutibleIDStartIndex);
                    //AttributeNameImmutibleID = AttributeNameImmutibleIDFirstLine + Environment.NewLine + AttributeNameImmutibleIDSecondLine;
                }
                else
                {
                    AttributeNameImmutibleID = "SAML AttributeNameImmutibleID could not be determined.";
                }
            }
            catch (Exception e)
            {
                FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} SAML AttributeNameImmutibleID could not be determined. {e}");
                AttributeNameImmutibleID = "SAML AttributeNameImmutibleID could not be determined.";
            }

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication (TokenAttributeNameImmutibleID)",
                SamlTokenAttributeNameImmutibleID = AttributeNameImmutibleID
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
        

        private void SAMLParserFieldsNoData(Session session)
        {
            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication (SAMLParserFieldsNoData)",
                SamlTokenIssuer = "No SAML Data in session",
                SamlTokenAttributeNameUPN = "No SAML Data in session",
                SamlTokenNameIdentifierFormat = "No SAML Data in session",
                SamlTokenAttributeNameImmutibleID = "No SAML Data in session"
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        private int SearchSessionForWord(Session session, string searchTerm)
        {
            // Count the occurrences of common search terms match up to certain HTTP response codes to highlight certain scenarios.
            //
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-count-occurrences-of-a-word-in-a-string-linq
            //

            string text = session.ToString();

            //Convert the string into an array of words  
            string[] source = text.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',', '"' }, StringSplitOptions.RemoveEmptyEntries);

            // Create the query. Use ToLowerInvariant to match "data" and "Data"   
            var matchQuery = from word in source
                             where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                             select word;

            // Count the matches, which executes the query.  
            int wordCount = matchQuery.Count();

            return wordCount;
        }
    }
}