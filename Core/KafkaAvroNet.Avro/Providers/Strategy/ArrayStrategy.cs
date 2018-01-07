using System;
using System.Reflection;

namespace KafkaAvroNet.Avro.Providers.Strategy
{
    /// <summary>
    /// Array strategy.
    /// </summary>
    public class ArrayStrategy :IStrategy
    {
       /// <summary>
       /// Deserializes to object.
       /// </summary>
       /// <returns>The to object.</returns>
       /// <param name="collection">Collection.</param>
       /// <param name="propInfo">Property info.</param>
        public object DeserializeToObject(object collection, PropertyInfo propInfo)
        {
            if (collection == null)
            {
                return null;
            }
            var array = collection as object[];
            var retu = propInfo.PropertyType.GetElementType();
            var arr = Array.ConvertAll(array, elem => Convert.ChangeType(elem, retu));
            var destinationArray = Array.CreateInstance(retu, array.Length);
            Array.Copy(arr, destinationArray, arr.Length);
            return destinationArray;
        }


    }
}
