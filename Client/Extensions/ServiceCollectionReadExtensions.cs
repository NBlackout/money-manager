using Client.Read.Infra.Gateways.Account;
using Client.Read.Infra.Gateways.Category;
using Client.Write.App.Ports;
using Client.Write.Infra.Gateways.Transaction;
using IAccountGateway = Client.Read.App.Ports.IAccountGateway;
using ICategoryGateway = Client.Read.App.Ports.ICategoryGateway;

namespace Client.Extensions;

public static class ServiceCollectionReadExtensions
{
    public static IServiceCollection AddReadDependencies(this IServiceCollection services)
    {
        return services
            .AddUseCases()
            .AddScoped<IAccountGateway, HttpAccountGateway>()
            .AddScoped<ICategoryGateway, HttpCategoryGateway>()
            .AddScoped<ITransactionGateway, HttpTransactionGateway>();
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