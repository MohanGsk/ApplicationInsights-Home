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
    class Program_backup
    {

        static void Main_backup(string[] args)
        {
            var state = new _State();
            state.Initialize();

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            configuration.InstrumentationKey = "fb8a0b03-235a-4b52-b491-307e9fd6b209";

            var telemetryChannel = new ServerTelemetryChannel();
            telemetryChannel.Initialize(configuration);
            configuration.TelemetryChannel = telemetryChannel;
            

            // data collection modules
            var dependencies = new DependencyTrackingTelemetryModule();
            dependencies.Initialize(configuration);

            // telemetry initializers
            configuration.TelemetryInitializers.Add(new AppVersionTelemetryInitializer());
            configuration.TelemetryInitializers.Add(new DefaultTelemetryInitializer());
            configuration.TelemetryInitializers.Add(new BusinessTelemetryInitializer());
            configuration.TelemetryInitializers.Add(new NodeNameTelemetryInitializer());
            configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());

            // telemetry processors
            configuration.TelemetryProcessorChainBuilder
                .Use((next) => { return new PriceCalculatorTelemetryProcessor(next, state.Collected); })
//                .Use((next) => { return new MyTelemetryProcessor(next); })
//                .Use((next) => { return new CleanAutoCollecctedTelemetryProcessor(next); })
//                .Use((next) => { return new ExampleTelemetryProcessor(next); })
                // send only 10% of all dependency data
                .Use((next) =>
                {
                    return new SamplingTelemetryProcessor(next)
                    {
                        IncludedTypes = "Dependency",
                        SamplingPercentage = 10
                    };
                })
                // start sampling when load exceeds 2 events per second for all telemetry types except events
                .Use((next) =>
                {
                    return new AdaptiveSamplingTelemetryProcessor(next)
                    {
                        ExcludedTypes = "Event",
                        MaxTelemetryItemsPerSecond = 2,
                        SamplingPercentageIncreaseTimeout = TimeSpan.FromSeconds(1),
                        SamplingPercentageDecreaseTimeout = TimeSpan.FromSeconds(1),
                        EvaluationInterval = TimeSpan.FromSeconds(1),
                        InitialSamplingPercentage = 25
                    };
                })
                .Use((next) => { return new PriceCalculatorTelemetryProcessor(next, state.Sent); })
                .Build();

            TelemetryClient client = new TelemetryClient(configuration);

            var iterations = 0;


            MetricManager metricManager = new MetricManager(client);
            var itemsProcessed = metricManager.CreateMetric("Items processed");
            var processingFailed = metricManager.CreateMetric("Failed processing");
            var processingSize = metricManager.CreateMetric("Processing size");
            

            while (!state.IsTerminated)
            {

                iterations++;

                using (var operaiton = client.StartOperation<RequestTelemetry>("Process item"))
                {
                    client.TrackEvent("test");
                    client.TrackTrace("Something happened");

                    try
                    {
                        HttpClient http = new HttpClient();
                        var task = http.GetStringAsync("http://bing.com");
                        task.Wait();

                        // metrics aggregation:
                        //itemsProcessed.Track(1);
                        //processingSize.Track(task.Result.Length);
                        //processingFailed.Track(0);

                        //client.TrackMetric("Response size", task.Result.Length);
                        //client.TrackMetric("Successful responses", 1);
                    }
                    catch (Exception exc)
                    {
                        //client.TrackMetric("Successful responses", 0);
                        //operaiton.Telemetry.Success = false;

                        // metrics aggregation:
                        //processingFailed.Track(1);
                    }

                }
            }

            Console.WriteLine($"Program sent 1Mb of telemetry in {iterations} iterations!");
            Console.ReadLine();
        }
    }
}
