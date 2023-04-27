using MoneyManager.Client.Read.Application.Ports;
using MoneyManager.Client.Read.Application.UseCases.AccountSummaries;
using MoneyManager.Client.Write.Application.Ports;
using MoneyManager.Client.Write.Application.UseCases;
using MoneyManager.Client.Read.Infrastructure.AccountSummariesGateway;
using MoneyManager.Client.Write.Infrastructure.AccountGateway;
using MoneyManager.Client.Write.Infrastructure.BankStatementGateway;

namespace MoneyManager.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWriteDependencies(this IServiceCollection services)
    {
        return services
            .AddScoped<UploadBankStatement>()
            .AddScoped<StopAccountTracking>()
            .AddScoped<ResumeAccountTracking>()
            .AddScoped<IBankStatementGateway, HttpBankStatementGateway>()
            .AddScoped<IAccountGateway, HttpAccountGateway>();
    }

    public static IServiceCollection AddReadDependencies(this IServiceCollection services)
    {
        return services
            .AddScoped<GetAccountSummaries>()
            .AddScoped<IAccountSummariesGateway, HttpAccountSummariesGateway>();
    }
}