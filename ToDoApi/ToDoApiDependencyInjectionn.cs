using Microsoft.Extensions.DependencyInjection;
using Core.Application.Services;
using ToDoApi.Services;

namespace Core.Application
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
