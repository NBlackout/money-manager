using App.Write.UseCases;
using Infra.Write;

namespace Client.Extensions;

public static class ServiceCollectionWriteExtensions
{
    public static IServiceCollection AddWrite(this IServiceCollection services) =>
        services.AddUseCases().AddWriteInfra();

    private static IServiceCollection AddUseCases(this IServiceCollection services) =>
        services
            .AddSingleton<ImportBankStatement>()
            .AddSingleton<AssignAccountLabel>()
            .AddSingleton<CreateCategory>()
            .AddSingleton<ImportCategories>()
            .AddSingleton<DeleteCategory>()
            .AddSingleton<ApplyCategorizationRule>()
            .AddSingleton<DeleteCategorizationRule>()
            .AddSingleton<ImportCategorizationRules>()
            .AddSingleton<AssignTransactionCategory>()
            .AddSingleton<DefineBudget>()
            .AddSingleton<ToggleTransactionRecurrence>();
}