using System;
using System.IO;
using NUnit.Framework;

namespace KafkaAvroNet.Avro.Test
{
    [TestFixture]
    public class AvroDeserializertest
    {
     

        [Test]
        public void SerializeShouldReturnObjectForByteOfTypeEmp()
        {
            string Schema = "{\"type\":\"record\",\"name\":\"emp\", \"fields\":[{\"name\":\"anup\",\"type\":\"string\",\"default\":\"null\"}]}";
            var serializationContext = new SerializationContext()
            {
                SchemaId = "1",
                SchemaString = Schema,
                Topic = "test"
            };
            AvroSerializer<emp> _serializer = new AvroSerializer<emp>(serializationContext);

            var bytesCode = _serializer.Serialize(new emp() { anup = "test" });
            AvroDeserializer<emp> deserializer = new AvroDeserializer<emp>(serializationContext);
            emp reflectedobject;
            using (MemoryStream _mem = new MemoryStream(bytesCode))
            {
                reflectedobject = deserializer.Deserialize(_mem, null);
            }

            Assert.AreEqual(reflectedobject.anup,"test");
        }



        [TestCase("{\"type\":\"record\",\"name\":\"test\", \"fields\":[{\"name\":\"HouseStark\",\"type\":\"string\",\"default\":\"null\"}]}", "e")]
        [TestCase("{\"type\":\"record\",\"name\":\"test\",\"fields\":[{\"name\":\"HouseStark\",\"type\":\"int\",\"default\":\"null\"}]}", 2)]
        [TestCase("{\"type\":\"record\",\"name\":\"test\",\"fields\":[{\"name\":\"HouseStark\",\"type\":\"long\",\"default\":\"null\"}]}", 2000000000000)]
        [TestCase("{\"type\":\"record\",\"name\":\"test\",\"fields\":[{\"name\":\"HouseStark\",\"type\":\"double\",\"default\":\"null\"}]}", 100.75)]
        [TestCase("{\"name\":\"test\",\"type\":\"record\",\"fields\":[{\"name\":\"HouseStark\",\"type\":{\"type\":\"array\",\"items\":\"float\"}}]}", new float[] { 23.67f, 22.78f })]
        [TestCase("{\"name\":\"test\",\"type\":\"record\",\"fields\":[{\"name\":\"HouseStark\",\"type\":{\"type\":\"array\",\"items\":\"string\"}}]}", new string[] { "test1", "test2" })]

        public void DeSerializeShouldReturnTForByte<T>(string schemaString, T obj) 
        {
            var y = new param<T>((T)obj);
            var serializationContext = new SerializationContext()
            {
                SchemaId = "1",
                SchemaString = schemaString,
                Topic = "test"
            };

            AvroSerializer<param<T>> _serializer = new AvroSerializer<param<T>>(serializationContext);

            var bytesCode = _serializer.Serialize(y);

            AvroDeserializer<param<T>> deserializer = new AvroDeserializer<param<T>>(serializationContext);
            param<T> reflectedobject;
            using (MemoryStream _mem = new MemoryStream(bytesCode))
            {
                reflectedobject = deserializer.Deserialize(_mem, null);
            }

            Assert.AreEqual(reflectedobject.HouseStark.ToString(),obj.ToString());
        }

       

    }
}
