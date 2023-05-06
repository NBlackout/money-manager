using MoneyManager.Client.Read.Application.Ports;
using MoneyManager.Client.Read.Application.UseCases.AccountSummaries;
using MoneyManager.Client.Read.Infrastructure.Gateways.AccountSummaries;

namespace MoneyManager.Client.Extensions;

public static class ServiceCollectionReadExtensions
{
    public static IServiceCollection AddReadDependencies(this IServiceCollection services)
    {
        return services
            .AddScoped<AccountSummaries>()
            .AddScoped<IAccountSummariesGateway, HttpAccountSummariesGateway>();
    }
}