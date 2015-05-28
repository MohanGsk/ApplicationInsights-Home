using System.Runtime.Remoting.Messaging;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace WorkerRoleB.Telemetry
{
    class ItemCorrelationTelemetryInitializer : ITelemetryInitializer
    {

        private static string CORRELATION_SLOT = "CORRELATION-ID";
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Operation.Id = (string)CallContext.LogicalGetData(CORRELATION_SLOT);
            /*var nds = Thread.GetNamedDataSlot(CORRELATION_SLOT);
            if (null != nds && null != Thread.GetData(nds))
            {
                string cid = (string)Thread.GetData(nds);
                telemetry.Context.Operation.Id = cid;                
            } */           
        }
    }
}
