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
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("LibraryDb")
            ?? "Data Source=library.db";

        services.AddDbContext<LibraryDbContext>(options =>
        {
            options.UseSqlite(connectionString);

            // check query optimization - delete later
            options.LogTo(Console.WriteLine, LogLevel.Information);
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }
        );

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILoanTransactionRepository, LoanTransactionRepository>();
        services.AddScoped<ILibraryAnalyticsRepository, LibraryAnalyticsRepository>();

        return services;
    }
}
