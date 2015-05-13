using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace MvcWebRole.Telemetry
{
    /*
     * Custom TelemetryInitializer that overrides the default AI SDK 
     * behavior of treating response codes >= 400 as failed requests
     * 
     */
    public class MyTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            var requestTelemetry = telemetry as RequestTelemetry;
            if (requestTelemetry == null) return;
            if (requestTelemetry.ResponseCode != "401") return;
            requestTelemetry.Success = true;
            requestTelemetry.Context.Properties["Overridden401"] = "true";
        }
    }
}