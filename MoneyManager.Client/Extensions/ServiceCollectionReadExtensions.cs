using MoneyManager.Client.Read.Application.Ports;
using MoneyManager.Client.Read.Application.UseCases.AccountSummaries;
using MoneyManager.Client.Read.Infrastructure.AccountSummariesGateway;

namespace MoneyManager.Client.Extensions;

public static class ServiceCollectionReadExtensions
{
    public static IServiceCollection AddReadDependencies(this IServiceCollection services)
    {
        return services
            .AddScoped<GetAccountSummaries>()
            .AddScoped<IAccountSummariesGateway, HttpAccountSummariesGateway>();
    }
}