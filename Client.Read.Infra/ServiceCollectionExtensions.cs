using Client.Read.Infra.Gateways;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Read.Infra;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientReadInfra(this IServiceCollection services) =>
        services.AddGateways();

    private static IServiceCollection AddGateways(this IServiceCollection services)
    {
        return services
            .AddScoped<IAccountGateway, HttpAccountGateway>()
            .AddScoped<ICategoryGateway, HttpCategoryGateway>()
            .AddScoped<ICategorizationGateway, HttpCategorizationGateway>()
            .AddScoped<IBudgetGateway, HttpBudgetGateway>()
            .AddScoped<IDashboardGateway, StubbedDashboardGateway>();
    }
}

internal class StubbedDashboardGateway : IDashboardGateway
{
    public Task<SlidingAccountBalancesPresentation> SlidingAccountBalances()
    {
        return Task.FromResult(
            new SlidingAccountBalancesPresentation(
                [
                    new AccountBalancesByDatePresentation(
                        DateOnly.Parse("2025-01-01"),
                        [
                            new AccountBalancePresentation("Checking", 1280.05m),
                            new AccountBalancePresentation("Savings", 50m)
                        ]
                    ),
                    new AccountBalancesByDatePresentation(
                        DateOnly.Parse("2025-02-01"),
                        [
                            new AccountBalancePresentation("Checking", 1838.41m),
                            new AccountBalancePresentation("Savings", 100m)
                        ]
                    ),
                    new AccountBalancesByDatePresentation(
                        DateOnly.Parse("2025-03-01"),
                        [
                            new AccountBalancePresentation("Checking", 1915.00m),
                            new AccountBalancePresentation("Savings", 150m)
                        ]
                    ),
                    new AccountBalancesByDatePresentation(
                        DateOnly.Parse("2025-04-01"),
                        [
                            new AccountBalancePresentation("Checking", 1837.12m),
                            new AccountBalancePresentation("Savings", 200m)
                        ]
                    ),
                    new AccountBalancesByDatePresentation(
                        DateOnly.Parse("2025-05-01"),
                        [
                            new AccountBalancePresentation("Checking", 1898.72m),
                            new AccountBalancePresentation("Savings", 250m)
                        ]
                    )
                ]
            )
        );
    }
}