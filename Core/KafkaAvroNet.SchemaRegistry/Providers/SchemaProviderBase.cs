using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KafkaAvroNet.SchemaRegistry.Providers
{
    public   class SchemaProviderBase
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            Formatting = Formatting.Indented
        };

        /// <summary>
        /// Runs the request.
        /// </summary>
        /// <returns>The request.</returns>
        /// <param name="registryUrl">Registry URL.</param>
        /// <param name="path">Path.</param>
        /// <param name="method">Method.</param>
        /// <param name="payload">Payload.</param>
        /// <typeparam name="TRequest">The 1st type parameter.</typeparam>
        public virtual async Task<string> RunRequest<TRequest>(string registryUrl, string path, HttpMethod method, TRequest payload) 
        {
           

            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage();
                request.Headers.Add("Accept",
                    "application/vnd.schemaregistry.v1+json, application/vnd.schemaregistry+json, application/json");
                request.Method = method;
                var url = registryUrl + path;
                request.RequestUri = new Uri(url);

                if (payload != null)
                {
                    var payloadString = JsonConvert.SerializeObject(payload, JsonSerializerSettings);
                    request.Headers.Add("Content-Type", "application/json");
                    request.Content = new StringContent(payloadString, Encoding.UTF8);
                }

                var response = await httpClient.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                return responseString;
            }
        }


    }
}
