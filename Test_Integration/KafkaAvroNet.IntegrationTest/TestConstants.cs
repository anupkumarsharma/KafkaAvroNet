using System;
using System.Collections.Generic;

namespace KafkaAvroNet.Integration.Tests
{
    public static class TestConstants
    {
        //public const string SCHEMA = "{\"schema\":\"{\\\"namespace\\\":\\\"kafkaavronet.Employee\\\",\\\"type\\\":\\\"record\\\",\\\"name\\\":\\\"Employee\\\",\\\"fields\\\":[{\\\"name\\\":\\\"firstName\\\",\\\"type\\\":\\\"string\\\",\\\"doc\\\":\\\"The persons given name\\\"},{\\\"name\\\":\\\"middleName\\\",\\\"type\\\":[\\\"null\\\",\\\"string\\\"],\\\"default\\\":null},{\\\"name\\\":\\\"lastName\\\",\\\"type\\\":\\\"string\\\"},{\\\"name\\\":\\\"age\\\",\\\"type\\\":[\\\"null\\\",\\\"int\\\"],\\\"default\\\":null},{\\\"name\\\":\\\"email\\\",\\\"default\\\":[],\\\"type\\\":{\\\"type\\\":\\\"array\\\",\\\"items\\\":\\\"string\\\"}},{\\\"name\\\":\\\"compensation\\\",\\\"default\\\":\\\"compensation\\\",\\\"type\\\":{\\\"type\\\":\\\"enum\\\",\\\"name\\\":\\\"compensation\\\",\\\"symbols\\\":[\\\"RETIRED\\\",\\\"SALARY\\\",\\\"CONTRACTOR\\\"]}}]} \"}";
             public const string SCHEMA = "{\"schema\":\"{\\\"namespace\\\":\\\"kafkaavronet.Employee\\\",\\\"type\\\":\\\"record\\\",\\\"name\\\":\\\"Employee\\\",\\\"fields\\\":[{\\\"name\\\":\\\"firstName\\\",\\\"type\\\":\\\"string\\\",\\\"doc\\\":\\\"The persons given name\\\"},{\\\"name\\\":\\\"middleName\\\",\\\"type\\\":[\\\"null\\\",\\\"string\\\"],\\\"default\\\":null},{\\\"name\\\":\\\"lastName\\\",\\\"type\\\":\\\"string\\\"},{\\\"name\\\":\\\"age\\\",\\\"type\\\":[\\\"null\\\",\\\"int\\\"],\\\"default\\\":null},{\\\"name\\\":\\\"email\\\",\\\"default\\\":[],\\\"type\\\":{\\\"type\\\":\\\"array\\\",\\\"items\\\":\\\"string\\\"}}]} \"}";
        //public const string SCHEMA = "{\"schema\":\"{\\\"namespace\\\":\\\"kafkaavronet.Employee\\\"," +
            //"\\\"type\\\":\\\"record\\\",\\\"name\\\":\\\"Employee\\\",\\\"fields\\\":[{\\\"name\\\":\\\"firstName\\\",\\\"type\\\":\\\"string\\\",\\\"doc\\\":\\\"The persons given name\\\"}," +
            //"{\\\"name\\\":\\\"middleName\\\",\\\"type\\\":[\\\"null\\\",\\\"string\\\"],\\\"default\\\":null}," +
            //"{\\\"name\\\":\\\"lastName\\\",\\\"type\\\":\\\"string\\\"}," +
            //"{\\\"name\\\":\\\"age\\\",\\\"type\\\":[\\\"null\\\",\\\"int\\\"],\\\"default\\\":null}" +
            //"]} \"}";
            // "{\\\"name\\\":\\\"email\\\",\\\"type\\\":\\\"string\\\"}" +

        public const string SUBJECT_NAME = "kafkaavronet-employee";
        public const string KAFKA_TOPIC = "topic1";
        public const string SCHEMA_REGISTRY_PATH = "http://localhost:8081/";
        public static Dictionary<string, object> KAFKA_SETTINGS = new Dictionary<string, object>() { { "bootstrap.servers", "localhost:9092" } };
      
        public static Dictionary<string, object> KAFKA_SETTINGS_CONSUMER  =  new Dictionary<string, object>
            {
                { "group.id", "advanced-csharp-consumer" },
                { "enable.auto.commit", true},
                { "auto.commit.interval.ms", 5000 },
                { "statistics.interval.ms", 60000 },
            { "bootstrap.servers", "127.0.0.1:29092" },
                { "default.topic.config", new Dictionary<string, object>()
                    {
                        { "auto.offset.reset", "smallest" }
                    }
                }
            };
    }
}
