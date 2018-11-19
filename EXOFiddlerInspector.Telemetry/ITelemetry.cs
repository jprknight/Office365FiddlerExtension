using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXOFiddlerInspector.Telemetry
{
    public interface ITelemetry
    {
        Task Initialize();

        Task TrackEvent(string EventName, string UserId = null);
    }
}
