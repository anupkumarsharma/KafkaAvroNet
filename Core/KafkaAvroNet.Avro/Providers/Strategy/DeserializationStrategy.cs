using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Avro;
using Avro.Generic;

namespace KafkaAvroNet.Avro.Providers.Strategy
{
    /// <summary>
    /// Deserialization strategy.
    /// </summary>
    public class DeserializationStrategy
    {
        Dictionary<Type, IStrategy> _strategyList;
        public DeserializationStrategy()
        {
            _strategyList = new Dictionary<Type, IStrategy>();
            _strategyList.Add(typeof(Array), new ArrayStrategy());
        }

        /// <summary>
        /// Deserialize the specified genericRecord, toTransform and schema.
        /// </summary>
        /// <returns>The deserialize.</returns>
        /// <param name="genericRecord">Generic record.</param>
        /// <param name="toTransform">To transform.</param>
        /// <param name="schema">Schema.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T Deserialize<T>(GenericRecord genericRecord, ref T toTransform, Schema schema)
        {
            var propertyList = toTransform.GetType().GetProperties().ToList();

            foreach (var s in ((RecordSchema)schema).Fields)
            {
                object sourceValue;
                genericRecord.TryGetValue(s.Name, out sourceValue);
                var typeInfo = toTransform.GetType()
                    .GetProperty(s.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                // possible null exception handle it 
                if (typeInfo == null)
                {
                    throw new InvalidCastException("Source and Target model mismatch. Please check if the field exists in source");
                }
                var dType = typeInfo.PropertyType;
                var typeProperty = propertyList.First(p => p.Name.ToLower() == s.Name.ToLower());
                var targetValue = UseStrategy(typeProperty, sourceValue);
                if (targetValue == null)
                {
                    typeInfo.SetValue(toTransform, s.DefaultValue);
                }
                else
                {
                    typeInfo.SetValue(toTransform, targetValue);
                }

            }
            return toTransform;
        }

        /// <summary>
        /// Uses the strategy.
        /// </summary>
        /// <returns>The strategy.</returns>
        /// <param name="typeProperty">Type property.</param>
        /// <param name="source">Source.</param>
        private object UseStrategy(PropertyInfo typeProperty, object source)
        {
            if (typeProperty.PropertyType.IsPrimitive || typeProperty.PropertyType.Equals(typeof(string)))
            {
                return source;
            }
            if (_strategyList.ContainsKey(typeProperty.PropertyType.BaseType))
            {
                return _strategyList[typeProperty.PropertyType.BaseType].DeserializeToObject(source, typeProperty);
            }

            throw new NotImplementedException("unable to map types with any supported type");

        }
    }
}
