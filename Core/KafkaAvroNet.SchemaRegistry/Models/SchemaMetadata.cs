using System;
using Newtonsoft.Json;

namespace KafkaAvroNet.SchemaRegistry.Models
{
    public class SchemaMetadata
    {
        public string Subject { get; set; }
        public string Version { get; set; }
        public string Id { get; set; }
        [JsonProperty(PropertyName = "schema")]
        public string SchemaString { get; set; }
 
    }
}
