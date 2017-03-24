using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;

namespace ApplicationInsightsDataROI
{
    class DependencyFilteringTelemetryProcessor : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor _next;

        public DependencyFilteringTelemetryProcessor(ITelemetryProcessor next)
        {
            _next = next;
        }

        public void Process(ITelemetry item)
        {
            // check telemetry type
            if (item is DependencyTelemetry)
            {
                var d = item as DependencyTelemetry;
                if (d.Duration < TimeSpan.FromMilliseconds(100))
                {
                    // if dependency duration > 100 msec then stop telemetry  
                    // processing and return from the pipeline
                    return;
                }
            }

            this._next.Process(item);
        }
    }
}
