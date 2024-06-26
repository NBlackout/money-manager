﻿using Write.Infra.BankStatementParsing;

namespace Write.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWriteDependencies(this IServiceCollection services) =>
        services.AddUseCases().AddInfrastructureAdapters();

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<ImportBankStatement>()
            .AddScoped<AssignAccountLabel>()
            .AddScoped<AssignTransactionCategory>()
            .AddScoped<CreateCategory>()
            .AddScoped<DeleteCategory>();
    }

    private static IServiceCollection AddInfrastructureAdapters(this IServiceCollection services)
    {
        return services
            .AddRepositories()
            .AddScoped<IBankStatementParser, BankStatementParser>()
            .AddScoped<OfxBankStatementParser>()
            .AddScoped<CsvBankStatementParser>();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        InMemoryAccountRepository accountRepository = new() { NextId = Guid.NewGuid };
        InMemoryCategoryRepository categoryRepository = new() { NextId = Guid.NewGuid };
        InMemoryTransactionRepository transactionRepository = new() { NextId = Guid.NewGuid };

        return services
            .AddSingleton<IAccountRepository>(accountRepository)
            .AddSingleton(accountRepository)
            .AddSingleton<ITransactionRepository>(transactionRepository)
            .AddSingleton(transactionRepository)
            .AddSingleton<ICategoryRepository>(categoryRepository)
            .AddSingleton(categoryRepository);
    }
}
