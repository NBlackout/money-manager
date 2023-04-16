using MoneyManager.Application.Read.Ports;
using MoneyManager.Application.Read.UseCases.AccountSummaries;
using MoneyManager.Infrastructure.Read.AccountSummariesDataSource;

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

    public static IServiceCollection AddReadDependencies(this IServiceCollection services)
    {
        AccountSummary[] accountSummaries = {
            new(Guid.Parse("1A87A411-BBEB-4FB0-83E7-539CF5EFBE6C"), "Compte joint", 12345.67m),
            new(Guid.Parse("603F21F4-CE85-42AB-9E7E-87C9CFFE0F6D"), "Livret", 89.00m),
            new(Guid.Parse("2981760C-A17B-4ECF-B828-AB89CCD1B11A"), "Epargne", 1000.00m)
        };

        return services
            .AddScoped<GetAccountSummaries>()
            .AddScoped<IAccountSummariesDataSource>(_ => new StubbedAccountSummariesDataSource(accountSummaries));
    }
}