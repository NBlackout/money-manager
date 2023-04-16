using MoneyManager.Client.Application.Read.Ports;
using MoneyManager.Client.Application.Read.UseCases.AccountSummaries;
using MoneyManager.Client.Infrastructure.Read.AccountSummariesGateway;

namespace MoneyManager.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddReadDependencies(this IServiceCollection services)
    {
        return services
            .AddScoped<GetAccountSummaries>()
            .AddScoped<IAccountSummariesGateway, HttpAccountSummariesGateway>();
    }
}