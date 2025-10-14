using Microsoft.Extensions.DependencyInjection;
using SimonMovilidad.Application.Abstractions.Behaviors;
using FluentValidation;
using SimonMovilidad.Domain.Abstractions;
using SimonMovilidad.Application.Services;

namespace SimonMovilidad.Application
{
    public static class DependencyInjectionService
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddMediatR(opt =>
            {
                opt.RegisterServicesFromAssembly(typeof(DependencyInjectionService).Assembly);
                opt.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(DependencyInjectionService).Assembly);
            services.AddTransient<ISpeedCalculator, SpeedCalculator>();

            return services;
        }
    }
}
