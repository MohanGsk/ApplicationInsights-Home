using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.ApplicationInsights.Ingestion.Maestro
{
    /** Contains routing extension methods for Maestro */
    public static class RoutingExtensions
    {
        /** Helper to add endpoints to routes while allowing for dependency injection */
        public static void AddEndpoint<T>(this RouteBuilder routeBuilder, string path) where T: BaseEndpoint
        {
            T endpoint = (T) routeBuilder.ApplicationBuilder.ApplicationServices.GetService(typeof(T));
            routeBuilder.MapGet(path, endpoint.GET);
            routeBuilder.MapPost(path, endpoint.POST);
        }
    }
}