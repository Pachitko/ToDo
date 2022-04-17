using Microsoft.Extensions.DependencyInjection;
using Core.Application.Abstractions;
using ToDoApi.Services;

namespace ToDoApi
{
    public static class ToDoApiDependencyInjectionn
    {
        public static IServiceCollection AddToDoApiServices(this IServiceCollection services)
        {
            services.AddTransient<IPropertyChecker, PropertyChecker>();
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            services.AddAutoMapper(typeof(ToDoApiDependencyInjectionn).Assembly);

            return services;
        }
    }
}
