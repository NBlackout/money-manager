using App.Write.Model.Accounts;
using App.Write.Model.Categories;
using App.Write.Model.Transactions;
using App.Write.Ports;
using Infra.Write.BankStatementParsing;
using Infra.Write.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Write;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWriteInfra(this IServiceCollection services) =>
        services.AddRepositories().AddParsers();

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        InMemoryAccountRepository accountRepository = new() { NextId = () => new AccountId(Guid.NewGuid()) };
        InMemoryCategoryRepository categoryRepository = new() { NextId = () => new CategoryId(Guid.NewGuid()) };
        InMemoryTransactionRepository transactionRepository = new() { NextId = () => new TransactionId(Guid.NewGuid()) };
        InMemoryBudgetRepository budgetRepository = new();

        return services
            .AddSingleton<IAccountRepository>(accountRepository)
            .AddSingleton(accountRepository)
            .AddSingleton<ITransactionRepository>(transactionRepository)
            .AddSingleton(transactionRepository)
            .AddSingleton<ICategoryRepository>(categoryRepository)
            .AddSingleton(categoryRepository)
            .AddSingleton<IBudgetRepository>(budgetRepository)
            .AddSingleton(budgetRepository);
    }

    private static IServiceCollection AddParsers(this IServiceCollection services) =>
        services
            .AddSingleton<IBankStatementParser, BankStatementParser>()
            .AddSingleton<OfxBankStatementParser>()
            .AddSingleton<CsvBankStatementParser>()
            .AddSingleton<ICategoryImporter, CsvCategoryImporter>();
}