using MoneyManager.Read.Infrastructure.DataSources.AccountSummaries;

namespace MoneyManager.Api.Extensions;

public static class ServiceCollectionReadExtensions
{
    public static IServiceCollection AddReadDependencies(this IServiceCollection services) =>
        services.AddUseCases().AddInfrastructureAdapters();

    private static IServiceCollection AddUseCases(this IServiceCollection services) =>
        services.AddScoped<GetAccountSummaries>();

    private static IServiceCollection AddInfrastructureAdapters(this IServiceCollection services) =>
        services.AddScoped<IAccountSummariesDataSource, RepositoryAccountSummariesDataSource>();
}