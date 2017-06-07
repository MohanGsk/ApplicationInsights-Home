using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.ApplicationInsights.Ingestion.Maestro
{
    /** An endpoint that validates the content of a supplied AI envelope without processing it. */
    public class ValidateEndpoint: BaseEndpoint 
    {

        public override async Task HandlePOST(HttpContext context) 
        {
            ProcessingBatch batch = new ProcessingBatch(await SDKPayloadDeserializer.Deserialize(context.Request));

            await this.Respond(context, batch.GenerateResponse());
        }
    }
}