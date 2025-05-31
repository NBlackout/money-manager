using Microsoft.Extensions.DependencyInjection;

namespace Client.Write.Infra;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientWriteInfra(this IServiceCollection services) =>
        services.AddGateways();

    private static IServiceCollection AddGateways(this IServiceCollection services) =>
        services
            .AddScoped<IBankStatementGateway, HttpBankStatementGateway>()
            .AddScoped<IAccountGateway, HttpAccountGateway>()
            .AddScoped<ICategoryGateway, HttpCategoryGateway>()
            .AddScoped<ITransactionGateway, HttpTransactionGateway>()
            .AddScoped<IBudgetGateway, HttpBudgetGateway>();
}