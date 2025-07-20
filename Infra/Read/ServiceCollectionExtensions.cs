using App.Read.Ports;
using Infra.Read.DataSources;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Read;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddReadInfra(this IServiceCollection services) =>
        services.AddDataSources();

    private static IServiceCollection AddDataSources(this IServiceCollection services) =>
        services
            .AddScoped<IAccountSummariesDataSource, InMemoryAccountSummariesDataSource>()
            .AddScoped<IAccountDetailsDataSource, InMemoryAccountDetailsDataSource>()
            .AddScoped<ITransactionsOfMonthDataSource, InMemoryTransactionsOfMonthDataSource>()
            .AddScoped<ICategorySummariesDataSource, InMemoryCategorySummariesDataSource>()
            .AddScoped<ICategoriesWithKeywordsDataSource, InMemoryCategoriesWithKeywordsDataSource>()
            .AddScoped<ITransactionsToCategorizeDataSource, InMemoryTransactionsToCategorizeDataSource>()
            .AddScoped<IBudgetSummariesDataSource, InMemoryBudgetSummariesDataSource>()
            .AddScoped<ISlidingBalancesDataSource, InMemorySlidingBalancesDataSource>();
}