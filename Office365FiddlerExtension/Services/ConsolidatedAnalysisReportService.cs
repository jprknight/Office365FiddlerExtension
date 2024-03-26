using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtension.Services
{
    class ConsolidatedAnalysisReportService
    {

        internal Session session { get; set; }

        private static ConsolidatedAnalysisReportService _instance;

        public static ConsolidatedAnalysisReportService Instance => _instance ?? (_instance = new ConsolidatedAnalysisReportService());

        public void SetProcessName(ConsolidatedAnalysisReportService session)
        {
            var Sessions = FiddlerApplication.UI.GetSelectedSessions();

            int connectTunnelCount = 0;

            // First and last session to collect data collection date/times.

            Session firstSession = Sessions.First();
            Session lastSession = Sessions.Last();

            var firstSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(firstSession);
            var lastSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(lastSession);

            string test1 = firstSessionFlags.DateDataCollected;
            string test2 = lastSessionFlags.DateDataCollected;

            int sessionsCount = Sessions.Count();

            foreach (var Session in Sessions)
            {
                this.session = Session;

                if (this.session.isTunnel)
                {
                    connectTunnelCount++;
                }

            }

            int percentageConnectTunnels = connectTunnelCount / Sessions.Count() * 100;

        }
    }
}
