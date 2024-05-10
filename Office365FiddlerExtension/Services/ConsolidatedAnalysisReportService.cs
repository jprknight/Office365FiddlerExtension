using Fiddler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Office365FiddlerExtension.Services
{
    class ConsolidatedAnalysisReportService
    {

        internal Session session { get; set; }

        private static ConsolidatedAnalysisReportService _instance;

        public static ConsolidatedAnalysisReportService Instance => _instance ?? (_instance = new ConsolidatedAnalysisReportService());

        public StringBuilder ResultsString { get; set; }

        public StringBuilder InterestingSessions {  get; set; }

        /// <summary>
        /// Create Consolidation Analysis Report.
        /// </summary>
        public void CreateCAR()
        {
            if (SessionService.Instance.AllSessionsCount() == 0)
            {
                string message = $"{SessionService.Instance.AllSessionsCount()} {LangHelper.GetString("sessions")}. {LangHelper.GetString("LoadOrImportSessions")}";

                string caption = $"{LangHelper.GetString("Office 365 Fiddler Extension")}";

                MessageBoxButtons buttons = MessageBoxButtons.OK;

                //Display the MessageBox.
                MessageBox.Show(message, caption, buttons, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                return;
            }

            // Tuple -- checkAllSessionsAreAnalysed (bool), not analysed count (int).
            Tuple<bool, int> checkAllSessionsAreAnalysed = SessionFlagService.Instance.CheckAllSessionsAreAnalysed();

            // All sessions have analysis, proceed to create the CAR.
            if (!checkAllSessionsAreAnalysed.Item1)
            {
                string message = $"{checkAllSessionsAreAnalysed.Item2} of {SessionService.Instance.AllSessionsCount()} {LangHelper.GetString("ConsolidatedAnalysisReportNotAllSessionsHaveAnalysis")}";

                string caption = $"{LangHelper.GetString("Office 365 Fiddler Extension")}";

                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                //Display the MessageBox.
                result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Yes)
                {
                    SessionFlagService.Instance.AnalyseAllSessions();
                }
                else
                {
                    return;
                }
            }

            // Create a HTML report summarising findings from all sessions loaded in Fiddler. DONE.
            // Prompt for location to save HTML file in or just open it.
            // Record the date/time the report was created. DONE.
            // Record the date/time of the first and last session in the trace. DONE.
            // Determine the processes traffic has been collected from and the percentages of each process. DONE.
            // Determine percentage of sessions that are connect tunnels. TLS version. Highlight a high percentage as no decryption set. DONE.
            // Determine percentage of sessions that are 401 Auth Challenges. Highlight a high percentage as a potential auth issue. DONE.
            // Determine percentage of sessions with severity of 60. DONE.

            ResultsString = new StringBuilder();
            InterestingSessions = new StringBuilder();
            int interestingSessionsCount = 0;

            ResultsString.AppendLine(" <html>");
            ResultsString.AppendLine("<body>");
            ResultsString.AppendLine($"<h1>{LangHelper.GetString("Office365 Fiddler Extension")} - {LangHelper.GetString("Consolidated Analysis Report")} - {DateTime.Now:dddd, MMM dd yyyy}</h1>");

            Dictionary<string, int> sessionProcesses = new Dictionary<string, int>();
            Dictionary<string, int> tlsVersions = new Dictionary<string, int>();

            var Sessions = FiddlerApplication.UI.GetAllSessions();

            int connectTunnelCount = 0;
            int http401 = 0;

            // First and last session to collect data collection date/times.
            ResultsString.AppendLine($"{LangHelper.GetString("Analysed")} {Sessions.Count()} {LangHelper.GetString("sessions in trace from")} " +
                $"{SessionFlagService.Instance.GetDeserializedSessionFlags(Sessions.Last()).DateDataCollected}");

            InterestingSessions.AppendLine($"<h2>{LangHelper.GetString("Interesting Sessions")}</h2>");

            foreach (var Session in Sessions)
            {
                this.session = Session;

                var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

                ////////////////////////////////////
                ///
                /// Session severity 60 sessions.

                if (ExtensionSessionFlags.SessionSeverity == 60)
                {
                    interestingSessionsCount++;
                    InterestingSessions.AppendLine($"<h3>{LangHelper.GetString("Session")} {this.session.id}</h3>");
                    InterestingSessions.AppendLine($"<p>{ExtensionSessionFlags.ResponseAlert}</p>");
                    InterestingSessions.AppendLine($"<p>{ExtensionSessionFlags.ResponseComments}</p>");
                    InterestingSessions.AppendLine("<hr />");
                }

                ////////////////////////////////////
                ///
                /// Connect Tunnels.

                if (this.session.isTunnel)
                {
                    connectTunnelCount++;

                    try
                    {
                        tlsVersions.Add(ExtensionSessionFlags.TLSVersion, 1);
                    }
                    // Use the exception, already exists, to increment the value for the proess name.
                    catch (Exception)
                    {
                        int oldvalue = tlsVersions[ExtensionSessionFlags.TLSVersion];
                        tlsVersions[ExtensionSessionFlags.TLSVersion] = oldvalue + 1;
                    }
                }

                ////////////////////////////////////
                ///
                /// Process Names

                // Attempt to add the process name to the dictionary.
                try
                {
                    sessionProcesses.Add(ExtensionSessionFlags.ProcessName, 1);
                }
                // Use the exception, already exists, to increment the value for the proess name.
                catch (Exception)
                {
                    int oldvalue = sessionProcesses[ExtensionSessionFlags.ProcessName];
                    sessionProcesses[ExtensionSessionFlags.ProcessName] = oldvalue + 1;
                }

                ////////////////////////////////////
                ///
                /// HTTP 401 Authentication Challenges.

                if(this.session.responseCode == 401)
                {
                    http401++;
                }

            }

            if (interestingSessionsCount > 0)
            {
                ResultsString.Append(InterestingSessions);
            }

            ResultsString.AppendLine($"<h2>{LangHelper.GetString("Processes")}</h2>");

            ResultsString.AppendLine("<table>");
            // Iterate through the processName hashtable, calculate percentages, and output.
            foreach (KeyValuePair<string, int> processkvp in sessionProcesses)
            {
                // Cast to double, and cast an input to double to break out of int rounding down, and giving a zero result.
                double processresult = (double)processkvp.Value / SessionService.Instance.AllSessionsCount() * 100;
                ResultsString.AppendLine($"<tr><td>{processkvp.Key}</td><td>{Math.Round(processresult, 2)}%</td></tr>");
            }
            ResultsString.AppendLine("</table>");

            ResultsString.AppendLine("<h2>Connect Tunnels - TLS</h2>");

            ResultsString.AppendLine($"<p>There are {connectTunnelCount} connect tunnel sessions in this trace.</p>");

            ResultsString.AppendLine("<table>");
            ResultsString.AppendLine("<tr><th>TLS version</th><th>Percentage of sessions</th></tr>");

            foreach (KeyValuePair<string, int> tlskvp in tlsVersions)
            {
                // Cast to double, and cast an input to double to break out of int rounding down, and giving a zero result.
                double tlsresult = (double)tlskvp.Value / connectTunnelCount *100;
                ResultsString.AppendLine($"<tr><td>{tlskvp.Key}</td><td>{Math.Round(tlsresult, 2)}%</td></tr>");
            }

            ResultsString.AppendLine("</table>");

            double percentageConnectTunnels = (double)connectTunnelCount / SessionService.Instance.AllSessionsCount() * 100;

            if (percentageConnectTunnels >= 80)
            {
                ResultsString.AppendLine($"<p><span style='color=red'>There's a high percentage of sessions which are connect tunnels ({percentageConnectTunnels}%. " +
                    "It's likely decryption wasn't enabled in Fiddler when this trace was collected.</span></p>");
            }

            // Cast to double, and cast an input to double to break out of int rounding down, and giving a zero result.
            double http401count = (double)http401 / SessionService.Instance.AllSessionsCount() * 100;

            if (http401count > 50)
            {
                ResultsString.AppendLine("More than 50% of sessions are HTTP 401 Authentication Challenge. There may be an authentication issue in this trace.");
            }

            ResultsString.AppendLine("</body>");
            ResultsString.AppendLine("</html>");

            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "Web Page|*.html",
                Title = "Save the report",
                FileName = $"Consolidated Analysis Report {DateTime.Now:dd-MMM-yyyy H.mm.ss tt}.html"
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter outputFile = new StreamWriter(saveFileDialog1.FileName))
                {
                        outputFile.Write(ResultsString);
                }
            }
        }
    }
}
