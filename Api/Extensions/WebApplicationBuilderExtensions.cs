using Read.Api;
using Shared.Infra;
using Write.Api;

namespace Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddSwaggerDependencies()
            .AddBlazorDependencies()
            .AddSharedInfra()
            .AddWrite()
            .AddRead();
    }

    private static IServiceCollection AddSwaggerDependencies(this IServiceCollection services)
    {
        return services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();
    }

    private static IServiceCollection AddBlazorDependencies(this IServiceCollection services)
    {
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddControllers();
        services.AddRazorPages();

        return services;
    }
}