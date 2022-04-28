using Microsoft.Extensions.DependencyInjection;
using Core.Application.Abstractions;
using ToDoApi.Services;
using System.Reflection;

namespace ToDoApi
{
    public static class ToDoApiDependencyInjectionn
    {
        public static IServiceCollection AddToDoApiServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddTransient<IPropertyChecker, PropertyChecker>();
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            services.AddAutoMapper(assembly);

            return services;
        }
    }
}
