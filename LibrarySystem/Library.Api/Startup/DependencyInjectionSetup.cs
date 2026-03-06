using Library.Api.Middleware;
using Library.Shared.Contracts.Inventory.V1;

namespace Library.Api.Startup
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services,IConfiguration configuration)
        {
            // register layer level DIs 
            services.AddGrpcClient<InventoryService.InventoryServiceClient>(options =>
            {
                options.Address = new Uri(configuration["GrpcSettings:BackendUrl"] ?? "localhost:5000");
            });

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            return services;
        }
    }
}
