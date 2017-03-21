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
    class Program
    {

        static void Main(string[] args)
        {
            //Demo1.Run(); // default AI model with request/dependency/exception/trace and event
            //Demo2.Run(); // price calculation and fixed & adaptive sampling 
            //Demo3.Run(); //exemplification of dependencies
            Demo4.Run(); // filtering of dependencies
            //Demo5.Run(); // metrics aggregation, channeling business telemetry into a different iKey and default context settings
            //Demo6.Run(); // LiveMetrics enablement
        }

    }
}
