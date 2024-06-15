using Client.Write.App.Ports;
using Client.Write.Infra.Gateways.Account;
using Client.Write.Infra.Gateways.BankStatement;
using Client.Write.Infra.Gateways.Category;
using Client.Write.Infra.Gateways.Transaction;

namespace Client.Extensions;

public static class ServiceCollectionWriteExtensions
{
    public static IServiceCollection AddWriteDependencies(this IServiceCollection services)
    {
        return services
            .AddUseCases()
            .AddGateways();
    }

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<UploadBankStatement>()
            .AddScoped<StopAccountTracking>()
            .AddScoped<ResumeAccountTracking>()
            .AddScoped<AssignAccountLabel>()
            .AddScoped<CreateCategory>()
            .AddScoped<AssignTransactionCategory>();
    }

    private static IServiceCollection AddGateways(this IServiceCollection services)
    {
        return services
            .AddScoped<IBankStatementGateway, HttpBankStatementGateway>()
            .AddScoped<IAccountGateway, HttpAccountGateway>()
            .AddScoped<ICategoryGateway, HttpCategoryGateway>()
            .AddScoped<ITransactionGateway, HttpTransactionGateway>();
    }
}