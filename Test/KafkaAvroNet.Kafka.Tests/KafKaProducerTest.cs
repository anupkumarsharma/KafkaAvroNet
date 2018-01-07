using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace KafkaAvroNet.Kafka.Tests
{
    [TestFixture()]
    public class KafkaProducerTest
    {
        KafKaProducer _producer; 
        
        public KafkaProducerTest()
        {
            _producer = new KafKaProducer(new Dictionary<string, object>(){{"bootstrap.servers","e"} });
        }

        [Test]
        public void ConstructorShouldThrowNullWhenConfigurationListIsNull(){
            Assert.Throws<ArgumentNullException>(() => { new KafKaProducer(null); });
        }

        [Test]
        public void ConstructorShouldThrowNullWhenConfigurationListIsBootstrapIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => { new KafKaProducer(new Dictionary<string, object>() { { "Wrongbootstrap.servers", "e" } }); });
        }

        [Test]
        public void WriteShouldThrowExceptionWhenTopicIsUndefined(){
            Assert.Throws<ArgumentNullException>(() => { _producer.Write(new byte[] { }, ""); });
        }

        [Test]
        public void WriteListShouldThrowExceptionWhenTopicIsUndefined()
        {
            Assert.Throws<ArgumentNullException>(() => { _producer.Write(new List<byte[]>(), ""); });
        }
    }
}
