using MoneyManager.Application.Read.AccountSummaries;
using MoneyManager.Application.Read.Ports;
using MoneyManager.Infrastructure.Read.AccountSummaries;

namespace MoneyManager.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        return services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();
    }

    public static IServiceCollection AddReadDependencies(this IServiceCollection services)
    {
        var accountSummaries = new[]
        {
            new AccountSummary(Guid.NewGuid(), "Compte joint", 12345.67m),
            new AccountSummary(Guid.NewGuid(), "Livret", 89.00m),
            new AccountSummary(Guid.NewGuid(), "Epargne", 1000.00m)
        };

        return services
            .AddScoped<GetAccountSummaries>()
            .AddScoped<IAccountSummariesDataSource>(_ => new StubbedAccountSummariesDataSource(accountSummaries));
    }
}