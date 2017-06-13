using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.ApplicationInsights.Ingestion.Maestro
{
    /** Main entry point for Maestro */
    public class Server
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Server>()
                .Build();
            host.Run();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();

            // Register data brokers
            services.AddSingleton<IDataBroker<HttpResource, HttpBrokerResponse>, HttpDataBroker>();

            // Register data clients
            services.AddSingleton<GraphDataClient, GraphDataClient>();

            // Register endpoints
            services.AddSingleton<FallbackEndpoint>();
            services.AddSingleton<PingEndpoint>();
            services.AddSingleton<ProfileEndpoint>();
            services.AddSingleton<ValidateEndpoint>();
        }

        public void Configure(IApplicationBuilder app, GraphDataClient graphDataClient)
        {
            // Prepare data caches
            LRUInMemoryCache memoryCache = new LRUInMemoryCache(Configuration.MAX_LRU_CACHE_SIZE);

            // Configure data clients
            graphDataClient.RegisterCache(memoryCache, DataCacheTier.One);

            // Configure routes
            RouteBuilder routes = new RouteBuilder(app);

            routes.AddEndpoint<ValidateEndpoint>("v2/validate");
            routes.AddEndpoint<PingEndpoint>("api/ping");
            routes.AddEndpoint<ProfileEndpoint>("api/profiles/{iKey}/{profileKey?}");
            routes.AddEndpoint<FallbackEndpoint>("");

            app.UseRouter(routes.Build());
        }
    }
}
