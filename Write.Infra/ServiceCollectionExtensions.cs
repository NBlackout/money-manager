using Microsoft.Extensions.DependencyInjection;
using Write.Infra.BankStatementParsing;
using Write.Infra.Repositories;

namespace Write.Infra;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServerWriteInfra(this IServiceCollection services) =>
        services.AddRepositories().AddParsers();

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        InMemoryAccountRepository accountRepository = new() { NextId = () => new AccountId(Guid.NewGuid()) };
        InMemoryCategoryRepository categoryRepository = new() { NextId = () => new CategoryId(Guid.NewGuid()) };
        InMemoryTransactionRepository transactionRepository =
            new() { NextId = () => new TransactionId(Guid.NewGuid()) };
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
            .AddScoped<IBankStatementParser, BankStatementParser>()
            .AddScoped<OfxBankStatementParser>()
            .AddScoped<CsvBankStatementParser>();
}