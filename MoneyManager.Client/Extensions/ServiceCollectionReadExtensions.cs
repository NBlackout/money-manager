using MoneyManager.Client.Read.Application.Ports;
using MoneyManager.Client.Read.Infrastructure.Gateways.Account;
using MoneyManager.Client.Read.Infrastructure.Gateways.Category;

namespace MoneyManager.Client.Extensions;

public static class ServiceCollectionReadExtensions
{
    public static IServiceCollection AddReadDependencies(this IServiceCollection services)
    {
        return services
            .AddUseCases()
            .AddScoped<IAccountGateway, HttpAccountGateway>()
            .AddScoped<ICategoryGateway, HttpCategoryGateway>();
    }

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<AccountSummaries>()
            .AddScoped<AccountDetails>()
            .AddScoped<TransactionsOfMonth>()
            .AddScoped<CategorySummaries>()
            .AddScoped<AssignTransactionCategory>();
    }
}