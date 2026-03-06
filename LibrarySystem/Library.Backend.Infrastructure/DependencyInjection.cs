using Library.Backend.Application.Interfaces;
using Library.Backend.Infrastructure.Persistence;
using Library.Backend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("LibraryDb")
            ?? "Data Source=library.db";

        services.AddDbContext<LibraryDbContext>(options => options.UseSqlite(connectionString));

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILoanTransactionRepository, LoanTransactionRepository>();
        services.AddScoped<ILibraryAnalyticsRepository, LibraryAnalyticsRepository>();

        return services;
    }
}
