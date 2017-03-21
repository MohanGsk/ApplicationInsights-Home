using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
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
    class Demo6
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

            QuickPulseTelemetryProcessor processor = null;

            // enable Live Metrics
            configuration.TelemetryProcessorChainBuilder

                //adding LiveMetrics telemetry processor
                .Use((next) =>
                {
                    processor = new QuickPulseTelemetryProcessor(next);
                    return processor;
                })

                .Build();

            var QuickPulse = new QuickPulseTelemetryModule();
            QuickPulse.Initialize(configuration);
            QuickPulse.RegisterTelemetryProcessor(processor);

            TelemetryClient client = new TelemetryClient(configuration);

            var iterations = 0;
            var rnd = new Random();
            
            while (true)
            {

                iterations++;

                using (var operaiton = client.StartOperation<RequestTelemetry>("Process item"))
                {
                    client.TrackEvent("test", new Dictionary<string, string>() { { "iteration", iterations.ToString() } });
                    client.TrackTrace($"Iteration {iterations} happened", SeverityLevel.Information);

                    var status = rnd.Next() < rnd.Next();
                    try
                    {
                        if (status)
                        {
                            throw (new Exception($"Failure during processing of iteration #{iterations}"));
                        };
                        HttpClient http = new HttpClient();
                        var task = http.GetStringAsync("http://bing.com");
                        task.Wait();

                    }
                    catch (Exception exc)
                    {
                        client.TrackException(exc);
                    }
                    finally
                    {
                        client.StopOperation<RequestTelemetry>(operaiton);
                        operaiton.Telemetry.Success = status;

                        Console.WriteLine($"Iteration {iterations}. Elapesed time: {operaiton.Telemetry.Duration}. Success: {operaiton.Telemetry.Success}");
                    }
                }

            }
        }
    }
}
