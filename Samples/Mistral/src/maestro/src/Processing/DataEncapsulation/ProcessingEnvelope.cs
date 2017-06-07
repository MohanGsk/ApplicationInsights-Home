using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.ApplicationInsights.Ingestion.Maestro
{
    public class ProcessingEnvelope
    {
        public Contracts.Envelope data;
        public ProcessingError error;
    }
}