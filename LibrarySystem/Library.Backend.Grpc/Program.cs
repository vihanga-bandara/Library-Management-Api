using Library.Backend.Grpc;
using Library.Backend.Grpc.Services;
using Library.Backend.Application;
using Library.Backend.Infrastructure;
using Library.Backend.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<GlobalExceptionInterceptor>();
});
builder.Services.AddSingleton<GlobalExceptionInterceptor>();
builder.Services
    .AddApplicationServices()
    .AddInfrastructure(builder.Configuration, builder.Environment.IsDevelopment());

var app = builder.Build();

app.MapGrpcService<InventoryGrpcService>();
app.MapGrpcService<UserActivityGrpcService>();
app.MapGrpcService<RecommendationGrpcService>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Get the DbContext from Dependency Injection
        var context = services.GetRequiredService<LibraryDbContext>();

        // Run the seeder
        DbSeeder.Initialize(context);
    }
    catch (Exception ex)
    {
        // Log errors if seeding fails
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the database.");
    }
}

app.Run();
