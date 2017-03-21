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
    class Demo2back
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

            //// initialize price calculation logic
            var state = new _State();
            state.Initialize();

            // enable sampling
            configuration.TelemetryProcessorChainBuilder
                // this telemetry processor will be executed first for all telemetry items to calculate the size and # of items
                .Use((next) => { return new PriceCalculatorTelemetryProcessor(next, state.Collected); })

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
                .Use((next) => { return new PriceCalculatorTelemetryProcessor(next, state.Sent); })
                .Build();


            TelemetryClient client = new TelemetryClient(configuration);

            var iterations = 0;


            while (!state.IsTerminated)
            {

                iterations++;

                using (var operaiton = client.StartOperation<RequestTelemetry>("Process item"))
                {
                    client.TrackEvent("test");
                    client.TrackTrace("Something happened", SeverityLevel.Information);

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

                    //client.StopOperation(operaiton);
                    //Console.WriteLine($"Iteration {iterations}. Elapesed time: {operaiton.Telemetry.Duration}");

                }
            }

            Console.WriteLine($"Program sent 100K of telemetry in {iterations} iterations!");
            Console.ReadLine();
        }
    }
}
