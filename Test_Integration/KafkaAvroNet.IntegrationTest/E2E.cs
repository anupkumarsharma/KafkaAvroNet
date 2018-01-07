using System;
using System.IO;
using KafkaAvroNet.Avro;
using KafkaAvroNet.Integration.Tests.Helpers;
using KafkaAvroNet.IntegrationTest;
using KafkaAvroNet.SchemaRegistry;
using KafkaAvroNet.SchemaRegistry.Models;
using KafkaAvroNet.SchemaRegistry.Providers;
using NUnit.Framework;

namespace KafkaAvroNet.Integration.Tests
{
    [TestFixture()]
    public class E2E : TestSetup
    {
        KafkaAvroNet<Employee> _kafkaAvroNet;
        KafkaCrudeConsumer _consumer;
        AvroDeserializer<Employee> _deser;
        private CachedSchemaRegistry<Schema> _schemaRegistry;

        public E2E()
        {

            _consumer = new KafkaCrudeConsumer();
            _schemaRegistry = new CachedSchemaRegistry<Schema>(new SchemaRegistryProvider(TestConstants.SCHEMA_REGISTRY_PATH));
        }

        [Test]
        public void UseSchemaRegistryToWriteToKafkaAndReadItBack()
        {

            _kafkaAvroNet = new KafkaAvroNet<Employee>(TestConstants.SCHEMA_REGISTRY_PATH,
                                                       TestConstants.KAFKA_SETTINGS_CONSUMER,
                                                       TestConstants.SUBJECT_NAME,
                                                       Model.SchemaProviderType.SchemaRegistry,
                                                       (sender, e) =>
                                                       {
                                                           Console.WriteLine(e.ToString());
                                                       }

                                                     );
            _kafkaAvroNet.WriteToKafka(new Employee() { age = 100, email = new string[] { "abc@abc.com" }, firstName = "test", lastName = "testLast", middleName = "testMiddle" });
            var arr = _consumer.GetData();
            var schema = _schemaRegistry.GetSchema(new Schema() { Topic = TestConstants.SUBJECT_NAME, Id = String.Empty });
            _deser = new AvroDeserializer<Employee>(new SerializationContext()
            {
                SchemaId = schema.Id,
                SchemaString = schema.SchemaString
            });
            Employee reflectedObject;
            using (MemoryStream stream = new MemoryStream(arr))
            {
                reflectedObject = _deser.Deserialize(stream, null);
            }
            Assert.AreEqual(reflectedObject.age, 100);
            Assert.AreEqual(reflectedObject.firstName, "test");
            Assert.AreEqual(reflectedObject.lastName, "testLast");
            Assert.AreEqual(reflectedObject.middleName, "testMiddle");
            Assert.AreEqual(reflectedObject.email[0], "abc@abc.com");
        }
    }
}
