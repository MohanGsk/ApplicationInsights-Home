using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;


namespace ApplicationInsightsDataROI
{
    class Demo1
    {
        public static void Run()
        {
            // set iKey
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            configuration.InstrumentationKey = "fb8a0b03-235a-4b52-b491-307e9fd6b209";

            // automatically collect dependency calls
            var dependencies = new DependencyTrackingTelemetryModule();
            dependencies.Initialize(configuration);

            // automatically correlate all telemetry data with request
            configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());

            TelemetryClient client = new TelemetryClient(configuration);

            var iteration = 0;

            while (true)
            {
                using (var operaiton = client.StartOperation<RequestTelemetry>("Process item"))
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
                        operaiton.Telemetry.Success = false;
                    }

                    client.StopOperation(operaiton);
                    Console.WriteLine($"Iteration {iteration}. Elapesed time: {operaiton.Telemetry.Duration}");
                    iteration++;
                }
            }
        }
    }

}
