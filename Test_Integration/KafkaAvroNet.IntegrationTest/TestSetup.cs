using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using KafkaAvroNet.Integration.Tests;
using NUnit.Framework;

namespace KafkaAvroNet.IntegrationTest
{
  
    public  class TestSetup
    {

        [OneTimeSetUp]
        public void InitializeSchemaRegistry()
        {
            HttpClient _http = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("Accept", "application/vnd.schemaregistry.v1+json, application/vnd.schemaregistry+json, application/json");
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri("http://localhost:8081/subjects/"+TestConstants.SUBJECT_NAME+"/versions");
            request.Content = new StringContent(TestConstants.SCHEMA, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = _http.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
        }

        [OneTimeTearDown]
        public void DestroySchemaRegistry(){

            HttpClient _http = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("Accept", "application/vnd.schemaregistry.v1+json, application/vnd.schemaregistry+json, application/json");
            request.Method = HttpMethod.Delete;
            request.RequestUri = new Uri(" http://localhost:8081/subjects/"+TestConstants.SUBJECT_NAME);
            var response = _http.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
        }
        
    }

    public class SchemaContainer
    {
        public string Schema { get; set; }
    }
}
