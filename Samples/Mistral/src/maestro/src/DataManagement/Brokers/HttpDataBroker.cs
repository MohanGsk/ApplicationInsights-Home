using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.ApplicationInsights.Ingestion.Maestro
{
    public class HttpDataBroker : IDataBroker<HttpResource, HttpBrokerResponse>
    {
        private HttpClient httpClient = new HttpClient();

        public async Task<HttpBrokerResponse> Query(HttpResource resource)
        {
            HttpRequestMessage request = new HttpRequestMessage() {
                RequestUri = resource.Uri,
                Method = HttpMethod.Get,
            };

            request.Headers.Add("User-Agent", Configuration.OUTGOING_USERAGENT);
            request.Headers.Add("Accept", "application/json");

            if (resource.Headers != null) 
            {
                foreach(var header in resource.Headers) 
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            HttpResponseMessage response = await httpClient.SendAsync(request);
            string responseText = await response.Content.ReadAsStringAsync();
            return new HttpBrokerResponse(responseText, response.StatusCode, response.ReasonPhrase);
        }

        public Task<HttpBrokerResponse> Submit(HttpResource resource, object payload)
        {
            throw new NotImplementedException();
        }
    }

    public class HttpBrokerResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string ResponseText { get; set; }

        public HttpBrokerResponse(string responseText, HttpStatusCode statusCode, string statusMessage)
        {
            ResponseText = responseText;
            StatusCode = statusCode;
            StatusMessage = statusMessage;
        }

        public T GetResponseAs<T>()
        {
            return JsonConvert.DeserializeObject<T>(ResponseText);
        }
    }
}