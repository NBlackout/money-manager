using MoneyManager.Read.Infrastructure.DataSources.AccountDetails;
using MoneyManager.Read.Infrastructure.DataSources.AccountSummaries;

namespace MoneyManager.Read.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddReadDependencies(this IServiceCollection services) =>
        services.AddUseCases().AddInfrastructureAdapters();

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<AccountSummaries>()
            .AddScoped<AccountDetails>();
    }

    private static IServiceCollection AddInfrastructureAdapters(this IServiceCollection services)
    {
        return services
            .AddScoped<IAccountSummariesDataSource, RepositoryAccountSummariesDataSource>()
            .AddScoped<IAccountDetailsDataSource, RepositoryAccountDetailsDataSource>();
    }
}