using Client.Read.Infra;
using Client.Write.Infra;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Client;

public static class WebAssemblyHostBuilderExtensions
{
    public static void AddServices(this WebAssemblyHostBuilder builder)
    {
        builder
            .Services
            .AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api/") })
            .AddWrite()
            .AddRead();
    }

    private static IServiceCollection AddWrite(this IServiceCollection services)
    {
        return services.AddWriteUseCases().AddClientWriteInfra();
    }

    private static IServiceCollection AddWriteUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<UploadBankStatement>()
            .AddScoped<StopAccountTracking>()
            .AddScoped<ResumeAccountTracking>()
            .AddScoped<AssignAccountLabel>()
            .AddScoped<CreateCategory>()
            .AddScoped<DeleteCategory>()
            .AddScoped<AssignTransactionCategory>()
            .AddScoped<DefineBudget>();
    }

    private static IServiceCollection AddRead(this IServiceCollection services)
    {
        return services.AddReadUseCases().AddClientReadInfra();
    }

    private static IServiceCollection AddReadUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<AccountSummaries>()
            .AddScoped<AccountDetails>()
            .AddScoped<TransactionsOfMonth>()
            .AddScoped<CategorySummaries>()
            .AddScoped<CategorizationSuggestions>()
            .AddScoped<BudgetSummaries>();
    }
}