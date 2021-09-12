using Microsoft.Extensions.DependencyInjection;
using Core.Application.PipelineBehaviors;
using FluentValidation;
using MediatR;

namespace Core.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = typeof(ApplicationDependencyInjection).Assembly;

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddMediatR(assembly);
            services.AddValidatorsFromAssembly(assembly);
            services.AddAutoMapper(assembly);

            return services;
        }
    }
}
