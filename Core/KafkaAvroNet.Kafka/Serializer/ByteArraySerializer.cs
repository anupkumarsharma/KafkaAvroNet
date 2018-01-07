//using System;
//using System.Collections.Generic;
//using System.Text;
//using Confluent.Kafka.Serialization;

//namespace KafkaAvroNet.Kafka.Serializer
//{
//    public class ByteArraySerializer : ISerializer<byte[]>
//    {
       
//        public byte[] Serialize(byte[] data)
//        {
//            return data;
//        }


//        public IEnumerable<KeyValuePair<string, object>> Configure(IEnumerable<KeyValuePair<string, object>> config, bool isKey)
//            => config;
//    }
//}
