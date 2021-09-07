using System.Collections.Generic;
using Core.Application.Services;
using System.Linq.Dynamic.Core;
using System.Linq;
using System;

namespace Core.Application.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy, Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (mappingDictionary is null)
            {
                throw new ArgumentNullException(nameof(mappingDictionary));
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByString = string.Empty;

            var orderByAfterSplit = orderBy.Split(',');

            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                var trimmedOrderByClause = orderByClause.Trim();

                bool isDescending = trimmedOrderByClause.EndsWith(" desc");

                // remove " asc" or " desc"
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(' ');
                var propertyName = indexOfFirstSpace == -1 ? trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                if(!mappingDictionary.ContainsKey(propertyName))
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");

                var propertyMappingValue = mappingDictionary[propertyName];

                if (propertyMappingValue is null)
                    throw new ArgumentException(nameof(propertyMappingValue));

                foreach (var destinationProperty in propertyMappingValue.DestinationProperties)
                {
                    if(propertyMappingValue.Revert)
                        isDescending ^= true;

                    orderByString = orderByString +
                       (string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ", ")
                       + destinationProperty
                       + (isDescending ? " descending" : " ascending");
                }
            }

            return source.OrderBy(orderByString);
        }
    }
}
