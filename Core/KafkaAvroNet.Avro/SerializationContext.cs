using Avro;

namespace KafkaAvroNet.Avro
{
    public class SerializationContext
    {

        public string Topic { get; set; }

        public string SchemaId { get; set; }

        public string SchemaString { get; set; }

        public Schema AvroSchema => Schema.Parse(SchemaString);
    }

}
