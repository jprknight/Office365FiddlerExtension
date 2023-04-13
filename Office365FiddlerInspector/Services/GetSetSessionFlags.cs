using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;

namespace Office365FiddlerInspector.Services
{
    // The name SessionFlags is already taken by Fiddler, changed to resolve abiquity.
    class GetSetSessionFlags : ActivationService
    {

        private static GetSetSessionFlags _instance;
        public static GetSetSessionFlags Instance => _instance ?? (_instance = new GetSetSessionFlags());

        // Colour codes for sessions. Softer tones, easier on the eye than standard red, orange and green.
        public void SetUIBackColour(Session session, String Colour)
        {
            switch (Colour) {
                case "blue":
                    this.session["ui-backcolor"] = "#81BEF7";
                    break;
                case "Green":
                    this.session["ui-backcolor"] = "#81F7BA";
                    break;
                case "Red":
                    this.session["ui-backcolor"] = "#F06141";
                    break;
                case "Gray":
                    this.session["ui-backcolor"] = "#BDBDBD";
                    break;
                case "Orange":
                    this.session["ui-backcolor"] = "#F59758";
                    break;
                case "Black":
                    this.session["ui-backcolor"] = "#000000";
                    break;
                default:
                    // Default to pink, so we know if something isn't caught.
                    this.session["ui-backcolor"] = "#FFC0CB";
                    break;
            }
        }

        public void SetUITextColour(Session session, String Colour)
        {
            switch (Colour)
            {
                case "Black":
                    this.session["ui-color"] = "#000000";
                    break;
                case "Red":
                    this.session["ui-color"] = "#F06141";
                    break;
                default:
                    this.session["ui-color"] = "#000000";
                    break;
            }
        }

        // SESSION CLASSIFICATIONS

        // How are session classifications used?
        // None; -5 : Session classification isn't set.
        // Low;  0  : Session classification has low confidence, any and all subsequent functions should be run to further attempt to classify the session.
        // Mid;  5  : Session classification has some confidence, but overriding functions should be run just in case.
        // High; 10 : Session classification has high level of confidence and any overriding functions should not be run.

        public Boolean GetAnySessionConfidenceLevelTen(Session session)
        {
            this.session = session;
            if (GetSessionAuthenticationConfidenceLevel(this.session) == 10 ||
                GetSessionResponseServerConfidenceLevel(this.session) == 10 ||
                GetSessionTypeConfidenceLevel(this.session) == 10)
            {
                return true;
            }
            return false;
        }

        public int GetSessionAuthenticationConfidenceLevel(Session session)
        {
            this.session = session;

            // Avoid null object exceptions by setting this session flag to something
            // rather than nothing.
            if (this.session["X-SACL"] == null || this.session["X-SACL"] == "")
            {
                this.session["X-SACL"] = "-5";
            }

            return int.Parse(session["X-SACL"]);
        }

        // Set Session Authentication Confidence Level.
        public void SetSessionAuthenticationConfidenceLevel(Session session, string SessionAuthenticationConfidenceLevel)
        {
            this.session = session;
            this.session["X-SACL"] = SessionAuthenticationConfidenceLevel;
        }

        // Get Session Type Confidence Level.
        public int GetSessionTypeConfidenceLevel(Session session)
        {
            this.session = session;
            // Avoid null object exceptions by setting this session flag to something
            // rather than nothing.
            if (this.session["X-STCL"] == null || this.session["X-STCL"] == "")
            {
                this.session["X-STCL"] = "-5";
            }
            return int.Parse(session["X-STCL"]);
        }

        // Set Session Type Confidence Level.
        public void SetSessionTypeConfidenceLevel(Session session, string SessionTypeConfidenceLevel)
        {
            this.session = session;
            this.session["X-STCL"] = SessionTypeConfidenceLevel;
        }

        // Get Session Response Server Confidence Level.
        public int GetSessionResponseServerConfidenceLevel(Session session)
        {
            this.session = session;
            // Avoid null object exceptions by setting this session flag to something
            // rather than nothing.
            if (this.session["X-SRSCL"] == null || this.session["X-SRSCL"] == "")
            {
                this.session["X-SRSCL"] = "-5";
            }
            return int.Parse(this.session["X-SRSCL"]);
        }

        // Set Session Response Server Confidence Level.
        public void SetSessionResponseServerConfidenceLevel(Session session, string SessionResponseServerConfidenceLevel)
        {
            this.session = session;
            this.session["X-SRSCL"] = SessionResponseServerConfidenceLevel;
        }

        public void SetProcess(Session session)
        {
            this.session = session;
            // Set process name, split and exclude port used.
            if (session.LocalProcess != String.Empty)
            {
                string[] ProcessName = session.LocalProcess.Split(':');
                session["X-ProcessName"] = ProcessName[0];
            }
            // No local process to split.
            else
            {
                session["X-ProcessName"] = "Remote Capture";
            }
        }

        public void SetResponseCodeDescription(Session session, String ResponseCodeDescription)
        {
            this.session = session;
            this.session["X-ResponseCodeDescription"] = ResponseCodeDescription;
        }

        public void SetSessionType(Session session, String SessionType)
        {
            this.session = session;
            this.session["X-SessionType"] = SessionType;
        }

        public void SetXAuthentication(Session session, String Authentication)
        {
            this.session = session;
            this.session["X-Authentication"] = Authentication;
        }

        public void SetXResponseServer(Session session, String ResponseServer)
        {
            this.session = session;
            this.session["X-ResponseServer"] = ResponseServer;
        }
        
        public void SetXResponseAlert(Session session, String ResponseAlert)
        {
            this.session = session;
            this.session["X-ResponseAlert"] = ResponseAlert;
        }

        public void SetXResponseComments(Session session, String ResponseComments)
        {
            this.session = session;
            this.session["X-ResponseComments"] = ResponseComments;
        }

        public void SetXResponseCommentsNoKnownIssue(Session session)
        {
            this.session = session;
            this.session["X-ResponseComments"] = "<p>No known issue with Office 365 and this type of session. If you have a suggestion for an improvement, "
                + "create an issue or better yet a pull request in the project Github repository: "
                + "<a href='https://aka.ms/Office365FiddlerExtension' target='_blank'>https://aka.ms/Office365FiddlerExtension</a>.</p>";
        }

        public void SetXDateDataCollected(Session session, String DateDataCollected)
        {
            this.session = session;
            this.session["X-DataCollected"] = DateDataCollected;
        }

        public void SetXDataAge(Session session, String DataAge)
        {
            this.session = session;
            this.session["X-DataAge"] = DataAge;
        }

        public void SetXCalculatedSessionAge(Session session, String CalculatedSessionAge)
        {
            this.session = session;
            this.session["X-CalculatedSessionAge"] = CalculatedSessionAge;
        }

        public void SetXSessionTimersDescription(Session session, String SessionTimersDescription)
        {
            this.session = session;
            this.session["X-SessionTimersDescription"] = SessionTimersDescription;
        }

        public void SetXServerThinkTime(Session session, String ServerThinkTime)
        {
            this.session = session;
            this.session["X-ServerThinkTime"] = ServerThinkTime;
        }

        public void SetXTransitTime(Session session, String TransitTime)
        {
            this.session["X-TransitTime"] = TransitTime;
        }

        public void SetXElapsedTime(Session sesssion, String ElapsedTime)
        {
            this.session = session;
            this.session["X-ElapsedTime"] = ElapsedTime;
        }

        public void SetXInspectorElapsedTime(Session session, String InspectorElapsedTime)
        {
            this.session = session;
            this.session["X-InspectorElapsedTime"] = InspectorElapsedTime;
        }
    }
}