using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.ApplicationInsights.Ingestion.Maestro
{
    /** 
     * Common interface for a Data Broker - an object that encapsulates a transmission layer (such as HTTP)
     * Users should get a concrete implementation of IDataBroker<ResourceType, ResponseType> from DI container
     */
    public interface IDataBroker<T, R> 
    {
        Task<R> Query(T resource);
        Task<R> Submit(T resource, object payload);
    }

    public struct HttpResource
    {
        public Uri Uri;
        public Dictionary<string, string> Headers;

        public HttpResource(string uri, Dictionary<string, string> headers = null) {
            this.Uri = new Uri(uri);
            this.Headers = headers;
        }
    }
}