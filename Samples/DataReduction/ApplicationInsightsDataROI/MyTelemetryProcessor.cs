using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Channel;

namespace ApplicationInsightsDataROI
{
    class MyTelemetryProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor _next;

        public MyTelemetryProcessor(ITelemetryProcessor next)
        {
            this._next = next;
        }

        public void Process(ITelemetry item)
        {
            this._next.Process(item);
        }
    }
}
