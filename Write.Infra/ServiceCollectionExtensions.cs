﻿using Microsoft.Extensions.DependencyInjection;
using Write.App.UseCases;
using Write.Infra.BankStatementParsing;
using Write.Infra.Repositories;

namespace Write.Infra;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServerWriteInfra(this IServiceCollection services)
    {
        return services
            .AddUseCases()
            .AddRepositories()
            .AddParsers();
    }

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<ImportBankStatement>()
            .AddScoped<AssignAccountLabel>()
            .AddScoped<AssignTransactionCategory>()
            .AddScoped<CreateCategory>()
            .AddScoped<DeleteCategory>()
            .AddScoped<DefineBudget>();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        InMemoryAccountRepository accountRepository = new() { NextId = Guid.NewGuid };
        InMemoryCategoryRepository categoryRepository = new() { NextId = Guid.NewGuid };
        InMemoryTransactionRepository transactionRepository = new() { NextId = Guid.NewGuid };
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

    private static IServiceCollection AddParsers(this IServiceCollection services)
    {
        return services
            .AddScoped<IBankStatementParser, BankStatementParser>()
            .AddScoped<OfxBankStatementParser>()
            .AddScoped<CsvBankStatementParser>();
    }
}