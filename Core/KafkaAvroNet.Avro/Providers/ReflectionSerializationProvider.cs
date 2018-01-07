using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Avro;
using Avro.Generic;
using Avro.IO;
using Avro.Specific;
using KafkaAvroNet.Avro.Providers.Strategy;
using Encoder = Avro.IO.Encoder;

namespace KafkaAvroNet.Avro.Providers
{
    /// <summary>
    /// Reflection based serialization. Leverges the Avro library to convert the data type into avro stream
    /// </summary>
    public class ReflectionSerializationProvider
    {

        private readonly DeserializationStrategy _deserializationStrategy;

        public ReflectionSerializationProvider()
        {
            _deserializationStrategy = new DeserializationStrategy(); 
        }
        /// <summary>
        /// Comverts the model into avro serialized byte stream. This assumes that the schema and the 
        /// model types are same, else it will throw exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toTransform"></param>
        /// <param name="schema"></param>
        /// <param name="schemaString"></param>
        /// <returns></returns>
        public byte[] Format<T>(T toTransform, Schema schema, string schemaString)
        {
            byte[] serializedModel;
            var datumWriter = new GenericDatumWriter<GenericRecord>(schema);
            var schemap = Schema.Parse(schemaString) as RecordSchema;
            using (var memoryStream = new MemoryStream())
            {
                Encoder encoder = new BinaryEncoder(memoryStream);
                datumWriter.Write(GetGenericRecord(toTransform, schema, schemaString), encoder);
                serializedModel = memoryStream.ToArray();
            }

            return serializedModel;
        }
       
        /// <summary>
        /// Format the specified stream, writerSchema, readerSchema and reflectedObject.
        /// </summary>
        /// <returns>The format.</returns>
        /// <param name="stream">Stream.</param>
        /// <param name="writerSchema">Writer schema.</param>
        /// <param name="readerSchema">Reader schema.</param>
        /// <param name="reflectedObject">Reflected object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void Format<T>(Stream stream, Schema writerSchema, Schema readerSchema, ref T reflectedObject) where T : new()
        {

            var datumReader = new GenericDatumReader<GenericRecord>(writerSchema, readerSchema);
            var decoder = new BinaryDecoder(stream);
            GenericRecord genericRecord = new GenericRecord(readerSchema as RecordSchema);
            var result1 = datumReader.Read(genericRecord, decoder);
            _deserializationStrategy.Deserialize(genericRecord,ref reflectedObject,writerSchema);
        }


        /// <summary>
        /// Gets the generic record.
        /// </summary>
        /// <returns>The generic record.</returns>
        /// <param name="toTransform">To transform.</param>
        /// <param name="schema">Schema.</param>
        /// <param name="schemaString">Schema string.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private GenericRecord GetGenericRecord<T>(T toTransform, Schema schema, string schemaString)
        {
            var schemap = Schema.Parse(schemaString) as RecordSchema;
            GenericRecord genericRecord = new GenericRecord(schemap);
            var propertyList = toTransform.GetType().GetProperties().ToList();

            foreach (var s in ((RecordSchema)schema).Fields)
            {
                var typeInfo = toTransform.GetType()
                    .GetProperty(s.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                // possible null exception handle it 
                if (typeInfo == null)
                {
                    throw new InvalidCastException("Source and Target model mismatch. Please check if the field exists in source");
                }
                var dType = typeInfo.PropertyType;
                var dTypeValue = typeInfo.GetValue(toTransform);
                var typeProperty = propertyList.First(p => p.Name.ToLower() == s.Name.ToLower());
                if (typeProperty != null)
                {
                    AddType(dType, dTypeValue, s, genericRecord);
                }
            }
            return genericRecord;

        }

        /// <summary>
        /// Adds the type.
        /// </summary>
        /// <param name="dType">D type.</param>
        /// <param name="value">Value.</param>
        /// <param name="s">S.</param>
        /// <param name="genericRecord">Generic record.</param>
        private void AddType(Type dType, object value, Field s, GenericRecord genericRecord)
        {

            if (dType.IsGenericType && dType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type t = Nullable.GetUnderlyingType(dType) ?? dType.GetType();
                object safeValue = (value == null) ? null : Convert.ChangeType(value, t);
                genericRecord.Add(s.Name, safeValue);
            }
            else
            {
                genericRecord.Add(s.Name, value);
            }


        }


    }
}