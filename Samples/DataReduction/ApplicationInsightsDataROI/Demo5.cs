using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;


namespace ApplicationInsightsDataROI
{
    class Demo5
    {
        public static void Run()
        {

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            configuration.InstrumentationKey = "fb8a0b03-235a-4b52-b491-307e9fd6b209";

            // automatically track dependency calls
            var dependencies = new DependencyTrackingTelemetryModule();
            dependencies.Initialize(configuration);

            // automatically correlate all telemetry data with request
            configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());

            // initialize state for the telemetry size calculation
            ProcessedItems CollectedItems = new ProcessedItems();
            ProcessedItems SentItems = new ProcessedItems();

            // build telemetry processing pipeline
            configuration.TelemetryProcessorChainBuilder
                // this telemetry processor will be executed first for all telemetry items to calculate the size and # of items
                .Use((next) => { return new SizeCalculatorTelemetryProcessor(next, CollectedItems); })

               // this is a standard fixed sampling processor that will let only 10% 
               .Use((next) =>
               {
                   return new SamplingTelemetryProcessor(next)
                   {
                       IncludedTypes = "Dependency",
                       SamplingPercentage = 10
                   };
               })

                // this is a standard adaptive sampling telemetry processor that will sample in/out any telemetry item it receives
                .Use((next) =>
                {

                    return new AdaptiveSamplingTelemetryProcessor(next)
                    {
                        ExcludedTypes = "Event", // exclude custom events from being sampled
                        MaxTelemetryItemsPerSecond = 1, //default: 5 calls/sec
                        SamplingPercentageIncreaseTimeout = TimeSpan.FromSeconds(1), //default: 2 min
                        SamplingPercentageDecreaseTimeout = TimeSpan.FromSeconds(1), //default: 30 sec
                        EvaluationInterval = TimeSpan.FromSeconds(1), //default: 15 sec
                        InitialSamplingPercentage = 25 //default: 100% 
                    };
                })

                // this telemetry processor will be execuyted ONLY when telemetry is sampled in
                .Use((next) => { return new SizeCalculatorTelemetryProcessor(next, SentItems); })
                .Build();

            TelemetryClient client = new TelemetryClient(configuration);

            // configure metrics collection
            MetricManager metricManager = new MetricManager(client);
            var reductionsize = metricManager.CreateMetric("Reduction Size");

            var iteration = 0;

            while (true)
            {

                iteration++;

                using (var operaiton = client.StartOperation<RequestTelemetry>("Process item"))
                {
                    client.TrackEvent("test", new Dictionary<string, string>() { { "iteration", iteration.ToString() } });
                    client.TrackTrace($"Iteration {iteration} happened", SeverityLevel.Information);

                    try
                    {
                        HttpClient http = new HttpClient();
                        var task = http.GetStringAsync("http://bing.com");
                        task.Wait();

                    }
                    catch (Exception exc)
                    {
                        client.TrackException(exc);
                        operaiton.Telemetry.Success = false;
                    }
                    client.StopOperation(operaiton);
                    Console.WriteLine($"Iteration {iteration}. Elapesed time: {operaiton.Telemetry.Duration}. Collected Telemetry: {CollectedItems.size}/{CollectedItems.count}. Sent Telemetry: {SentItems.size}/{SentItems.count}. Ratio: {1.0 * CollectedItems.size / SentItems.size}");

                    reductionsize.Track(CollectedItems.size - SentItems.size);
                    client.TrackMetric("[RAW] Reduction Size", CollectedItems.size - SentItems.size);
                }
            }
            metricManager.Flush();
        }
    }
}
