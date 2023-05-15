using MoneyManager.Write.Application.Model.Accounts;
using MoneyManager.Write.Application.Model.Banks;
using MoneyManager.Write.Application.Model.Categories;
using MoneyManager.Write.Application.Model.Transactions;
using MoneyManager.Write.Infrastructure.OfxProcessing;

namespace MoneyManager.Write.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWriteDependencies(this IServiceCollection services) =>
        services.AddUseCases().AddInfrastructureAdapters();

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<ImportBankStatement>()
            .AddScoped<StopAccountTracking>()
            .AddScoped<ResumeAccountTracking>()
            .AddScoped<AssignAccountLabel>()
            .AddScoped<AssignTransactionCategory>();
    }

    private static IServiceCollection AddInfrastructureAdapters(this IServiceCollection services) =>
        services.AddRepositories().AddScoped<IOfxParser, OfxParser>();

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        InMemoryBankRepository bankRepository = new() { NextId = Guid.NewGuid };
        bankRepository.Feed(
            Bank.From(new BankSnapshot(Guid.Parse("25B71ECF-9514-4A75-874A-FCFB651D4928"), "4150012")),
            Bank.From(new BankSnapshot(Guid.Parse("0B9D0446-4E2D-45AE-9E90-47D3233EE10F"), "09414"))
        );
        InMemoryAccountRepository accountRepository = new() { NextId = Guid.NewGuid };
        accountRepository.Feed(
            Account.From(new AccountSnapshot(Guid.Parse("1A87A411-BBEB-4FB0-83E7-539CF5EFBE6C"),
                Guid.Parse("25B71ECF-9514-4A75-874A-FCFB651D4928"), "1234567890", "Compte joint", 12345.67m,
                DateTime.Parse("2023-03-30"), true)
            ),
            Account.From(new AccountSnapshot(Guid.Parse("603F21F4-CE85-42AB-9E7E-87C9CFFE0F6D"),
                Guid.Parse("0B9D0446-4E2D-45AE-9E90-47D3233EE10F"), "0987654321", "Livret", 89.00m,
                DateTime.Parse("2022-06-17"), true)
            ),
            Account.From(new AccountSnapshot(Guid.Parse("2981760C-A17B-4ECF-B828-AB89CCD1B11A"),
                Guid.Parse("25B71ECF-9514-4A75-874A-FCFB651D4928"), "ABC123ABC", "Epargne", 1000.00m,
                DateTime.Parse("2023-01-12"), false)
            )
        );
        InMemoryTransactionRepository transactionRepository = new() { NextId = Guid.NewGuid };
        transactionRepository.Feed(
            Transaction.From(new TransactionSnapshot(Guid.Parse("4C957075-C660-4616-9FB4-492A08FFAE1C"),
                Guid.Parse("1A87A411-BBEB-4FB0-83E7-539CF5EFBE6C"), "External id 1", -193.62m, "Payment",
                DateTime.Parse("2023-05-07"), null)
            ),
            Transaction.From(new TransactionSnapshot(Guid.Parse("3E9F5A24-9057-44F9-9A0F-2204F42E15ED"),
                Guid.Parse("1A87A411-BBEB-4FB0-83E7-539CF5EFBE6C"), "External id 2", 2305.51m, "Revenue",
                DateTime.Parse("2023-06-01"), null)
            ),
            Transaction.From(new TransactionSnapshot(Guid.Parse("63BE873F-1153-4200-9992-C26661B31B08"),
                Guid.Parse("1A87A411-BBEB-4FB0-83E7-539CF5EFBE6C"), "External id 3", -2.99m, "Pastry",
                DateTime.Parse("2023-06-02"), null)
            ),
            Transaction.From(new TransactionSnapshot(Guid.Parse("912B37C1-2089-4AF4-90DF-6C96F89DE716"),
                Guid.Parse("2981760C-A17B-4ECF-B828-AB89CCD1B11A"), "External id 1", 1400.00m, "Savings",
                DateTime.Parse("2023-04-16"), null)
            ),
            Transaction.From(new TransactionSnapshot(Guid.Parse("355C7029-D35E-49D9-8E01-0230280A012F"),
                Guid.Parse("2981760C-A17B-4ECF-B828-AB89CCD1B11A"), "External id 2", -15.99m, "Internet provider",
                DateTime.Parse("2023-01-02"), null)
            )
        );
        InMemoryCategoryRepository categoryRepository = new();
        categoryRepository.Feed(
            Category.From(new CategorySnapshot(Guid.Parse("E06F5A93-0C3C-4BF4-B8D5-738C0772D29E"), "First category")),
            Category.From(new CategorySnapshot(Guid.Parse("F8DE1AD3-7566-4BC0-B898-435373F6E89D"), "Second category"))
        );

        return services
            .AddSingleton<IBankRepository>(bankRepository)
            .AddSingleton(bankRepository)
            .AddSingleton<IAccountRepository>(accountRepository)
            .AddSingleton(accountRepository)
            .AddSingleton<ITransactionRepository>(transactionRepository)
            .AddSingleton(transactionRepository)
            .AddSingleton<ICategoryRepository>(categoryRepository)
            .AddSingleton(categoryRepository);
    }
}