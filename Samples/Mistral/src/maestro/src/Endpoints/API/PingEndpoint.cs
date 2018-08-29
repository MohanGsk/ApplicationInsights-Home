using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.ApplicationInsights.Ingestion.Maestro
{
    /** Endpoint that validates the service is running */
    public class PingEndpoint: BaseEndpoint 
    {

        public override async Task HandleGET(HttpContext context) 
        {
            await Respond(context, StatusCodes.Status200OK, "alive");
        }

        public override async Task HandlePOST(HttpContext context) 
        {
            await Respond(context, StatusCodes.Status200OK, "alive");
        }
    }
}