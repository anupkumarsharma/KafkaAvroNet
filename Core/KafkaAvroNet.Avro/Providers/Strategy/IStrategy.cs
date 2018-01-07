using System;
using System.Reflection;

namespace KafkaAvroNet.Avro.Providers.Strategy
{
    /// <summary>
    /// Strategy.
    /// </summary>
    public interface IStrategy
    {
        object DeserializeToObject(object sourceObject, PropertyInfo propInfo);
    }
}
