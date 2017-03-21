using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System.Collections.Concurrent;

namespace ApplicationInsightsDataROI
{
    class DependencyFilteringWithMetricsTelemetryProcessor : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor _next;
        private readonly ConcurrentDictionary<string, Tuple<Metric, Metric>> _metrics = new ConcurrentDictionary<string, Tuple<Metric, Metric>>();
        private readonly MetricManager _manager;

        public DependencyFilteringWithMetricsTelemetryProcessor(ITelemetryProcessor next, TelemetryConfiguration configuraiton)
        {
            _next = next;
            _manager = new MetricManager(new TelemetryClient(configuraiton));
        }

        public void Process(ITelemetry item)
        {
            // check telemetry type
            if (item is DependencyTelemetry)
            {
                var d = item as DependencyTelemetry;

                // increment counters
                var metrics = _metrics.GetOrAdd(d.Type, (type) =>
                    {
                        var numberOfDependencies = _manager.CreateMetric("# of dependencies", new Dictionary<string, string> { { "type", type } });
                        var dependenciesDuration = _manager.CreateMetric("dependencies duration (ms)", new Dictionary<string, string> { { "type", type } });
                        return new Tuple<Metric, Metric>(numberOfDependencies, dependenciesDuration);
                    });

                metrics.Item1.Track(1);
                metrics.Item2.Track(d.Duration.TotalMilliseconds);

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
