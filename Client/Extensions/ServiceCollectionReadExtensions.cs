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
            .AddSingleton<CategorizationSuggestions>()
            .AddSingleton<CategoriesExport>()
            .AddSingleton<BudgetSummaries>()
            .AddSingleton<MonthlyPerformance>()
            .AddSingleton<PerformanceForecast>();
}