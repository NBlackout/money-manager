using App.Read.Ports;
using Infra.Read.DataSources;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Read;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddReadInfra(this IServiceCollection services) =>
        services.AddDataSources().AddSingleton<ICategoryExporter, CsvCategoryExporter>();

    private static IServiceCollection AddDataSources(this IServiceCollection services) =>
        services
            .AddSingleton<IAccountSummariesDataSource, InMemoryAccountSummariesDataSource>()
            .AddSingleton<IAccountDetailsDataSource, InMemoryAccountDetailsDataSource>()
            .AddSingleton<ITransactionsOfMonthDataSource, InMemoryTransactionsOfMonthDataSource>()
            .AddSingleton<ICategorySummariesDataSource, InMemoryCategorySummariesDataSource>()
            .AddSingleton<ICategoriesWithKeywordsDataSource, InMemoryCategoriesWithKeywordsDataSource>()
            .AddSingleton<ITransactionsToCategorizeDataSource, InMemoryTransactionsToCategorizeDataSource>()
            .AddSingleton<IBudgetSummariesDataSource, InMemoryBudgetSummariesDataSource>()
            .AddSingleton<ISlidingBalancesDataSource, InMemorySlidingBalancesDataSource>();
}