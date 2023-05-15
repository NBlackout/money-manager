using MoneyManager.Client.Write.Application.Ports;
using MoneyManager.Client.Write.Application.UseCases;
using MoneyManager.Client.Write.Infrastructure.Gateways.Account;
using MoneyManager.Client.Write.Infrastructure.Gateways.BankStatement;
using MoneyManager.Client.Write.Infrastructure.Gateways.Category;

namespace MoneyManager.Client.Extensions;

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
            .AddScoped<CreateCategory>();
    }

    private static IServiceCollection AddGateways(this IServiceCollection services)
    {
        return services
            .AddScoped<IBankStatementGateway, HttpBankStatementGateway>()
            .AddScoped<IAccountGateway, HttpAccountGateway>()
            .AddScoped<ICategoryGateway, HttpCategoryGateway>();
    }
}