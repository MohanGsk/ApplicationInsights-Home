using MvcWebRole.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace MvcWebRole
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            // report unhandled exceptions from WebAPI 2 controllers to AI
            config.Services.Add(typeof(IExceptionLogger), new AiWebApiExceptionLogger()); 
        }
    }
}
