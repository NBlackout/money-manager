using Microsoft.Extensions.DependencyInjection;
using Read.Infra;

namespace Read.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRead(this IServiceCollection services) =>
        services.AddUseCases().AddServerReadInfra();

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