using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace KafkaAvroNet.Integration.Tests.Helpers
{
    public  class KafkaCrudeConsumer
    {
         readonly Consumer<Null, byte[]> consumer;
        public KafkaCrudeConsumer()
        {
           
            consumer = new Consumer<Null, byte[]>(TestConstants.KAFKA_SETTINGS_CONSUMER, null, new ByteArrayDeserializer());
                consumer.Assign(new List<TopicPartitionOffset> { new TopicPartitionOffset(TestConstants.SUBJECT_NAME, 0, 0) });
                // Raised on critical errors, e.g. connection failures or all brokers down.
                consumer.OnError += (_, error)
                    => Console.WriteLine($"Error: {error}");
                // Raised on deserialization errors or when a consumed message has an error != NoError.
                consumer.OnConsumeError += (_, error)
                    => Console.WriteLine($"Consume error: {error}");
                consumer.Subscribe(TestConstants.SUBJECT_NAME);   
        }


        public byte[] GetData()
        {
            List<Tuple<string, int, byte[]>> _partitionLoad = new List<Tuple<string, int, byte[]>>();

            Message<Null, byte[]> msg;
            int i = 10;
            while (i >= 0)
            {
                i--;
                if (consumer.Consume(out msg, TimeSpan.FromSeconds(1)))
                    _partitionLoad.Add(new Tuple<string, int, byte[]>(msg.Topic, msg.Partition, msg.Value));
            }
            consumer.CommitAsync();
            if (_partitionLoad.Count == 0)
            {
                throw new Exception("unable to read data from Kafka ");
            }
            return _partitionLoad[0].Item3;
        }
    }
}
