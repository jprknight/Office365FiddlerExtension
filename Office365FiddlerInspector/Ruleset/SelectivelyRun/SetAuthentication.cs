﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;

namespace Office365FiddlerExtension.Ruleset
{
    class SetAuthentication : ActivationService
    {
        //GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        private static SetAuthentication _instance;

        public static SetAuthentication Instance => _instance ?? (_instance = new SetAuthentication());

        // Functions where Authentication column is populated and SAML parser code lives.
        public void SetAuthenticationData(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Running SetAuthentication.");

            GetSetSessionFlags.Instance.SetXOffice365AuthType(this.session, "");

            GetSetSessionFlags.Instance.SetProcess(this.session);

            // Logic check so we don't walk through the SAML Parser on every session.
            if (this.session.oRequest["Authorization"].Contains("Bearer") || this.session.oRequest["Authorization"].Contains("Basic")
                || this.session.uriContains("adfs/ls"))
            {
                // Do nothing here, this is a session which is detected to have auth headers.
                // Let the Auth / SAML parser run through.
            }
            else
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " No Auth Headers found in session.");
                
                SAMLParserFieldsNoData(this.session);
                // Change which control appears for this session on the Office365 Auth inspector tab.
                GetSetSessionFlags.Instance.SetXOffice365AuthType(this.session, "Office365Auth");

                GetSetSessionFlags.Instance.SetXAuthentication(this.session, "No Auth Headers");
                GetSetSessionFlags.Instance.SetXAuthenticationDescription(this.session, "No Auth Headers");
                
                // Set SCCL to 10, stop any further session processing.
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "10");

                // REVIEW THIS.
                return;
            }

            // SetAuthenticationSAMLParser

            // Determine if this session contains a SAML response.
            if (this.session.utilFindInResponse("Issuer=", false) > 1 &&
                this.session.utilFindInResponse("Attribute AttributeName=", false) > 1 &&
                this.session.utilFindInResponse("NameIdentifier Format=", false) > 1 &&
                this.session.utilFindInResponse("Attribute AttributeName=", false) > 1)
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " SAML Request/Response.");

                GetSetSessionFlags.Instance.SetXAuthenticationDescription(this.session, "SAML Request/Response");

                GetSetSessionFlags.Instance.SetSessionType(this.session, "SAML Request/Response");

                // wrap all of this in a check to see if the SAML token came back from an ADFS endpoint.
                // If it didn't we don't have the labs setup to validate how 3rd-party IDPs format things
                // out for SAML tokens.
                if (this.session.uriContains("adfs/ls"))
                {
                    // Used in session analysis. Needs to be set here to override the unclassified response.
                    GetSetSessionFlags.Instance.SetXResponseComments(this.session, "ADFS SAML response found. See below for SAML response parser.");

                    // Used in Auth column and Office365 Auth inspector tab.
                    GetSetSessionFlags.Instance.SetXAuthenticationDescription(this.session, "ADFS SAML response found. See below for SAML response parser.");

                    // Change which control appears for this session on the Office365 Auth inspector tab.
                    GetSetSessionFlags.Instance.SetXOffice365AuthType(this.session, "SAMLResponseParser");

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
                    
                    // Set SCCL to 10, stop any further session processing.
                    GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "10");
                    GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
                }
                else
                {
                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Third-party SAML response found. SAML response parser not running.");

                    GetSetSessionFlags.Instance.SetXResponseComments(this.session, "Third-party SAML response found. SAML response parser not running.");

                    // Used in Auth column and Office365 Auth inspector tab.
                    GetSetSessionFlags.Instance.SetXAuthenticationDescription(this.session, "Third-party SAML response found. SAML response parser not running.");

                    // Change which control appears for this session on the Office365 Auth inspector tab.
                    // REVIEW THIS. Is it needed?
                    GetSetSessionFlags.Instance.SetXOffice365AuthType(this.session, "SAMLResponseParser");

                    GetSetSessionFlags.Instance.SetSamlTokenIssuer(this.session, "SAML token issued by third-party IDP. SAML response parser not running.");

                    GetSetSessionFlags.Instance.SetSamlTokenSigningCertificate(this.session, "SAML token issued by third-party IDP. SAML response parser not running.");

                    GetSetSessionFlags.Instance.SetSamlTokenAttributeNameUPN(this.session, "SAML token issued by third-party IDP. SAML response parser not running.");

                    GetSetSessionFlags.Instance.SetSamlTokenNameIdentifierFormat(this.session, "SAML token issued by third-party IDP. SAML response parser not running.");

                    GetSetSessionFlags.Instance.SetSamlTokenAttributeNameImmutibleID(this.session, "SAML token issued by third-party IDP. SAML response parser not running.");

                    // Set SCCL to 10, stop any further session processing.
                    GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "10");
                }

            }
            // Determine if Modern Authentication is enabled in session request.
            else if (this.session.oRequest["Authorization"] == "Bearer" || this.session.oRequest["Authorization"] == "Basic")
            {
                SAMLParserFieldsNoData(this.session);

                // Change which control appears for this session on the Office365 Auth inspector tab.
                // REVIEW THIS. Is this needed?
                GetSetSessionFlags.Instance.SetXOffice365AuthType(this.session, "Office365Auth");

                // Looking for the following in a response body:
                // x-ms-diagnostics: 4000000;reason="Flighting is not enabled for domain 'user@contoso.com'.";error_category="oauth_not_available"

                int KeywordFourMillion = SearchSessionForWord(this.session, "4000000");
                int KeywordFlighting = SearchSessionForWord(this.session, "Flighting");
                int Keywordenabled = SearchSessionForWord(this.session, "enabled");
                int Keyworddomain = SearchSessionForWord(this.session, "domain");
                int Keywordoauth_not_available = SearchSessionForWord(this.session, "oauth_not_available");

                // Check if all the above checks have a value of at least 1. 
                // If they do, then the Office 365 workload (Exchange Online / Skype etc) is configured with Modern Authentication disabled.
                if (KeywordFourMillion > 0 && KeywordFlighting > 0 && Keywordenabled > 0 &&
                    Keyworddomain > 0 && Keywordoauth_not_available > 0 && this.session.HostnameIs("autodiscover-s.outlook.com"))
                {
                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Modern Auth Disabled.");

                    GetSetSessionFlags.Instance.SetXAuthentication(this.session, "Modern Auth Disabled");

                    DateTime today = DateTime.Today;

                    GetSetSessionFlags.Instance.SetXAuthenticationDescription(this.session, "Office 365 workload has Modern Authentication disabled. "
                        + $"At this point in {today:yyyy} there isn't a good reason to not have Modern Authentication turned on or having a plan to turn it on."
                        + "<p>MutiFactor Authentication will not work as expected while Modern Authentication "
                        + "is disabled in the Office 365 workload."
                        + "For Exchange Online, the following is important for Outlook connectivity:</p>"
                        + "<p>Outlook 2010 and older do not support Modern Authentication and by extension MutliFactor Authentication.</p>"
                        + "<p>Outlook 2013 supports modern authentication with updates and the EnableADAL registry key set to 1.</p>"
                        + "<p>See https://support.microsoft.com/en-us/help/4041439/modern-authentication-configuration-requirements-for-transition-from-o </p>"
                        + "<p>Outlook 2016 or newer. No updates or registry keys needed for Modern Authentication.</p>");

                    

                    // Set SCCL to 10, stop any further session processing.
                    GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "10");
                }

                // Now get specific to find out what the client can do.
                // If the session request header Authorization equals Bearer this is a Modern Auth capable client.
                if (this.session.oRequest["Authorization"] == "Bearer")
                {
                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Client Modern Auth.");

                    GetSetSessionFlags.Instance.SetXAuthentication(this.session, "Client Modern Auth Capable");

                    GetSetSessionFlags.Instance.SetXAuthenticationDescription(this.session, GetSetSessionFlags.Instance.GetProcess(this.session) + " is stating it is Modern Authentication capable. "
                        + "Whether it is used or not will depend on whether Modern Authentication is enabled in the Office 365 service.");

                    // Set SCCL to 10, stop any further session processing.
                    GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "10");
                }
                // If the session request header Authorization equals Basic this is a Basic Auth capable client.
                else if (this.session.oRequest["Authorization"] == "Basic")
                {
                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Outlook Basic Auth.");

                    GetSetSessionFlags.Instance.SetXAuthentication(this.session, "Client Basic Auth Capable");

                    GetSetSessionFlags.Instance.SetXAuthenticationDescription(this.session, GetSetSessionFlags.Instance.GetProcess(this.session) + " is stating it is Basic Authentication capable. "
                        + "Whether it is used or not will depend on whether Basic Authentication is enabled in the Office 365 service."
                        + "<p>If this is Outlook, in all likelihood this is an Outlook 2013 (updated prior to Modern Auth), Outlook 2010 or an "
                        + "older Outlook client, which does not support Modern Authentication.<br />"
                        + "MutiFactor Authentication will not work as expected with Basic Authentication only capable Outlook clients</p>");

                    // Set SCCL to 10, stop any further session processing.
                    GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "10");
                }
            }
            // Now we can check for Authorization headers which contain Bearer or Basic, signifying security tokens are being passed
            // from the Outlook client to Office 365 for resource access.
            //
            // Bearer == Modern Authentication.
            else if (this.session.oRequest["Authorization"].Contains("Bearer"))
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Modern Auth Token.");

                SAMLParserFieldsNoData(this.session);

                GetSetSessionFlags.Instance.SetXAuthentication(this.session, "Modern Auth Token");

                GetSetSessionFlags.Instance.SetXAuthenticationDescription(this.session, GetSetSessionFlags.Instance.GetProcess(this.session) + " accessing resources with a Modern Authentication security token.");

                // Set SCCL to 10, stop any further session processing.
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "10");
            }
            // Basic == Basic Authentication.
            else if (this.session.oRequest["Authorization"].Contains("Basic"))
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Basic Auth Token.");

                SAMLParserFieldsNoData(this.session);

                GetSetSessionFlags.Instance.SetXAuthentication(this.session, "Basic Auth Token");

                GetSetSessionFlags.Instance.SetXAuthenticationDescription(this.session, $"Process '{GetSetSessionFlags.Instance.GetProcess(this.session)}' accessing resources with a Basic Authentication security token.<br />"
                    + "<b><span style='color:red'>It's time to think about Modern Authentication!</span></b>");

                // Set SCCL to 10, stop any further session processing.
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "10");
            }
            // ADFS session with no other defining features yet classified.
            else
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " ADFS.");

                SAMLParserFieldsNoData(this.session);

                GetSetSessionFlags.Instance.SetXAuthentication(this.session, "ADFS");

                GetSetSessionFlags.Instance.SetXAuthenticationDescription(this.session, $"Process '{GetSetSessionFlags.Instance.GetProcess(this.session)}' communicating with ADFS at {session.hostname}.<br />");
                
                // Set SCCL to 10, stop any further session processing.
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "10");
            }

        }

        private void SetTokenIssuer(Session session)
        {
            // Issuer

            this.session = session;

            if (!(this.session.utilFindInResponse("Issuer=", false) > 1) && !(this.session.utilFindInResponse("IssueInstant=", false) > 1))
            {
                GetSetSessionFlags.Instance.SetSamlTokenIssuer(this.session, "Issuer in SAML token could not be determined.");
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

            // Populate X flag on session.
            GetSetSessionFlags.Instance.SetSamlTokenIssuer(this.session, Issuer);
        }

        private void SetSigningCertificate(Session session)
        {
            // SigningCertificate

            this.session = session;

            // Pull the x509 signing certificate data.
            if (!(this.session.utilFindInResponse("&lt;X509Certificate>", false) > 1) && !(this.session.utilFindInResponse("&lt;/X509Certificate>", false) > 1))
            {
                GetSetSessionFlags.Instance.SetSamlTokenSigningCertificate(this.session, "Data points not found for SigningCertificate.");
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
            GetSetSessionFlags.Instance.SetSamlTokenSigningCertificate(this.session, x509SigningCertificate);
        }

        private void SetAttributeNameUPN(Session session)
        {
            // AttributeNameUPN
            this.session = session;

            if (!(this.session.utilFindInResponse("&lt;saml:Attribute AttributeName=&quot;UPN", false) > 1) &&
                !(this.session.utilFindInResponse("&lt;/saml:Attribute>", false) > 1))
            {
                GetSetSessionFlags.Instance.SetSamlTokenAttributeNameUPN(this.session, "Data points not found for AttributeNameUPN");
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

            // Populate X flag on session.
            GetSetSessionFlags.Instance.SetSamlTokenAttributeNameUPN(this.session, AttributeNameUPN);
        }

        private void SetNameIdentifierFormat(Session session)
        {
            // NameIdentifierFormat
            /////////////////////////////
            //
            // NameIdentifierFormat.

            this.session = session;

            if (!(this.session.utilFindInResponse("&lt;saml:NameIdentifier Format", false) > 1) &&
                !(this.session.utilFindInResponse("&lt;saml:SubjectConfirmation>", false) > 1))
            {
                GetSetSessionFlags.Instance.SetSamlTokenNameIdentifierFormat(this.session, "Data points not found for NameIdentifierFormat");
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

            // Populate X flag on session.
            GetSetSessionFlags.Instance.SetSamlTokenNameIdentifierFormat(this.session, NameIdentifierFormat);
        }

        private void SetAttributeNameImmutableID(Session session)
        {
            // AttributeNameImmutableID

            this.session = session;

            if (!(this.session.utilFindInResponse("&lt;saml:NameIdentifier Format", false) > 1) &&
                !(this.session.utilFindInResponse("&lt;saml:SubjectConfirmation>", false) > 1))
            {
                GetSetSessionFlags.Instance.SetSamlTokenAttributeNameImmutibleID(this.session, "Data points not found for TokenAttributeNameImmutibleID");
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

            // Populate X flag on session.
            GetSetSessionFlags.Instance.SetSamlTokenAttributeNameImmutibleID(this.session, AttributeNameImmutibleID);
        }
        

        private void SAMLParserFieldsNoData(Session session)
        {
            GetSetSessionFlags.Instance.SetSamlTokenIssuer(this.session, "No SAML Data in session");
            GetSetSessionFlags.Instance.SetSamlTokenAttributeNameUPN(this.session, "No SAML Data in session");
            GetSetSessionFlags.Instance.SetSamlTokenNameIdentifierFormat(this.session, "No SAML Data in session");
            GetSetSessionFlags.Instance.SetSamlTokenAttributeNameImmutibleID(this.session, "No SAML Data in session");
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