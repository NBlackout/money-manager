using App.Read.UseCases;
using Infra.Read;

namespace Client.Extensions;

public static class ServiceCollectionReadExtensions
{
    public static IServiceCollection AddRead(this IServiceCollection services) =>
        services.AddUseCases().AddReadInfra();

    private static IServiceCollection AddUseCases(this IServiceCollection services) =>
        services
            .AddSingleton<AccountSummaries>()
            .AddSingleton<AccountDetails>()
            .AddSingleton<TransactionsOfMonth>()
            .AddSingleton<ExpectedTransactionSummaries>()
            .AddSingleton<CategorySummaries>()
            .AddSingleton<CategoriesExport>()
            .AddSingleton<CategorizationRuleSummaries>()
            .AddSingleton<CategorizationRulesExport>()
            .AddSingleton<CategorizationSuggestions>()
            .AddSingleton<BudgetSummaries>()
            .AddSingleton<MonthlyPerformance>()
            .AddSingleton<PerformanceForecast>();
}