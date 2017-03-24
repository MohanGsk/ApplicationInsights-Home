using System;

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

            Console.ReadKey();
        }

    }
}
