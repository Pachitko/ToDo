using System.Collections.Generic;
using System.Reflection;
using System.Dynamic;
using System;

namespace Core.Application.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<T>(this IEnumerable<T> source, string fields)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var propertyInfos = new List<PropertyInfo>();
         
            if(string.IsNullOrWhiteSpace(fields))
            {
                var publicProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                propertyInfos.AddRange(publicProperties);
            }
            else
            {
                var fieldsAfterSplit = fields.Split(',');

                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();
                    var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo is null)
                        throw new ArgumentException($"Propery {propertyName} wasn't found on {typeof(T)}");

                    propertyInfos.Add(propertyInfo);
                }
            }

            var expandoObjects = new List<ExpandoObject>();

            foreach (T sourceObject in source)
            {
                var dataShapedObject = new ExpandoObject();

                foreach (var propertyInfo in propertyInfos)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);
                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                }

                expandoObjects.Add(dataShapedObject);
            }

            return expandoObjects;
        }
    }
}
