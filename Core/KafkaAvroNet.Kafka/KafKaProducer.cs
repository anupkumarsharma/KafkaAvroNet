using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using KafkaAvroNet.Kafka.Interface;

namespace KafkaAvroNet.Kafka
{
    public class KafKaProducer : IKafKaProducer, IDisposable
    {
        private readonly Producer<Null, byte[]> _producer;
        private IDeliveryHandler<Null, byte[]> _eventHandler = null;

        /// <summary>
        /// Initializes the Kafka Producer. For list of setting refer - https://kafka.apache.org/documentation.html#producerconfigs
        /// </summary>
        /// <param name="configList"></param>
        /// <param name="cbOnError"></param>
        public KafKaProducer(Dictionary<string, object> configList, EventHandler<Error> cbOnError = null, EventHandler<string> cbDeliveryEventHandler = null)
        {
            if (configList == null || !configList.ContainsKey("bootstrap.servers"))
            {
                throw new ArgumentNullException(nameof(configList), "bootstrap.servers is not defined");
            }
            _producer = new Producer<Null, byte[]>(configList, null, new ByteArraySerializer());
            if (cbOnError != null)
                _producer.OnError += cbOnError;

            if (cbDeliveryEventHandler != null)
                _eventHandler = new DeliveryHandler(cbDeliveryEventHandler);
        }

        /// <summary>
        /// Write the specified value, topicName and cb.
        /// </summary>
        /// <returns>The write.</returns>
        /// <param name="value">Value.</param>
        /// <param name="topicName">Topic name.</param>
        /// <param name="cb">Cb.</param>

        public void Write(byte[] value, string topicName, Action<Task> cb = null)
        {
            if (value == null || String.IsNullOrEmpty(topicName)) { throw new ArgumentNullException("topicName/value"); }
            if (cb != null)
            {
                var deliveryReport = _producer.ProduceAsync(topicName, null, value);
                deliveryReport.Wait();
                deliveryReport.ContinueWith(task =>
                {
                    cb(task);
                });
            }
            else
            {
                FastProduce(topicName, value);

            }
        }

        /// <summary>
        /// Write the specified value, topicName and cb.
        /// </summary>
        /// <returns>The write.</returns>
        /// <param name="value">Value.</param>
        /// <param name="topicName">Topic name.</param>
        /// <param name="cb">Cb.</param>
        public void Write(List<byte[]> value, string topicName, Action<Task> cb = null)
        {
            if (value == null || String.IsNullOrEmpty(topicName)) { throw new ArgumentNullException("topicName/value"); }
            foreach (var payload in value)
            {
                if (cb != null)
                {
                    var deliveryReport = _producer.ProduceAsync(topicName, null, payload);
                    deliveryReport.Wait();
                    deliveryReport.ContinueWith(cb);
                }
                else
                {
                    FastProduce(topicName, payload);
                }
            }
        }

        /// <summary>
        /// Fastest way to write to Kafka. This involves no delivery report, hence void return. To handle
        /// any error OnError callback should be appropriately used
        /// </summary>
        /// <param name="topicName">Topic name.</param>
        /// <param name="value">Value.</param>
        private void FastProduce(string topicName, byte[] value)
        {
            if (_eventHandler != null)
                _producer.ProduceAsync(topicName, null, value, _eventHandler);
            else
                _producer.ProduceAsync(topicName, null, value);
        }

        /// <summary>
        /// Releases all resource used by the <see cref="T:KafkaAvroNet.Kafka.KafKaProducer"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="T:KafkaAvroNet.Kafka.KafKaProducer"/>.
        /// The <see cref="Dispose"/> method leaves the <see cref="T:KafkaAvroNet.Kafka.KafKaProducer"/> in an unusable
        /// state. After calling <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:KafkaAvroNet.Kafka.KafKaProducer"/> so the garbage collector can reclaim the memory that the
        /// <see cref="T:KafkaAvroNet.Kafka.KafKaProducer"/> was occupying.</remarks>
        public void Dispose()
        {
            var result = 1;
            // For now this time is kept as 3 min and loop until message left in queue = 0
            while (result != 0)
            {
                result = _producer.Flush(TimeSpan.FromSeconds(1800));
            }

            _producer.Dispose();
        }
    }
}
