using Library.Backend.Application.Interfaces;
using Library.Backend.Infrastructure.Persistence;
using Library.Backend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Library.Backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isDevelopment = false)
    {
        var connectionString = configuration.GetConnectionString("LibraryDb");

        services.AddDbContext<LibraryDbContext>(options =>
        {
            options.UseSqlServer(connectionString);

            if (isDevelopment)
            {
                options.LogTo(Console.WriteLine, LogLevel.Information);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        }
        );

        services.AddScoped<ILibraryAnalyticsRepository, LibraryAnalyticsRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
