using System;
using System.Collections.Generic;
using KafkaAvroNet.SchemaRegistry.Interface;
using KafkaAvroNet.SchemaRegistry.Models;

namespace KafkaAvroNet.SchemaRegistry
{
    /// <summary>
    /// Cached schema registry.
    /// </summary>
    public class CachedSchemaRegistry<T> where T:Schema
    {
        readonly Dictionary<Schema, SchemaMetadata> _dictionary;
        readonly ISchemaRegistryProvider _registryProvider;

        public CachedSchemaRegistry(ISchemaRegistryProvider provider )
        {
            _registryProvider = provider ?? throw new ArgumentNullException(nameof(provider)); 
            _dictionary = new Dictionary<Schema, SchemaMetadata>(); 
        }
        /// <summary>
        /// Gets the schema.
        /// </summary>
        /// <returns>The schema.</returns>
        /// <param name="schema">Schema. For Latest Schema Pass Empty String</param>

        public SchemaMetadata GetSchema(Schema schema)
        {
          
            if (String.IsNullOrEmpty(schema?.Topic)|| schema.Id==null)
            {
                throw new ArgumentException("One more more argument is not valid");
            }

            if (_dictionary.ContainsKey(schema))
            {
                return _dictionary[schema];
            }

            var schemaMetadata = schema.Id==String.Empty
                ? _registryProvider.GetLatestSchemaMetadata(schema.Topic)
                : _registryProvider.GetBySubjectAndId(schema.Topic, schema.Id);
            
            if(String.IsNullOrEmpty(schemaMetadata.SchemaString)){
                throw new ApplicationException("Invalid Schema or unable to get load");
            }
            _dictionary.Add(schema, schemaMetadata);
            return schemaMetadata;
        }
    }
}
