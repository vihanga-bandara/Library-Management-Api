using Libary.Backend.Grpc.Services;
using Library.Backend.Application;
using Library.Backend.Infrastructure;
using Library.Backend.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services
    .AddApplicationServices()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<InventoryGrpcService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");


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
