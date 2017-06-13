using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace Microsoft.ApplicationInsights.Ingestion.Maestro
{
    /** Common base class of all endpoint controllers. Supplies helper methods for GET/POST processing and returning valid responses. */
    public abstract class BaseEndpoint
    {
        public virtual async Task HandleGET(HttpContext context) 
        {
            await Respond(context, StatusCodes.Status405MethodNotAllowed, "Method Not Allowed");
        }

        public async Task GET(HttpContext context)
        {
            try 
            {
                await HandleGET(context);
            }
            catch (Exception e) 
            {
                await ErrorHandler(context, e);
            }
        }

        public async Task POST(HttpContext context)
        {
            try 
            {
                await HandlePOST(context);
            }
            catch (Exception e) 
            {
                await ErrorHandler(context, e);
            }
        }

        public virtual async Task HandlePOST(HttpContext context) 
        {
            await Respond(context, StatusCodes.Status405MethodNotAllowed, "Method Not Allowed");
        }

        protected async Task Respond(HttpContext context, int responseCode, object responseObj) 
        {
            context.Response.StatusCode = responseCode;
            string responseText;
            if (responseObj is string)
            {
                responseText = (string)responseObj;
            }
            else
            {
                responseText = JsonConvert.SerializeObject(responseObj);
            }

            if (Configuration.ENABLE_REQUEST_LOGGING)
            {
                Console.WriteLine(String.Format("-----\nIncoming Request: {0}\nResponse:\n{1}\n-----", context.Request.Path, responseText));
            }
             
            context.Response.Headers.Add("Content-Length", responseText.Length.ToString());
            await context.Response.WriteAsync(responseText);
        }

        protected async Task Respond(HttpContext context, Contracts.SDKResponse response)
        {
            int statusCode = StatusCodes.Status200OK;

            if (response.itemsAccepted != response.itemsReceived && response.itemsAccepted > 0)
            {
                statusCode = StatusCodes.Status206PartialContent;
            }
            else if (response.itemsAccepted != response.itemsReceived && response.itemsReceived > 0)
            {
                statusCode = response.errors[0].statusCode;
            }

            await Respond(context, statusCode, response);
        }

        private async Task ErrorHandler(HttpContext context, Exception e)
        {
            string message = "Internal Server Error";

            if (Configuration.ENABLE_ERROR_TRACES) 
            {
                message = message + "\n\n" + e.ToString();
            }

            await Respond(context, StatusCodes.Status500InternalServerError, message);
        }
    }

    public static class BaseEndpointExtensions
    {
        public static string GetRouteParameter(this HttpContext context, string paramName) 
        {
            return (string)context.GetRouteData().Values[paramName];
        }
    }
}