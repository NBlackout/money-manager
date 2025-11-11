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
            .AddSingleton<DeleteCategory>()
            .AddSingleton<ImportCategories>()
            .AddSingleton<AssignTransactionCategory>()
            .AddSingleton<DefineBudget>()
            .AddSingleton<ToggleTransactionRecurrence>();
}