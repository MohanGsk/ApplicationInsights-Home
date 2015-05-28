using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace MvcWebRole.Telemetry
{
    public class AiWebApiExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            if (context != null && context.Exception != null)
            {
                /*
                 * Please note: You do not need to construct a new TelemetryClient every time. 
                 * Indeed, we recommend that you reuse a client instance.
                 * You only need to create separate instances, when you need to initialize with different configuration
                 * The default constructor without parameters, uses TelemetryConfiguration.Active
                 */
                var ai = new TelemetryClient();
                ai.TrackException(context.Exception);
            }
            base.Log(context);
        }
    }
}