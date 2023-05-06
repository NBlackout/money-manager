using MoneyManager.Read.Api.Extensions;
using MoneyManager.Write.Api.Extensions;

namespace MoneyManager.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddSwaggerDependencies()
            .AddBlazorDependencies()
            .AddWriteDependencies()
            .AddReadDependencies();
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
        services.AddControllersWithViews();
        services.AddRazorPages();

        return services;
    }
}