using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaAvroNet.Avro;
using KafkaAvroNet.Kafka;
using KafkaAvroNet.Model;
using KafkaAvroNet.SchemaRegistry;
using KafkaAvroNet.SchemaRegistry.Models;
using KafkaAvroNet.SchemaRegistry.Providers;

namespace KafkaAvroNet
{

    public class KafkaAvroNet<T> : IDisposable
    {
        private readonly AvroSerializer<T> _avroSerializer;
        private readonly KafKaProducer _kafKaProducer;
        private readonly CachedSchemaRegistry<Schema> _cachedSchemaRegistry;
        private readonly string _topic;

        public KafkaAvroNet(string registryUrl, Dictionary<string, object> configList, string topic, SchemaProviderType registryType, EventHandler<Error> cbOnError = null)
        {
            _topic = topic;
            _cachedSchemaRegistry =  GetSchemaProvider(registryType,registryUrl);
          
            _kafKaProducer = cbOnError != null ? new KafKaProducer(configList, cbOnError) : new KafKaProducer(configList);
            var schemaMetadata = _cachedSchemaRegistry.GetSchema(new Schema() { Id = String.Empty, Topic = topic });
            _avroSerializer = new AvroSerializer<T>(new SerializationContext() { Topic = schemaMetadata.Subject, SchemaId = schemaMetadata.Id, SchemaString = schemaMetadata.SchemaString });
        }

        private CachedSchemaRegistry<Schema> GetSchemaProvider(SchemaProviderType registryType, string registryUrl)
        {
            switch(registryType){
                case SchemaProviderType.SchemaRegistry:
                    return  new CachedSchemaRegistry<Schema>(new SchemaRegistryProvider(registryUrl));
                case SchemaProviderType.SchemaRepo:
                    return new CachedSchemaRegistry<Schema>(new SchemaRepoProvider(registryUrl));
                 default:
                    throw new ArgumentException("Invalid registry type");
            }
        }

        public void WriteToKafka(T payload, Action<Task> deliveryCallback=null)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }
            _kafKaProducer.Write(_avroSerializer.Serialize(payload), _topic, deliveryCallback);

        }

        public void WriteToKafka(List<T> model, Action<Task> deliveryCallback = null)

        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var serializedArrayList = _avroSerializer.Serialize(model);
            _kafKaProducer.Write(serializedArrayList, _topic, deliveryCallback);

        }

        public void Dispose()
        {
            _kafKaProducer.Dispose();
        }
    }
    }

