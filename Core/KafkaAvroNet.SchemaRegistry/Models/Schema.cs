using System;
namespace KafkaAvroNet.SchemaRegistry.Models
{
    public class Schema 
    {
        public string Topic { get; set; }
        public string Id { get; set; }

        public override int GetHashCode()
        {
            return Topic.GetHashCode() + Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }
    }
}
    