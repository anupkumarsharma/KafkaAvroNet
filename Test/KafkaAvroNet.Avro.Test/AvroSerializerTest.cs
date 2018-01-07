using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using NUnit.Framework;

namespace KafkaAvroNet.Avro.Test
{
    [TestFixture()]
    public class AvroSerializerTest
    {


        [TestCase("{\"type\":\"record\",\"name\":\"test\", \"fields\":[{\"name\":\"HouseStark\",\"type\":\"string\",\"default\":\"null\"}]}", "e")]
        [TestCase("{\"type\":\"record\",\"name\":\"test\",\"fields\":[{\"name\":\"HouseStark\",\"type\":\"int\",\"default\":\"null\"}]}", 2)]
        [TestCase("{\"type\":\"record\",\"name\":\"test\",\"fields\":[{\"name\":\"HouseStark\",\"type\":\"long\",\"default\":\"null\"}]}", 2000000000000)]
        [TestCase("{\"type\":\"record\",\"name\":\"test\",\"fields\":[{\"name\":\"HouseStark\",\"type\":\"double\",\"default\":\"null\"}]}", 100.75)]
        [TestCase("{\"name\":\"test\",\"type\":\"record\",\"fields\":[{\"name\":\"HouseStark\",\"type\":{\"type\":\"array\",\"items\":\"float\"}}]}", new float[] { 23.67f, 22.78f })]
        [TestCase("{\"name\":\"test\",\"type\":\"record\",\"fields\":[{\"name\":\"HouseStark\",\"type\":{\"type\":\"array\",\"items\":\"string\"}}]}", new string[] { "test1", "test2" })]

        public void SerializeShouldReturnByteForTypeT<T>(string schemaString, T obj)
        {
            var y = new param<T>((T)obj);
            AvroSerializer<param<T>> _serializer = new AvroSerializer<param<T>>(new SerializationContext()
            {
                SchemaId = "1",
                SchemaString = schemaString,
                Topic = "test"
            });

            var bytesCode = _serializer.Serialize(y);
            Assert.NotNull(bytesCode);
        }


        public void SerializeShouldReturnByteForTypeListOfT<T>(string schemaString, T obj)
        {
            var y = new param<T>((T)obj);
            AvroSerializer<param<T>> _serializer = new AvroSerializer<param<T>>(new SerializationContext()
            {
                SchemaId = "1",
                SchemaString = schemaString,
                Topic = "test"
            });

            var bytesCodeList = _serializer.Serialize(new List<param<T>>() { y });
            Assert.NotNull(bytesCodeList[0]);
        }

        public void SerializeShouldThrowExceptionWhenObjectIsNullT<T>(string schemaString, T obj)
        {
            param<T> p = null;
            AvroSerializer<param<T>> _serializer = new AvroSerializer<param<T>>(new SerializationContext()
            {
                SchemaId = "1",
                SchemaString = schemaString,
                Topic = "test"
            });
            Assert.Throws<ArgumentNullException>(() => { _serializer.Serialize(p); });

        }

     
    }
}
