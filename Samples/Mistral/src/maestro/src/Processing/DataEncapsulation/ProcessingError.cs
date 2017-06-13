using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.ApplicationInsights.Ingestion.Maestro
{
    public class ProcessingError
    {
        private int code;
        private string msgOverride;

        public ProcessingError(int status, string message = null)
        {
            code = status;
            msgOverride = message;
        }

        public int StatusCode
        {
            get
            {
                return code;
            }
        }

        public string Message
        {
            get
            {
                if (msgOverride != null)
                {
                    return msgOverride;
                }

                switch(code)
                {
                    case StatusCodes.Status402PaymentRequired:
                        return "Monthly Quota Exceeded";
                    case StatusCodes.Status400BadRequest:
                        return "Invalid JSON or Instrumentation Key";
                    case 429:
                        return "Throttle";
                    case StatusCodes.Status500InternalServerError:
                        return "Processing Error";
                    default:
                        return "Unknown Processing Error";
                }
            }
        }
    }
}