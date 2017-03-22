using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

namespace ApplicationInsightsDataROI
{
    class NodeNameTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.GetInternalContext().NodeName = "MyNode";
        }
    }
}
