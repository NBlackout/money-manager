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
            .AddScoped<AssignBankName>()
            .AddScoped<StopAccountTracking>()
            .AddScoped<ResumeAccountTracking>()
            .AddScoped<AssignAccountLabel>();
    }

    private static IServiceCollection AddInfrastructureAdapters(this IServiceCollection services) =>
        services.AddRepositories().AddScoped<IOfxParser, OfxParser>();

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        InMemoryBankRepository bankRepository = new();
        bankRepository.Feed(
            Bank.From(new BankSnapshot(Guid.Parse("25B71ECF-9514-4A75-874A-FCFB651D4928"), "4150012", "My bank")),
            Bank.From(new BankSnapshot(Guid.Parse("0B9D0446-4E2D-45AE-9E90-47D3233EE10F"), "09414", "Other bank"))
        );
        InMemoryAccountRepository accountRepository = new();
        accountRepository.Feed(
            Account.From(new AccountSnapshot(Guid.Parse("1A87A411-BBEB-4FB0-83E7-539CF5EFBE6C"),
                Guid.Parse("25B71ECF-9514-4A75-874A-FCFB651D4928"), "1234567890", "Compte joint", 12345.67m, true)
            ),
            Account.From(new AccountSnapshot(Guid.Parse("603F21F4-CE85-42AB-9E7E-87C9CFFE0F6D"),
                Guid.Parse("0B9D0446-4E2D-45AE-9E90-47D3233EE10F"), "0987654321", "Livret", 89.00m, true)
            ),
            Account.From(new AccountSnapshot(Guid.Parse("2981760C-A17B-4ECF-B828-AB89CCD1B11A"),
                Guid.Parse("25B71ECF-9514-4A75-874A-FCFB651D4928"), "ABC123ABC", "Epargne", 1000.00m, false)
            )
        );

        return services
            .AddSingleton<IBankRepository>(bankRepository)
            .AddSingleton(bankRepository)
            .AddSingleton<IAccountRepository>(accountRepository)
            .AddSingleton(accountRepository);
    }
}