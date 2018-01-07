

using Avro;
using KafkaAvroNet.Avro.Providers;
using NUnit.Framework;

namespace KafkaAvroNet.Avro.Test.Providers
{
    [TestFixture()]
    public class ReflectionSerializationProviderTest
    {
        private readonly ReflectionSerializationProvider _reflectionSerialization;

        public ReflectionSerializationProviderTest()
        {
            _reflectionSerialization = new ReflectionSerializationProvider();

        }

        [TestCase("{\"type\":\"record\",\"name\":\"test\", \"fields\":[{\"name\":\"HouseStark\",\"type\":\"string\",\"default\":\"null\"}]}", "e")]
        [TestCase("{\"type\":\"record\",\"name\":\"test\",\"fields\":[{\"name\":\"HouseStark\",\"type\":\"int\",\"default\":\"null\"}]}", 2)]
        [TestCase("{\"type\":\"record\",\"name\":\"test\",\"fields\":[{\"name\":\"HouseStark\",\"type\":\"long\",\"default\":\"null\"}]}", 2000000000000)]
        [TestCase("{\"type\":\"record\",\"name\":\"test\",\"fields\":[{\"name\":\"HouseStark\",\"type\":\"double\",\"default\":\"null\"}]}", 100.75)]
        [TestCase("{\"name\":\"test\",\"type\":\"record\",\"fields\":[{\"name\":\"HouseStark\",\"type\":{\"type\":\"array\",\"items\":\"float\"}}]}", new float[] { 23.67f, 22.78f })]
        [TestCase("{\"name\":\"test\",\"type\":\"record\",\"fields\":[{\"name\":\"HouseStark\",\"type\":{\"type\":\"array\",\"items\":\"string\"}}]}", new string[] { "test1", "test2" })]

        public void ReflectionSerializerToAddTypeAsGenericRecordShouldBeAbleToSerialize<T>( string schemaString, T obj)
        {
            var y = new param<T>((T)obj);
            var bytesCode = _reflectionSerialization.Format(y, Schema.Parse(schemaString), schemaString);
            Assert.NotNull(bytesCode);
        }

    }   
    public class param<T>
    {
        public T HouseStark { get; set; }

        public param(T ob)
        {
            HouseStark = ob;
        }

    }



} 

 