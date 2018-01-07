using System;
using System.IO;
using KafkaAvroNet.Integration.Tests;
using KafkaAvroNet.IntegrationTest;
using KafkaAvroNet.SchemaRegistry;
using KafkaAvroNet.SchemaRegistry.Models;
using KafkaAvroNet.SchemaRegistry.Providers;
using Newtonsoft.Json;
using NUnit.Framework;
namespace KafkaNetIntegration
{
  
    [TestFixture()]
 
    public class SchemaRegistryTest:TestSetup
    {
        private CachedSchemaRegistry<Schema> _schemaRegistry;
        public SchemaRegistryTest()
        {
            _schemaRegistry = new CachedSchemaRegistry<Schema>(new SchemaRegistryProvider(TestConstants.SCHEMA_REGISTRY_PATH));
        }

        [Test()]
        public void GetSchemaFromInvalidTopicShouldThrowFatalApplicationException()
        {
            Assert.Throws<ApplicationException>(() => { _schemaRegistry.GetSchema(new Schema() { Topic = "topic", Id="1" }); });
        }

        [Test()]
        public void GetSchemaForNullSchemaIdShouldThrowArgumentError()
        {
            Assert.Throws<ArgumentException>(() => { _schemaRegistry.GetSchema(new Schema() { Topic = "test" }); });
        }

        [Test()]
        public void GetSchemaFromValidTopicReturnsSchema()
        {
            var schema=  _schemaRegistry.GetSchema(new Schema() { Topic = TestConstants.SUBJECT_NAME, Id=String.Empty });
            dynamic json = JsonConvert.DeserializeObject(schema.SchemaString);
            Assert.IsTrue(json.name=="Employee");
            Assert.IsTrue(json.type=="record");
        }


    }
}
