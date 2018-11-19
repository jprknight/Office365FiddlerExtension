using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using System.Management;
using System.Diagnostics;
using Microsoft.ApplicationInsights.Extensibility;
using Fiddler;

namespace EXOFiddlerInspector.Telemetry
{
    public sealed class Client : ITelemetry
    {
        private TelemetryClient telemetryClient { get; set; }

        private static readonly string iKey = "f236d9b9-fc00-461d-a9c6-64f231416908";

        private string UUID { get; set; }
        internal bool IsInitialized { get; set; } = false;

        public Task Initialize()
        {
            if (!IsInitialized)
            {
                try
                {
                    telemetryClient = new TelemetryClient();
                    telemetryClient.InstrumentationKey = iKey;
                    UUID = GetComputerUUID();
                    IsInitialized = true;

                }
                catch
                {

                }
            }
            return Task.CompletedTask;
        }

        public async Task TrackEvent(string EventName, string UserId = null)
        {
            try
            {
                telemetryClient.Context.User.Id = UUID;
                telemetryClient.Context.Session.Id = Guid.NewGuid().ToString();
                telemetryClient.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
                telemetryClient.TrackEvent(EventName);

                //telemetryClient.TrackEvent("UserSession");

                telemetryClient.Flush();
                // Allow time for flushing:
                await Task.Delay(1500);
            }
            catch
            {

            }
            return;
        }
        private string GetComputerUUID()
        {
#if DEBUG
            UUID = "dev1";         
            return UUID;
#else
            UUID = GetComputerUUID();
#endif
            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            ManagementObjectCollection _managementObjects = query.Get();
          
            foreach (ManagementObject mgmtObject in _managementObjects)
            {
                UUID = (string)mgmtObject["SerialNumber"];
            }
            return UUID;
        }

    }
}
