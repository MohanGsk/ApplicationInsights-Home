using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Channel;
using System.Security.Principal;
using System.ComponentModel;
using System.Diagnostics;

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
