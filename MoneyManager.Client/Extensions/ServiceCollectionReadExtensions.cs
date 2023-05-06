using MoneyManager.Client.Read.Application.Ports;
using MoneyManager.Client.Read.Infrastructure.Gateways.AccountSummaries;

namespace MoneyManager.Client.Extensions;

public static class ServiceCollectionReadExtensions
{
    public static IServiceCollection AddReadDependencies(this IServiceCollection services)
    {
        return services
            .AddUseCases()
            .AddScoped<IAccountSummariesGateway, HttpAccountGateway>()
            .AddScoped<IAccountDetailsGateway, HttpAccountGateway>();
    }

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<AccountSummaries>()
            .AddScoped<AccountDetails>();
    }
}