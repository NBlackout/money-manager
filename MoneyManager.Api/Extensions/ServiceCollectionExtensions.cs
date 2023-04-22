using MoneyManager.Application.Read.Ports;
using MoneyManager.Application.Read.UseCases.AccountSummaries;
using MoneyManager.Application.Write.Ports;
using MoneyManager.Application.Write.UseCases;
using MoneyManager.Infrastructure.Read.DataSources.AccountSummaries;
using MoneyManager.Infrastructure.Write.OfxProcessing;
using MoneyManager.Infrastructure.Write.Repositories;

namespace MoneyManager.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerDependencies(this IServiceCollection services)
    {
        return services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();
    }

    public static IServiceCollection AddBlazorDependencies(this IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddRazorPages();

        return services;
    }

    public static IServiceCollection AddWriteDependencies(this IServiceCollection services)
    {
        InMemoryAccountRepository accountRepository = new();

        return services
            .AddScoped<ImportBankStatement>()
            .AddSingleton<IAccountRepository>(accountRepository)
            .AddSingleton(accountRepository)
            .AddScoped<IOfxParser, OfxParser>();
    }

    public static IServiceCollection AddReadDependencies(this IServiceCollection services)
    {
        return services
            .AddScoped<GetAccountSummaries>()
            .AddScoped<IAccountSummariesDataSource, RepositoryAccountSummariesDataSource>();
    }
}