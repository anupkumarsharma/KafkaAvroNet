using System;
using System.Threading.Tasks;

namespace KafkaAvroNet.Kafka.Interface
{
    public interface IKafKaProducer
    {
        void Write(byte[] value, string topicName, Action<Task> cb);
    }
}
