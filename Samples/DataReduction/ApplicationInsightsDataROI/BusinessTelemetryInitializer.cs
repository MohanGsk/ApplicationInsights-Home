using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace ApplicationInsightsDataROI
{
    class BusinessTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry is EventTelemetry)
            {
                telemetry.Context.InstrumentationKey = "BUSINESS_TELEMETRY_KEY";
            }
        }
    }
}
