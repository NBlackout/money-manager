using MoneyManager.Client.Application.Read.Ports;
using MoneyManager.Client.Application.Read.UseCases.AccountSummaries;
using MoneyManager.Client.Application.Write.Ports;
using MoneyManager.Client.Application.Write.UseCases.BankStatement;
using MoneyManager.Client.Infrastructure.Read.AccountSummariesGateway;
using MoneyManager.Client.Infrastructure.Write.BankStatementGateway;

namespace MoneyManager.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWriteDependencies(this IServiceCollection services)
    {
        return services
            .AddScoped<UploadBankStatement>()
            .AddScoped<IBankStatementGateway, HttpBankStatementGateway>();
    }

    public static IServiceCollection AddReadDependencies(this IServiceCollection services)
    {
        return services
            .AddScoped<GetAccountSummaries>()
            .AddScoped<IAccountSummariesGateway, HttpAccountSummariesGateway>();
    }
}