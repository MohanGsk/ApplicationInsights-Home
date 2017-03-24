using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Diagnostics;
using System.Security.Principal;

namespace ApplicationInsightsDataROI
{
    class DefaultTelemetryInitializer : ITelemetryInitializer
    {
        string sessionId = Guid.NewGuid().ToString();
        Process currentProcess = Process.GetCurrentProcess();
        string userid = WindowsIdentity.GetCurrent().Name;

        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Component.Version = "1.2.3";

            telemetry.Context.Cloud.RoleName = currentProcess.ProcessName;

            telemetry.Context.User.Id = userid;

            telemetry.Context.Session.Id = sessionId;
        }
    }
}
