using MoneyManager.Write.Application.Model;
using MoneyManager.Write.Application.Ports;
using MoneyManager.Write.Application.UseCases;
using MoneyManager.Write.Infrastructure.OfxProcessing;
using MoneyManager.Write.Infrastructure.Repositories;

namespace MoneyManager.Api.Extensions;

public static class ServiceCollectionWriteExtensions
{
    public static IServiceCollection AddWriteDependencies(this IServiceCollection services) => 
        services.AddUseCases().AddInfrastructureAdapters();

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<ImportBankStatement>()
            .AddScoped<StopAccountTracking>()
            .AddScoped<ResumeAccountTracking>()
            .AddScoped<AssignAccountLabel>();
    }

    private static IServiceCollection AddInfrastructureAdapters(this IServiceCollection services) => 
        services.AddRepositories().AddScoped<IOfxParser, OfxParser>();

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        InMemoryAccountRepository accountRepository = new();
        accountRepository.Feed(
            Account.From(new AccountSnapshot(Guid.Parse("1A87A411-BBEB-4FB0-83E7-539CF5EFBE6C"), "My bank",
                "1234567890", "Compte joint", 12345.67m, true)),
            Account.From(new AccountSnapshot(Guid.Parse("603F21F4-CE85-42AB-9E7E-87C9CFFE0F6D"), "My other bank",
                "0987654321", "Livret", 89.00m, true)),
            Account.From(new AccountSnapshot(Guid.Parse("2981760C-A17B-4ECF-B828-AB89CCD1B11A"), "My bank", "ABC123ABC",
                "Epargne", 1000.00m, false))
        );

        return services.AddSingleton<IAccountRepository>(accountRepository)
            .AddSingleton(accountRepository);
    }
}