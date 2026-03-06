using Library.Api.Middleware;
using Library.Shared.Contracts.Inventory.V1;
using Library.Shared.Contracts.UserActivity.V1;
using Library.Shared.Contracts.Recommendation.V1;

namespace Library.Api.Startup
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services,IConfiguration configuration)
        {
            var grpcBackendUrl = configuration["GrpcSettings:BackendUrl"] ?? "http://localhost:5000";

            services.AddGrpcClient<InventoryService.InventoryServiceClient>(options =>
            {
                options.Address = new Uri(grpcBackendUrl);
            });

            services.AddGrpcClient<UserActivityService.UserActivityServiceClient>(options =>
            {
                options.Address = new Uri(grpcBackendUrl);
            });

            services.AddGrpcClient<RecommendationService.RecommendationServiceClient>(options =>
            {
                options.Address = new Uri(grpcBackendUrl);
            });

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            return services;
        }
    }
}
