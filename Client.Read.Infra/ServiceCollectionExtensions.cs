using Client.Read.App.UseCases;
using Client.Read.Infra.Gateways.Account;
using Client.Read.Infra.Gateways.Budget;
using Client.Read.Infra.Gateways.Categorization;
using Client.Read.Infra.Gateways.Category;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Read.Infra;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientReadInfra(this IServiceCollection services)
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
            .AddScoped<CategorizationSuggestions>()
            .AddScoped<BudgetSummaries>();
    }

    private static IServiceCollection AddGateways(this IServiceCollection services)
    {
        return services
            .AddScoped<IAccountGateway, HttpAccountGateway>()
            .AddScoped<ICategoryGateway, HttpCategoryGateway>()
            .AddScoped<ICategorizationGateway, HttpCategorizationGateway>()
            .AddScoped<IBudgetGateway, HttpBudgetGateway>();
    }
}