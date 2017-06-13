using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.ApplicationInsights.Ingestion.Maestro
{
    /** Data Client implementing the API surface of the profile service */
    public class GraphDataClient: BaseDataClient
    {
        private IDataBroker<HttpResource, HttpBrokerResponse> dataBroker;

        public GraphDataClient(IDataBroker<HttpResource, HttpBrokerResponse> dataBroker)
        {
            this.dataBroker = dataBroker;
        }

        public async Task<Contracts.Profile> GetProfile(string iKey)
        {
            // This would normally call out to the dataBroker
            // The service we need to call to isn't ready yet in Mistral - so there's no need to use the dataBroker
            // Make a fake profile instead
            if (iKey.Length != 36)
            {
                return null;
            }

            return await this.CacheWrap("profile_" + iKey, async () => 
            {
                await Task.Delay(1500); // Simulate network call to demonstrate cache layer

                return new Contracts.Profile
                {
                    AppId = "appid" + iKey.Substring(8),
                    InstrumentationKey = iKey
                };
            });
        }
    }
}