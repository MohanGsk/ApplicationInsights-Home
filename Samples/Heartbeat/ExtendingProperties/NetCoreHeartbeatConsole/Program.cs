using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace NetCoreHeartbeatConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Heartbeat extender. Press Esc to stop.");

            var appInsightsConfig = TelemetryConfiguration.Active;
            var testingChannel = new SenderChannel();
            appInsightsConfig.TelemetryChannel = testingChannel;

            var tc = new TelemetryClient(appInsightsConfig);
            var extender = new CustomHeartbeatProperties();

            // wait for the user to hit the Esc key
            int foundHeartbeats = 0;
            if (extender.Initialize(TimeSpan.FromSeconds(31.0)))
            {
                while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
                {
                    Thread.Sleep(100);
                    if (ReportHeartbeatsFound(testingChannel))
                    {
                        Console.WriteLine("Found {0} heartbeats.", foundHeartbeats);
                        if (foundHeartbeats == 0)
                        {
                            Console.WriteLine("Adding a custom property.");
                            extender.AddCustomHeartbeatProperty();
                        }
                        else if (foundHeartbeats == 1)
                        {
                            Console.WriteLine("Updating the custom property we added previously.");
                            extender.UpdateCustomHeartbeatProperty();
                        }
                        foundHeartbeats++;
                    }
                }

                Console.WriteLine("You pressed Esc. See ya!");
            }
            else
            {
                Console.WriteLine("Could not initialize. Please step through with a debugger to see what may have caused the problem.");
            }
        }

        static bool ReportHeartbeatsFound(SenderChannel telemetryChannel)
        {
            bool foundAHeartbeat = false;
            IEnumerable<ITelemetry> metricsTelemetry = telemetryChannel.GetTelemetryTypeSent("MetricTelemetry");
            if (metricsTelemetry != null)
            {
                foreach (ITelemetry ti in metricsTelemetry)
                {
                    if (ti is MetricTelemetry metric)
                    {
                        metric.Name.Equals("HeartbeatState", StringComparison.OrdinalIgnoreCase);
                        Console.WriteLine($"Heartbeat sent at {metric.Timestamp.ToString("h:mm:ss")}");
                        Console.WriteLine($" Properties:");
                        foreach (var kvp in metric.Properties)
                        {
                            Console.WriteLine($"   {kvp.Key}: '{kvp.Value}'");
                        }
                        foundAHeartbeat = true;
                    }
                }
            }

            return foundAHeartbeat;
        }
    }
}
