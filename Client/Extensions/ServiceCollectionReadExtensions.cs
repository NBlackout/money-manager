using Client.Read.Infra;

namespace Client.Extensions;

public static class ServiceCollectionReadExtensions
{
    public static IServiceCollection AddRead(this IServiceCollection services) =>
        services.AddUseCases().AddClientReadInfra();

    private static IServiceCollection AddUseCases(this IServiceCollection services) =>
        services
            .AddScoped<AccountSummaries>()
            .AddScoped<AccountDetails>()
            .AddScoped<TransactionsOfMonth>()
            .AddScoped<CategorySummaries>()
            .AddScoped<CategorizationSuggestions>()
            .AddScoped<BudgetSummaries>()
            .AddScoped<SlidingBalances>();
}