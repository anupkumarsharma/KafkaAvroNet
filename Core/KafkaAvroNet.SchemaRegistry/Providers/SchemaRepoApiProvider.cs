using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KafkaAvroNet.SchemaRegistry.Interface;
using KafkaAvroNet.SchemaRegistry.Models;

namespace KafkaAvroNet.SchemaRegistry.Providers
{
    /// <summary>
    /// This class provides caller for schema repo
    /// https://github.com/schema-repo/schema-repo/wiki/Service-Endpoints
    /// </summary>
    public class SchemaRepoProvider : SchemaProviderBase,ISchemaRegistryProvider
    {
        private readonly string _registryUrl;

        public SchemaRepoProvider(string registryUrl)
        {
            _registryUrl = registryUrl;
        }

        /// <summary>
        /// Get the specified path.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="path">Path.</param>
        /// <typeparam name="TResponse">The 1st type parameter.</typeparam>
        private Task<string> Get<TResponse>(string path)
        {
            return RunRequest<string>(_registryUrl, path, HttpMethod.Get, null);
        }

        /// <summary>
        /// Gets all subjects.
        /// </summary>
        /// <returns>The all subjects.</returns>
        public string[] GetAllSubjects()
        {
            return  Get<string>("/").Result.Split('\n');
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <returns>The by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public Schema GetById(string id)
        {
            throw new NotImplementedException("This operation is not supported in Schema Repo");
        }

        /// <summary>
        /// Gets the schema versions.
        /// </summary>
        /// <returns>The schema versions.</returns>
        /// <param name="subject">Subject.</param>
        public int[] GetSchemaVersions(string subject)
        {
            throw new NotImplementedException("This operation is not supported in Schema Repo");
        }

        /// <summary>
        /// Gets the by subject and identifier.
        /// </summary>
        /// <returns>The by subject and identifier.</returns>
        /// <param name="subject">Subject.</param>
        /// <param name="versionId">Version identifier.</param>
        public SchemaMetadata GetBySubjectAndId(string subject, string versionId)
        {
            if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(versionId))
            {
                throw new ArgumentNullException("Invalid Subject/VersionID");
            }
        
            return new SchemaMetadata()
            {
                Id = versionId,
                SchemaString = Get<string>(string.Format("{0}/id/{1}", subject, versionId)).Result,
                Subject = subject,
                Version = versionId
            };  
        }

        /// <summary>
        /// Gets the latest schema metadata.
        /// </summary>
        /// <returns>The latest schema metadata.</returns>
        /// <param name="subject">Subject.</param>
       public SchemaMetadata GetLatestSchemaMetadata(string subject)
        {
            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }
            var schemaString = Get<SchemaMetadata>(string.Format("{0}/latest", subject)).Result;
            return new SchemaMetadata()
            {
                Id = schemaString.Split('\t').First(),
                SchemaString = schemaString.Split('\t').Last(),
                Subject = subject,
                Version = schemaString.Split('\t').First()
            };

        }
    }
}
