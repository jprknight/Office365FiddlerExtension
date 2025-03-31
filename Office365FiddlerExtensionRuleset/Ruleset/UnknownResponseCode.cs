using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using Office365FiddlerExtensionRuleset.Services;
using System.Diagnostics;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class UnknownResponseCode
    {

        internal Session session { get; set; }

        private static UnknownResponseCode _instance;

        public static UnknownResponseCode Instance => _instance ?? (_instance = new UnknownResponseCode());

        /// <summary>
        /// Set session flags for any session which has an unknown / undefined response code in the extension.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            // Not setting colours on sessions not recognised.

            var sw = Stopwatch.StartNew();

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = RulesetLangHelper.GetString("Undefined"),

                SessionType = RulesetLangHelper.GetString("Undefined"),
                ResponseCodeDescription = RulesetLangHelper.GetString("Undefined"),
                ResponseAlert = RulesetLangHelper.GetString("Undefined"),
                ResponseComments = RulesetLangHelper.GetString("Response Comments No Known Issue"),

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0,
                SessionSeverity = 10
            };

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Updating session flags.");
            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);

            sw.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_UnknownResponseCode");
                TelemetryService.CustomTrackMetric("RS_UnknownResponseCode", sw.ElapsedMilliseconds);
            }
        }
    }
}
