using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;

namespace ApplicationInsightsDataROI
{
    class DependencyExampleTelemetryProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor _next;

        public DependencyExampleTelemetryProcessor(ITelemetryProcessor next)
        {
            this._next = next;
        }

        public void Process(ITelemetry item)
        {
            // check telemetry type
            if (item is DependencyTelemetry)
            {
                var r = item as DependencyTelemetry;
                if (r.Duration > TimeSpan.FromMilliseconds(100))
                {
                    // if dependency duration > 100 msec then "sample in" 
                    // this telemetry by setting sampling percentage to 100 
                    ((ISupportSampling)item).SamplingPercentage = 100;
                }
            }
            // continue with the next telemetry processor
            this._next.Process(item);
        }
    }
}
