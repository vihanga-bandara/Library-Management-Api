using Microsoft.OpenApi;

namespace Library.Api.Startup
{
    public static class OpenApiConfiguration
    {
        public static WebApplicationBuilder ConfigureOpenApiConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info.Version = "1";
                    document.Info.Title = "Library Management System";
                    document.Info.Description = "This API fronts a ASP Net Web API that interacts with a gRPC server along with layering based on clean architecture principles";
                    document.Info.Contact = new OpenApiContact
                    {
                        Name = "Vihanga Bandara",
                        Url = new Uri("https://vihangabandara.com")
                    };
                    return Task.CompletedTask;
                });
            });

            return builder;
        }
    }
}
