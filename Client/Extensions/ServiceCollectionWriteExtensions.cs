using App.Write.UseCases;
using App.Write.UseCases.Accounts;
using App.Write.UseCases.Categories;
using App.Write.UseCases.CategorizationRules;
using App.Write.UseCases.Transactions;
using Infra.Write;

namespace Client.Extensions;

public static class ServiceCollectionWriteExtensions
{
    public static IServiceCollection AddWrite(this IServiceCollection services) =>
        services.AddUseCases().AddWriteInfra();

    private static IServiceCollection AddUseCases(this IServiceCollection services) =>
        services
            .AddSingleton<ImportBankStatement>()
            .AddSingleton<RenameAccount>()
            .AddSingleton<CreateCategory>()
            .AddSingleton<RenameCategory>()
            .AddSingleton<ImportCategories>()
            .AddSingleton<DeleteCategory>()
            .AddSingleton<ApplyCategorizationRule>()
            .AddSingleton<DeleteCategorizationRule>()
            .AddSingleton<ImportCategorizationRules>()
            .AddSingleton<AssignTransactionCategory>()
            .AddSingleton<DefineBudget>()
            .AddSingleton<MarkTransactionAsRecurring>()
            .AddSingleton<PreferTransactionLabel>();
}