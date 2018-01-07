using System;
using System.Net.Http;
using System.Threading.Tasks;
using KafkaAvroNet.SchemaRegistry.Interface;
using KafkaAvroNet.SchemaRegistry.Models;
using KafkaAvroNet.SchemaRegistry.Providers;
using Moq;
using NUnit.Framework;

namespace KafkaAvroNet.SchemaRegistry.Test
{
  
    [TestFixture()]
    public class CachedSchemaRegistryTest
    {
        private ISchemaRegistryProvider _repoProviderProvider;
        private Mock<SchemaRepoProvider> _repoProviderProviderMock;
     
        public  CachedSchemaRegistryTest()
        {
            //act
             _repoProviderProviderMock = new Mock<SchemaRepoProvider>("http://testMock/api/")
            {
                CallBase = true
            };
            _repoProviderProviderMock
                .Setup(x => x.RunRequest<string>(
                    It.IsAny<string>(),
                    "testSubject/id/1",
                    HttpMethod.Get,
                    null
                )).Returns(() => Task.FromResult(
                    "{\"type\":\"record\",,\"fields\":[{\"name\":\"HouseStark\",\"type\":\"string\",\"default\":\"null\"}]}"));

            _repoProviderProviderMock
             .Setup(x => x.RunRequest<string>(
                 It.IsAny<string>(),
                 "testSubject/latest",
                 HttpMethod.Get,
                 null
             )).Returns(() => Task.FromResult(
                 "1\t{\"type\":\"record\",,\"fields\":[{\"name\":\"HouseStark\",\"type\":\"string\",\"default\":\"null\"}]}"));

            _repoProviderProviderMock
            .Setup(x => x.RunRequest<string>(
                It.IsAny<string>(),
                "testSubject/latest",
                HttpMethod.Get,
                null
            )).Returns(() => Task.FromResult(
                "1\t{\"type\":\"record\",,\"fields\":[{\"name\":\"HouseStark\",\"type\":\"string\",\"default\":\"null\"}]}"));

            _repoProviderProviderMock
            .Setup(x => x.RunRequest<string>(
                It.IsAny<string>(),
                "/",
                HttpMethod.Get,
                null
            )).Returns(() => Task.FromResult(
                "testSubject1\ntestSubject2"));


            _repoProviderProvider = _repoProviderProviderMock.Object;

        }
       [Test]
        public void GetSchemaShouldRetrieveSchemaFromServerForSchemaIdIfSchemaIsNotCachedUp()
        {
            CachedSchemaRegistry<Schema> cachedSchemaRegistry = new CachedSchemaRegistry<Schema>(_repoProviderProvider);
            var schemaMetadata = cachedSchemaRegistry.GetSchema(new Schema() {Id = "1", Topic = "testSubject" });
            Assert.True(schemaMetadata.SchemaString.Contains("HouseStark"));
        }

        [Test]
        public void GetSchemaShouldRetrieveSchemaFromServerForLatestIdIfSchemaIsNotCachedUp()
        {
            CachedSchemaRegistry<Schema> cachedSchemaRegistry = new CachedSchemaRegistry<Schema>(_repoProviderProvider);
            var schemaMetadata = cachedSchemaRegistry.GetSchema(new Schema() { Id = String.Empty, Topic = "testSubject" });
            Assert.True(schemaMetadata.SchemaString.Contains("HouseStark"));
            Assert.True(schemaMetadata.Id.Contains("1"));
        }

        [Test]
        public void GetSchemaShouldRetrieveSchemaFromDictionaryIfAlreadyFetchedInPast()
        {
          var cachedSchemaRegistry = new CachedSchemaRegistry<Schema>(_repoProviderProvider);
             cachedSchemaRegistry.GetSchema(new Schema() { Id = "1", Topic = "testSubject" });
             cachedSchemaRegistry.GetSchema(new Schema() { Id = "1", Topic = "testSubject" });
            _repoProviderProviderMock.Verify(t => t.RunRequest<string>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<HttpMethod>(),
                null
            ), Times.Once);
        }

    }
}
