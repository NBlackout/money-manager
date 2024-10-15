using Microsoft.Extensions.DependencyInjection;
using Read.App.UseCases;
using Read.Infra.DataSources;

namespace Read.Infra;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServerReadInfra(this IServiceCollection services)
    {
        return services
            .AddUseCases()
            .AddDataSources();
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