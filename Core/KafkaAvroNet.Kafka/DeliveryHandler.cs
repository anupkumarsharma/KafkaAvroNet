using System;
using Confluent.Kafka;

namespace KafkaAvroNet.Kafka
{
    public class DeliveryHandler :IDeliveryHandler<Null,byte[]>
    {
        private readonly EventHandler<String> _eventHandler;
        public DeliveryHandler(EventHandler<String> eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public bool MarshalData { get; }
        public void HandleDeliveryReport(Message<Null, byte[]> message)
        {
            if (message.Error.HasError)
            {
                _eventHandler(this, message.Error.ToString());
            }
        }
    }
}
