using System.Collections.Generic;
using Core.Domain.Entities;
using System.Linq;
using System;
using Core.Application.Abstractions;
using Core.Application.MapperProfiles;

namespace ToDoApi.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _userPropertyMappings = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Id", new(new List<string>() { "Id" }) },
            { "Username", new(new List<string>() { "Username" }) },
            { "Name", new(new List<string>() { "UserProfile.FirstName", "UserProfile.LastName" }) },
            { "Email", new(new List<string>() { "Email" }) },
            { "PhoneNumber", new(new List<string>() { "Username" }) },
        };

        private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<AppUser>(_userPropertyMappings));
        }

        public bool ValidMappingExistsFor<TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
                return true;

            var fieldsAfterSplit = fields.Split(',');

            foreach (var field in fieldsAfterSplit)
            {
                var trimmedField = field.Trim();

                // remove everything after the first " "
                var indexOfFirstSpace = trimmedField.IndexOf(' ');
                var propertyName = indexOfFirstSpace == -1 ? trimmedField : trimmedField.Remove(indexOfFirstSpace);

                if (!propertyMapping.ContainsKey(propertyName))
                    return false;
            }
            return true;
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TDestination>()
        {
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First().MappingDictionary;
            }
            else
            {
                throw new Exception($"Cannot find exact property mapping instance for <, {typeof(TDestination)}>");
            }
        }

        Dictionary<string, PropertyMappingValue> IPropertyMappingService.GetPropertyMapping<TDestination>()
        {
            throw new NotImplementedException();
        }
    }
}