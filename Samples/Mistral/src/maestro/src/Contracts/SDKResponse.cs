using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.ApplicationInsights.Ingestion.Maestro.Contracts
{
    public class SDKResponse
    {
        public int itemsReceived;
        public int itemsAccepted;
        public SDKResponseError[] errors;
    }

    public class SDKResponseError
    {
        public int index;
        public int statusCode;
        public string message;
    }
}