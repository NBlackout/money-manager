using Microsoft.Extensions.DependencyInjection;
using Read.App.UseCases;
using Read.Infra.DataSources;

namespace Read.Infra;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServerReadInfra(this IServiceCollection services) =>
        services.AddDataSources();

    private static IServiceCollection AddDataSources(this IServiceCollection services)
    {
        return services
            .AddScoped<IAccountSummariesDataSource, InMemoryAccountSummariesDataSource>()
            .AddScoped<IAccountDetailsDataSource, InMemoryAccountDetailsDataSource>()
            .AddScoped<ITransactionsOfMonthDataSource, InMemoryTransactionsOfMonthDataSource>()
            .AddScoped<ICategorySummariesDataSource, InMemoryCategorySummariesDataSource>()
            .AddScoped<ICategoriesWithKeywordsDataSource, InMemoryCategoriesWithKeywordsDataSource>()
            .AddScoped<ITransactionsToCategorizeDataSource, InMemoryTransactionsToCategorizeDataSource>()
            .AddScoped<IBudgetSummariesDataSource, InMemoryBudgetSummariesDataSource>();
    }
}