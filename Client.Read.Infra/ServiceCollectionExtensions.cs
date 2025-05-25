using Client.Read.Infra.Gateways;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Read.Infra;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientReadInfra(this IServiceCollection services) =>
        services.AddGateways();

    private static IServiceCollection AddGateways(this IServiceCollection services)
    {
        return services
            .AddScoped<IAccountGateway, HttpAccountGateway>()
            .AddScoped<ICategoryGateway, HttpCategoryGateway>()
            .AddScoped<ICategorizationGateway, HttpCategorizationGateway>()
            .AddScoped<IBudgetGateway, HttpBudgetGateway>()
            .AddScoped<IDashboardGateway, HttpDashboardGateway>();
    }
}