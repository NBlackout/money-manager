using Client.Read.App.Ports;
using Client.Read.Infra.Gateways.Account;
using Client.Read.Infra.Gateways.Categorization;
using Client.Read.Infra.Gateways.Category;

namespace Client.Extensions;

public static class ServiceCollectionReadExtensions
{
    public static IServiceCollection AddReadDependencies(this IServiceCollection services)
    {
        return services
            .AddUseCases()
            .AddGateways();
    }

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<AccountSummaries>()
            .AddScoped<AccountDetails>()
            .AddScoped<TransactionsOfMonth>()
            .AddScoped<CategorySummaries>()
            .AddScoped<CategorizationSuggestions>();
    }

    private static IServiceCollection AddGateways(this IServiceCollection services)
    {
        return services
            .AddScoped<IAccountGateway, HttpAccountGateway>()
            .AddScoped<ICategoryGateway, HttpCategoryGateway>()
            .AddScoped<ICategorizationGateway, HttpCategorizationGateway>();
    }
}