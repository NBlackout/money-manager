using Microsoft.Extensions.DependencyInjection;
using Write.Infra;

namespace Write.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWrite(this IServiceCollection services) =>
        services.AddUseCases().AddServerWriteInfra();

    private static IServiceCollection AddUseCases(this IServiceCollection services) =>
        services
            .AddScoped<ImportBankStatement>()
            .AddScoped<AssignAccountLabel>()
            .AddScoped<AssignTransactionCategory>()
            .AddScoped<CreateCategory>()
            .AddScoped<DeleteCategory>()
            .AddScoped<DefineBudget>();
}