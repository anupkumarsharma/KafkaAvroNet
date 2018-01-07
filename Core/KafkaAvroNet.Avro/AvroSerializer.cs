using System;
using System.Collections.Generic;
using System.IO;
using KafkaAvroNet.Avro.Providers;


namespace KafkaAvroNet.Avro
{
    public class AvroSerializer<T>
    {
        private readonly SerializationContext _serializationContext;
        private readonly ReflectionSerializationProvider _reflectionSerialization;

        public AvroSerializer(SerializationContext serializationContext)
        {
            _serializationContext = serializationContext;
            _reflectionSerialization = new ReflectionSerializationProvider();
        }

        /// <summary>
        /// Serialize the Model into byte using reflection 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public byte[] Serialize(T obj)
        {
            if(obj == null){
                throw new ArgumentNullException(nameof(obj));
            }

            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    SerializeToAvro(writer, obj);
                    stream.Seek(0, SeekOrigin.Begin);
                    return stream.ToArray();
                }
            }
        }

        public List<byte[]> Serialize(List<T> listObj)
        {
            if (listObj == null)
            {
                throw new ArgumentNullException(nameof(listObj));
            }
            List<byte[]> listofPayload = new List<byte[]>();

            using (var stream = new MemoryStream())
            {
                foreach (var obj in listObj)
                {
                    var writer = new BinaryWriter(stream);
                    SerializeToAvro(writer, obj);
                    stream.Seek(0, SeekOrigin.Begin);
                    listofPayload.Add(stream.ToArray());
                }
            }
            return listofPayload;
        }

        /// <summary>
        /// Use the confluent format to assemble payload
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="obj"></param>
        private void SerializeToAvro(BinaryWriter writer, T obj)
        {
            writer.BaseStream.Position = 0;
            //write the magic byte
            writer.Write((byte)0x0);
            //write the schema id 

            var uintSchemaId = Convert.ToUInt32(_serializationContext.SchemaId);
            if (BitConverter.IsLittleEndian)
            {
                uintSchemaId = Helper.SwapEndianness(uintSchemaId);
            }
            Helper.AvroEncodeInt(writer,Convert.ToInt32(_serializationContext.SchemaId));
          //  writer.Write(uintSchemaId);
            //write the content 
            var serializedBytes = _reflectionSerialization.Format<T>(obj, _serializationContext.AvroSchema,
                _serializationContext.SchemaString);
            writer.Write(serializedBytes);
        }

    }
}
