using Microsoft.Extensions.DependencyInjection;
using Read.App.UseCases;
using Read.Infra.DataSources.AccountDetails;
using Read.Infra.DataSources.AccountSummaries;
using Read.Infra.DataSources.BudgetSummaries;
using Read.Infra.DataSources.CategoriesWithKeywords;
using Read.Infra.DataSources.CategorySummaries;
using Read.Infra.DataSources.TransactionsOfMonth;
using Read.Infra.DataSources.TransactionsToCategorize;

namespace Read.Infra;

public static class ServiceCollectionExtensions
{
    public static void AddReadInfra(this IServiceCollection services)
    {
        services
            .AddUseCases()
            .AddInfrastructureAdapters();
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

    private static void AddInfrastructureAdapters(this IServiceCollection services)
    {
        services
            .AddScoped<IAccountSummariesDataSource, RepositoryAccountSummariesDataSource>()
            .AddScoped<IAccountDetailsDataSource, RepositoryAccountDetailsDataSource>()
            .AddScoped<ITransactionsOfMonthDataSource, RepositoryTransactionsOfMonthDataSource>()
            .AddScoped<ICategorySummariesDataSource, RepositoryCategorySummariesDataSource>()
            .AddScoped<ICategoriesWithKeywordsDataSource, RepositoryCategoriesWithKeywordsDataSource>()
            .AddScoped<ITransactionsToCategorizeDataSource, RepositoryTransactionsToCategorizeDataSource>()
            .AddScoped<IBudgetSummariesDataSource, RepositoryBudgetSummariesDataSource>();
    }
}