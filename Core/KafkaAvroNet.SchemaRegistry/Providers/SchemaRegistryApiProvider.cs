using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using KafkaAvroNet.SchemaRegistry.Interface;
using KafkaAvroNet.SchemaRegistry.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KafkaAvroNet.SchemaRegistry.Providers
{
    /// <summary>
    /// Schema registry provider.
    /// </summary>
    public class SchemaRegistryProvider : SchemaProviderBase , ISchemaRegistryProvider
    {
        private readonly string _registryUrl;


        public SchemaRegistryProvider(string url)
        {
            _registryUrl = url.TrimEnd('/');
        }

        /// <summary>
        /// Get the specified path.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="path">Path.</param>
        /// <typeparam name="TResponse">The 1st type parameter.</typeparam>
        private  async Task<TResponse> Get<TResponse>(string path)
        {
            var resultString=  base.RunRequest<string>(_registryUrl, path, HttpMethod.Get, null).Result;
         return JsonConvert.DeserializeObject<TResponse>(resultString); 
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <returns>The by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public Schema GetById(string id)
        {
         
            return Get<Schema>($"/schemas/ids/{id}").Result;
        }

        /// <summary>
        /// Gets all subjects.
        /// </summary>
        /// <returns>The all subjects.</returns>
        public string[] GetAllSubjects()
        {
            return Get<string[]>("/subjects").Result;
        }
        /// <summary>
        /// Gets the schema versions.
        /// </summary>
        /// <returns>The schema versions.</returns>
        /// <param name="subject">Subject.</param>
        public int[] GetSchemaVersions(string subject)
        {
            return Get<int[]>($"/subjects/{subject}/versions").Result;
        }
        /// <summary>
        /// Gets the by subject and identifier.
        /// </summary>
        /// <returns>The by subject and identifier.</returns>
        /// <param name="subject">Subject.</param>
        /// <param name="versionId">Version identifier.</param>

        public SchemaMetadata GetBySubjectAndId(string subject, string versionId)
        {
            return Get<SchemaMetadata>($"/subjects/{subject}/versions/{versionId}").Result;
        }

        /// <summary>
        /// Gets the latest schema metadata.
        /// </summary>
        /// <returns>The latest schema metadata.</returns>
        /// <param name="subject">Subject.</param>

        public SchemaMetadata GetLatestSchemaMetadata(string subject)
        {
            return Get<SchemaMetadata>($"/subjects/{subject}/versions/latest").Result;
        }

       
    }
}