using System;
using System.Net.Http;
using System.Threading.Tasks;
using KafkaAvroNet.SchemaRegistry.Interface;
using KafkaAvroNet.SchemaRegistry.Providers;
using Moq;
using NUnit.Framework;

namespace KafkaAvroNet.SchemaRegistry.Tests.Providers
{

    [TestFixture()]
    public class SchemaRepoApiProviderTest
    {
        private ISchemaRegistryProvider _repoProviderProvider;

       
        public  SchemaRepoApiProviderTest()
        {

            //act
            var repoProviderProviderMock = new Mock<SchemaRepoProvider>("http://testMock/api/")
            {
                CallBase = true
            };
            repoProviderProviderMock
                .Setup(x => x.RunRequest<string>(
                    It.IsAny<string>(),
                    "testSubject/id/1",
                    HttpMethod.Get,
                    null
                )).Returns(() => Task.FromResult(
                    "{\"type\":\"record\",\"fields\":[{\"name\":\"HouseStark\",\"type\":\"string\",\"default\":\"null\"}]}"));

            repoProviderProviderMock
             .Setup(x => x.RunRequest<string>(
                 It.IsAny<string>(),
                 "testSubject/latest",
                 HttpMethod.Get,
                 null
             )).Returns(() => Task.FromResult(
                 "1\t{\"type\":\"record\",\"fields\":[{\"name\":\"HouseStark\",\"type\":\"string\",\"default\":\"null\"}]}"));

            repoProviderProviderMock
            .Setup(x => x.RunRequest<string>(
                It.IsAny<string>(),
                "testSubject/latest",
                HttpMethod.Get,
                null
            )).Returns(() => Task.FromResult(
                "1\t{\"type\":\"record\",\"fields\":[{\"name\":\"HouseStark\",\"type\":\"string\",\"default\":\"null\"}]}"));

            repoProviderProviderMock
            .Setup(x => x.RunRequest<string>(
                It.IsAny<string>(),
                "/",
                HttpMethod.Get,
                null
            )).Returns(() => Task.FromResult(
                "testSubject1\ntestSubject2"));


            _repoProviderProvider = repoProviderProviderMock.Object;

        }

        [Test()]
        public void ShouldBeAbleToRetrieveASchemaForAnSubjectId()
        {
           var schema =  _repoProviderProvider.GetBySubjectAndId("testSubject", "1");
            //Validate the field exists
            Assert.IsTrue(schema.SchemaString.Contains("HouseStark"));
        }


        [Test()]

        public void ShouldThrowExceptionForInvalidId()
        {

            Assert.Throws<ArgumentNullException>(() => _repoProviderProvider.GetBySubjectAndId("4344343", ""));
        }


        [Test()]

        public void ShouldThrowExceptionForInvalidSubject()
        {
            // _repoProviderProvider.GetBySubjectAndId("",null);
            Assert.Throws<ArgumentNullException>(() => _repoProviderProvider.GetBySubjectAndId("", null));
        } 
        [Test()]
        public void ShouldBeAbleToGetTheLatestSchemaForASubject()
        {
            var schema = _repoProviderProvider.GetLatestSchemaMetadata("testSubject");
            Assert.IsTrue(schema.SchemaString.Contains("HouseStark"));
            Assert.IsTrue(schema.Id=="1");
        }


        [Test()]
        public void ShouldBeAbleToRetriveAllSubjectFromSchemaRepo()
        {
            var schema = _repoProviderProvider.GetAllSubjects();
            Assert.IsTrue(schema.Length==2);
        }
    }
}

