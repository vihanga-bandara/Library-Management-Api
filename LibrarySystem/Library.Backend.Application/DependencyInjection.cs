using Library.Backend.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Backend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IInventoryInsightsService, InventoryInsightsService>();
        services.AddScoped<IRecommendationService, RecommendationService>();
        services.AddScoped<IUserActivityService, UserActivityService>();

        return services;
    }
}
