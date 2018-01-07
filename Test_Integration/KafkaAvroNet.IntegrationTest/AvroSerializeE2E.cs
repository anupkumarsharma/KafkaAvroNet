
using System;
using System.Dynamic;
using KafkaAvroNet.Avro;
using KafkaAvroNet.IntegrationTest;
using KafkaAvroNet.SchemaRegistry;
using KafkaAvroNet.SchemaRegistry.Models;
using KafkaAvroNet.SchemaRegistry.Providers;
using NUnit.Framework;

namespace KafkaAvroNet.Integration.Tests
{
    [TestFixture()]
    public class AvroSerializeE2E : TestSetup
    {
        private AvroSerializer<Employee> _avroSerializer;
        private CachedSchemaRegistry<Schema> _schemaRegistry;
        public AvroSerializeE2E()
        {
            _schemaRegistry = new CachedSchemaRegistry<Schema>(new SchemaRegistryProvider(TestConstants.SCHEMA_REGISTRY_PATH));


        }

        [Test]
        public void SerializeShoudReturnByteWhenModelIsValid()
        {
            var schema = _schemaRegistry.GetSchema(new Schema() { Topic = TestConstants.SUBJECT_NAME, Id = String.Empty });
            _avroSerializer = new AvroSerializer<Employee>(new SerializationContext()
            {
                SchemaId = schema.Id,
                Topic = schema.Subject,
                SchemaString = schema.SchemaString

            });
            var byteCode = _avroSerializer.Serialize(new Employee()
            {
                age = 1,
                lastName = "last",
                firstName = "first",
                middleName = "middle",
                email = new string[] { "test@test.com" }
               // email = "test@test.com" 
            });

            Assert.NotNull(byteCode);
        }

        [Test]
        public void SerializeShoudThrowExceptionWhenModelIsInvalid()
        {
            var schema = _schemaRegistry.GetSchema(new Schema() { Topic = TestConstants.SUBJECT_NAME, Id = String.Empty });
            AvroSerializer<InvalidEmployee> _avroSerializer;
            _avroSerializer = new AvroSerializer<InvalidEmployee>(new SerializationContext()
            {
                SchemaId = schema.Id,
                Topic = schema.Subject,
                SchemaString = schema.SchemaString

            });
            InvalidEmployee invalidModel = new InvalidEmployee();
            invalidModel.lastName = "last";
            invalidModel.firstName = "first";
            invalidModel.middleName = "middle";
            invalidModel.InvalidAgeProperty = 1;
            invalidModel.email = new string[] { "test@test.com" };
           Assert.Throws<InvalidCastException>(()=>_avroSerializer.Serialize(invalidModel));
        }
    }
}

    

