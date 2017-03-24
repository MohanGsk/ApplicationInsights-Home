using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;


namespace ApplicationInsightsDataROI
{
    class Demo2
    {
        public static void Run()
        {
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            configuration.InstrumentationKey = "421095c4-8c1f-4a78-a5a3-58808b5fb0c5";

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

            var iteration = 0;

            while (true)
            {
                using (var operation = client.StartOperation<RequestTelemetry>("Process item"))
                {
                    client.TrackEvent("IterationStarted", new Dictionary<string, string>() { { "iteration", iteration.ToString() } });
                    client.TrackTrace($"Iteration {iteration} started", SeverityLevel.Information);

                    try
                    {
                        var task = (new HttpClient()).GetStringAsync("http://bing.com");
                        task.Wait();

                    }
                    catch (Exception exc)
                    {
                        client.TrackException(exc);
                        operation.Telemetry.Success = false;
                    }
                    client.StopOperation(operation);
                    Console.WriteLine($"Iteration {iteration}. Elapesed time: {operation.Telemetry.Duration}. Collected Telemetry: {CollectedItems.size}/{CollectedItems.count}. Sent Telemetry: {SentItems.size}/{SentItems.count}. Ratio: {1.0 * CollectedItems.size / SentItems.size}");
                    iteration++;
                }
            }
        }
    }
}
