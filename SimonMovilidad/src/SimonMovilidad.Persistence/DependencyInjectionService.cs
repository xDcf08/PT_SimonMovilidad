using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimonMovilidad.Application.Abstractions.Data;
using SimonMovilidad.Application.Abstractions.PasswordHash;
using SimonMovilidad.Domain.Abstractions;
using SimonMovilidad.Domain.Repository;
using SimonMovilidad.Persistence.Data;
using SimonMovilidad.Persistence.Hasher;
using SimonMovilidad.Persistence.Repositories;

namespace SimonMovilidad.Persistence
{
    public static class DependencyInjectionService
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseNpgsql(connectionString!);
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IAlertRepository, AlertRepository>();
            services.AddScoped<ISensorReadingRepository, SensorReadingRepository>();

            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.AddScoped<IUnitOfWork>( sp => 
                sp.GetRequiredService<ApplicationDbContext>());

            services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString!));

            return services;
        }
    }
}
