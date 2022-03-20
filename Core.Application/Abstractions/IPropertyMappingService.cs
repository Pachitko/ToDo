using Core.Application.MapperProfiles;
using System.Collections.Generic;

namespace Core.Application.Abstractions
{
    public interface IPropertyMappingService
    {
        // todo fix TSource -> TDestination relation.
        // AppUserDto doesn't exist at the Core.Application layer
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TDestination>();
        bool ValidMappingExistsFor<TDestination>(string fields);
    }
}
