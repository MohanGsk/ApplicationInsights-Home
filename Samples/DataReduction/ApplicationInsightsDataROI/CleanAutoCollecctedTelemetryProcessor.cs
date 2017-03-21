using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace ApplicationInsightsDataROI
{
    class CleanAutoCollecctedTelemetryProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor _next;

        public CleanAutoCollecctedTelemetryProcessor(ITelemetryProcessor next)
        {
            this._next = next;
        }

        public void Process(ITelemetry item)
        {
            if (item is TraceTelemetry)
            {
                // no need to correlate high-volume traces with other telemetry:
                item.Context.Operation.Name = "";
                item.Context.User.Id = "";
                item.Context.Session.Id = "";
            }

            this._next.Process(item);
        }
    }
}
