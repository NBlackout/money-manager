using Client.Write.Infra;

namespace Client.Extensions;

public static class ServiceCollectionWriteExtensions
{
    public static IServiceCollection AddWrite(this IServiceCollection services) =>
        services.AddUseCases().AddClientWriteInfra();

    private static IServiceCollection AddUseCases(this IServiceCollection services) =>
        services
            .AddScoped<UploadBankStatement>()
            .AddScoped<StopAccountTracking>()
            .AddScoped<ResumeAccountTracking>()
            .AddScoped<AssignAccountLabel>()
            .AddScoped<CreateCategory>()
            .AddScoped<DeleteCategory>()
            .AddScoped<AssignTransactionCategory>()
            .AddScoped<DefineBudget>();
}