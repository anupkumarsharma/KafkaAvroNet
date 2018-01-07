using System;
using System.IO;
using Avro;
using Avro.Generic;
using Avro.IO;
using Avro.Specific;
using KafkaAvroNet.Avro.Providers;

namespace KafkaAvroNet.Avro
{
    public class AvroDeserializer<T> where T : new()
    {
        ReflectionSerializationProvider _provider;
        private readonly SerializationContext _serializationContext;
        private Schema _readerSchema;
        public delegate Schema getSchema(int id);
        public AvroDeserializer(SerializationContext serializationContext)
        {
            _serializationContext = serializationContext;
            _readerSchema = Schema.Parse(_serializationContext.SchemaString);
            _provider = new ReflectionSerializationProvider();
        }


        public T Deserialize(Stream stream ,getSchema getWriterSchemaDelegate) 
        {
            var poco = new T();
           // var stream = new MemoryStream(payload);
            BinaryReader reader = new BinaryReader(stream);
            reader.BaseStream.Position = 0;
            //write the magic byte
            var magicByte = reader.ReadByte();
            // Read Schema Id 

            var schemaID = Helper.AvroDecodeInt(reader);  // reader.ReadUInt32();
            if(getWriterSchemaDelegate==null)
                 _provider.Format<T>(stream,_readerSchema,_readerSchema,ref poco);
            else
                _provider.Format<T>(stream, _readerSchema, getWriterSchemaDelegate(schemaID), ref poco); 
            reader.Close();
            return poco;

        }
    }

}
