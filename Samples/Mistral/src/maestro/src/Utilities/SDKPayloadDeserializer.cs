using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.ApplicationInsights.Ingestion.Maestro
{
    /** Helper to deserialize HttpRequest POST data containing AI envelopes  */
    public static class SDKPayloadDeserializer
    {
        /** Convert the POST body on a supplied HttpRequest to AI envelopes */
        public static async Task<Contracts.Envelope[]> Deserialize(HttpRequest req)
        {
            Stream envelopeStream = req.Body;

            // Is this gzip encoded?
            if (Configuration.ENABLE_GZIP_DECOMPRESSION && req.Headers[HeaderNames.ContentEncoding] == "gzip")
            {
                envelopeStream = new GZipStream(req.Body, CompressionMode.Decompress);
            }

            // Deserialize JSON
            JsonSerializer serializer = new JsonSerializer();

            try
            {
                using (JsonTextReader jsonReader = new JsonTextReader(new StreamReader(envelopeStream)))
                {
                    jsonReader.Read();

                    // Is this a raw envelope instead of a batch payload?
                    if (jsonReader.TokenType == JsonToken.StartObject)
                    {
                        JObject jObj = await JObject.LoadAsync(jsonReader);
                        try
                        {
                            return new Contracts.Envelope[] { jObj.ToObject<Contracts.Envelope>() };
                        }
                        catch(JsonException)
                        {
                            return null;
                        }
                    }

                    // Deserialize one by one
                    // This allows us to fail certain items but not the whole batch
                    List<Contracts.Envelope> batch = new List<Contracts.Envelope>();
                    JArray array = await JArray.LoadAsync(jsonReader);
                    foreach(JToken child in array.Children())
                    {
                        try
                        {
                            batch.Add(child.ToObject<Contracts.Envelope>());
                        }
                        catch(JsonException)
                        {
                            batch.Add(null);
                        }
                    }                    

                    return batch.ToArray();
                }
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}