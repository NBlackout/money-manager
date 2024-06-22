using Read.Infra.DataSources;
using Read.Infra.DataSources.AccountDetails;
using Read.Infra.DataSources.AccountSummaries;
using Read.Infra.DataSources.CategoriesWithPattern;
using Read.Infra.DataSources.CategorySummaries;
using Read.Infra.DataSources.TransactionsOfMonth;
using Read.Infra.DataSources.TransactionsToCategorize;

namespace Read.Api.Extensions;

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
            .AddScoped<CategorySummaries>()
            .AddScoped<CategorizationSuggestions>();
    }

    private static IServiceCollection AddInfrastructureAdapters(this IServiceCollection services)
    {
        return services
            .AddScoped<IAccountSummariesDataSource, RepositoryAccountSummariesDataSource>()
            .AddScoped<IAccountDetailsDataSource, RepositoryAccountDetailsDataSource>()
            .AddScoped<ITransactionsOfMonthDataSource, RepositoryTransactionsOfMonthDataSource>()
            .AddScoped<ICategorySummariesDataSource, RepositoryCategorySummariesDataSource>()
            .AddScoped<ICategoriesWithPatternDataSource, RepositoryCategoriesWithPatternDataSource>()
            .AddScoped<ITransactionsToCategorizeDataSource, RepositoryTransactionsToCategorizeDataSource>();
    }
}