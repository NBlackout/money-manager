using Api.Configuration;
using Microsoft.Extensions.Options;
using Read.Api;
using Shared.Infra;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Write.Api;

namespace Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void Configure(this WebApplicationBuilder builder) =>
        builder.Services.AddSwaggerDependencies().AddBlazorDependencies().AddSharedInfra().AddWrite().AddRead();

    private static IServiceCollection AddSwaggerDependencies(this IServiceCollection services) =>
        services
            .AddSwaggerGen()
            .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwagger>()
            .AddTransient<IConfigureOptions<SwaggerOptions>, ConfigureSwagger>()
            .AddTransient<IConfigureOptions<SwaggerUIOptions>, ConfigureSwagger>();

    private static IServiceCollection AddBlazorDependencies(this IServiceCollection services)
    {
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddControllers();
        services.AddRazorPages();

        return services;
    }
}