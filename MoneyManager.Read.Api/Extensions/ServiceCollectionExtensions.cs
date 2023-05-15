using MoneyManager.Read.Infrastructure.DataSources.AccountDetails;
using MoneyManager.Read.Infrastructure.DataSources.AccountSummaries;
using MoneyManager.Read.Infrastructure.DataSources.CategorySummaries;
using MoneyManager.Read.Infrastructure.DataSources.TransactionsOfMonth;

namespace MoneyManager.Read.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddReadDependencies(this IServiceCollection services) =>
        services.AddUseCases().AddInfrastructureAdapters();

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<AccountSummaries>()
            .AddScoped<AccountDetails>()
            .AddScoped<TransactionsOfMonth>()
            .AddScoped<CategorySummaries>();
    }

    private static IServiceCollection AddInfrastructureAdapters(this IServiceCollection services)
    {
        return services
            .AddScoped<IAccountSummariesDataSource, RepositoryAccountSummariesDataSource>()
            .AddScoped<IAccountDetailsDataSource, RepositoryAccountDetailsDataSource>()
            .AddScoped<ITransactionsOfMonthDataSource, RepositoryTransactionsOfMonthDataSource>()
            .AddScoped<ICategorySummariesDataSource, RepositoryCategorySummariesDataSource>();
    }
}