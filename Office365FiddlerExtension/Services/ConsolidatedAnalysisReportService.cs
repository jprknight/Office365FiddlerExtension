using Fiddler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace Office365FiddlerExtension.Services
{
    class ConsolidatedAnalysisReportService
    {

        internal Session session { get; set; }

        private static ConsolidatedAnalysisReportService _instance;

        public static ConsolidatedAnalysisReportService Instance => _instance ?? (_instance = new ConsolidatedAnalysisReportService());

        public StringBuilder ResultsString { get; set; }

        /// <summary>
        /// Create Consolidation Analysis Report.
        /// </summary>
        public void CreateCAR()
        {
            // Create a HTML report summarising findings from all sessions loaded in Fiddler. DONE.
            // Prompt for location to save HTML file in or just open it.
            // Record the date/time the report was created. DONE.
            // Record the date/time of the first and last session in the trace. DONE.
            // Determine the processes traffic has been collected from and the percentages of each process. DONE.
            // Determine percentage of sessions that are connect tunnels. TLS version. Highlight a high percentage as no decryption set. DONE.
            // Determine percentage of sessions that are 401 Auth Challenges. Highlight a high percentage as a potential auth issue. DONE.
            // Determine percentage of sessions with severity of 60. DONE.

            ResultsString = new StringBuilder();

            ResultsString.AppendLine("<html>");
            ResultsString.AppendLine($"<h1>Office365 Fiddler Extension - Consolidated Analysis Report - {DateTime.Now.ToString("dddd, MMM dd yyyy, hh:mm:ss tt")}</h1>");

            Dictionary<string, int> sessionProcesses = new Dictionary<string, int>();
            Dictionary<string, int> tlsVersions = new Dictionary<string, int>();

            var Sessions = FiddlerApplication.UI.GetAllSessions();

            int connectTunnelCount = 0;
            int http401 = 0;

            // First and last session to collect data collection date/times.
            ResultsString.AppendLine($"Analysed {Sessions.Count()} sessions in trace" +
                $" from " +
                $"{SessionFlagService.Instance.GetDeserializedSessionFlags(Sessions.First()).DateDataCollected}" +
                $" to " +
                $"{SessionFlagService.Instance.GetDeserializedSessionFlags(Sessions.Last()).DateDataCollected}");

            ResultsString.AppendLine("<h2>Interesting Sessions</h2>");

            foreach (var Session in Sessions)
            {
                this.session = Session;

                var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

                ////////////////////////////////////
                ///
                /// Session severity 60 sessions.

                if (ExtensionSessionFlags.SessionSeverity == 60)
                {
                    ResultsString.AppendLine($"<h3>Session {this.session.id}</h3>");
                    ResultsString.AppendLine($"<p>{ExtensionSessionFlags.ResponseAlert}{ExtensionSessionFlags.ResponseComments}</p>");
                    ResultsString.AppendLine("<hr />");
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
                    catch (Exception e)
                    {
                        int oldvalue = tlsVersions[ExtensionSessionFlags.TLSVersion];
                        tlsVersions[ExtensionSessionFlags.TLSVersion] = oldvalue = 1;
                    }

                    // Add / update values for keys (TLS versions) in hash table.
                    // Key/value pair already exists. Update it.
                    /*
                    if (tlsVersions.ContainsKey(ExtensionSessionFlags.TLSVersion))
                    {
                        // Get a collection of the keys. 
                        ICollection tlsVersionKeys = tlsVersions.Keys;

                        // Iterate through the collection until we find the match.
                        foreach (string str in tlsVersionKeys)
                        {
                            // Increment the value for the correct key.
                            if (str == ExtensionSessionFlags.TLSVersion)
                            {
                                int value = (int)tlsVersions[str];
                                value++;
                                tlsVersions[ExtensionSessionFlags.TLSVersion] = value;
                            }
                        }
                    }
                    // Key/value pair does not exist. Add it.
                    else
                    {
                        tlsVersions.Add(ExtensionSessionFlags.TLSVersion, "1");
                    }
                    */
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
                catch (Exception e)
                {
                    int oldvalue = sessionProcesses[ExtensionSessionFlags.ProcessName];
                    sessionProcesses[ExtensionSessionFlags.ProcessName] = oldvalue = 1;
                }

                /*if (sessionProcesses.ContainsKey(ExtensionSessionFlags.ProcessName))
                {
                    // Get a collection of the keys. 
                    ICollection sessionProcessesKeys = sessionProcesses.Keys;

                    // Iterate through the collection until we find the match.
                    foreach (string str in sessionProcessesKeys)
                    {
                        // Increment the value for the correct key.
                        if (str == ExtensionSessionFlags.ProcessName)
                        {
                            sessionProcesses[str] = ((int)sessionProcesses[str]) + 1;

                            //int value = (int)sessionProcesses[str];
                            //Object value = sessionProcesses[str];
                            //value = value.ToString();
                            //value++;
                            //sessionProcesses[ExtensionSessionFlags.ProcessName] = value;
                        }
                    }
                }
                // Key/value pair does not exist. Add it.
                else
                {
                    sessionProcesses.Add(ExtensionSessionFlags.ProcessName, "1");
                }*/

                ////////////////////////////////////
                ///
                /// HTTP 401 Authentication Challenges.

                if(this.session.responseCode == 401)
                {
                    http401++;
                }

            }

            ResultsString.AppendLine("<h2>Processes</h2>");

            int percentageConnectTunnels = connectTunnelCount / Sessions.Count() * 100;

            // Iterate through the processName hashtable, calculate percentages, and output.
            foreach (KeyValuePair<string, int> kvp in sessionProcesses)
            {
                string processName = kvp.Key;

                int count = kvp.Value;

                ResultsString.AppendLine($"{processName} has {count / Sessions.Count() * 100}% of sessions in the trace.");

            }

            ResultsString.AppendLine("<h2>Connect Tunnels - TLS</h2>");

            ResultsString.AppendLine($"<p>There are {connectTunnelCount} connect tunnel sessions in this trace.</p>");

            foreach (KeyValuePair<string, int> kvp in tlsVersions)
            {
                string tlsVersion = kvp.Key;

                int count = kvp.Value;

                ResultsString.AppendLine($"TLS version {tlsVersion} is used in {count / connectTunnelCount * 100}% of the connect tunnel sessions in the trace.");

            }

            if (connectTunnelCount / Sessions.Count() * 100 >= 80)
            {
                ResultsString.AppendLine("There's a high percentage of sessions which are connect tunnels. " +
                    "It's likely decryption wasn't enabled when this trace was collected");
            }

            if (http401 / Sessions.Count() * 100 > 50)
            {
                ResultsString.AppendLine("More than 50% of sessions are HTTP 401 Authentication Challenge. There may be an authentication issue in this trace.");
            }

            ResultsString.AppendLine("</html>");

            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Web Page|*.html";
            saveFileDialog1.Title = "Save the report";
            //saveFileDialog1.ShowDialog();


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
