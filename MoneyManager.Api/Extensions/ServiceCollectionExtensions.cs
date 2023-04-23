using MoneyManager.Application.Read.Ports;
using MoneyManager.Application.Read.UseCases;
using MoneyManager.Application.Write.Model;
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
        accountRepository.Feed(
            new AccountSnapshot(Guid.Parse("1A87A411-BBEB-4FB0-83E7-539CF5EFBE6C"), "My bank", "Compte joint", 12345.67m, true),
            new AccountSnapshot(Guid.Parse("603F21F4-CE85-42AB-9E7E-87C9CFFE0F6D"), "My bank", "Livret", 89.00m, true),
            new AccountSnapshot(Guid.Parse("2981760C-A17B-4ECF-B828-AB89CCD1B11A"), "My bank", "Epargne", 1000.00m, false));

        return services
            .AddScoped<ImportBankStatement>()
            .AddScoped<StopAccountTracking>()
            .AddScoped<ResumeAccountTracking>()
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