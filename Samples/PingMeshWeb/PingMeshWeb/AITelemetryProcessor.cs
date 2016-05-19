using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using PingMesh; 

namespace PingMeshWeb
{
    public class PingMeshExtension : ITelemetryProcessor
    {
   
        private ITelemetryProcessor Next { get; set; }

        // You can pass values from .config
        public string MyParamFromConfigFile { get; set; }

        // Link processors to each other in a chain.
        public PingMeshExtension(ITelemetryProcessor next)
        {
            m_pingClient = new PingClient();
            this.Next = next;
        }

        public void Process(ITelemetry item)
        {
            var request = item as DependencyTelemetry;

            //for each dependency item submit the URI to PingMesh 
            if (request != null)
            {
                m_pingClient.SubmitEndpointToTargetList(request.Name);
            }

            this.Next.Process(item);
        }

        PingClient m_pingClient;
    }

}




