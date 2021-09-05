using Microsoft.Extensions.DependencyInjection;
using Core.Application.PipelineBehaviors;
using FluentValidation;
using MediatR;

namespace Core.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = typeof(ApplicationServicesRegistration).Assembly;

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>)); //todo: fix
            services.AddMediatR(assembly);
            services.AddValidatorsFromAssembly(assembly);
            services.AddAutoMapper(assembly);

            return services;
        }
    }
}
