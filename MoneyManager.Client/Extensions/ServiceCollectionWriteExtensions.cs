using MoneyManager.Client.Write.Infrastructure.AccountGateway;
using MoneyManager.Client.Write.Infrastructure.BankGateway;
using MoneyManager.Client.Write.Infrastructure.BankStatementGateway;

namespace MoneyManager.Client.Extensions;

public static class ServiceCollectionWriteExtensions
{
    public static IServiceCollection AddWriteDependencies(this IServiceCollection services)
    {
        return services
            .AddUseCases()
            .AddScoped<IBankGateway, HttpBankGateway>()
            .AddScoped<IBankStatementGateway, HttpBankStatementGateway>()
            .AddScoped<IAccountGateway, HttpAccountGateway>();
    }

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<AssignBankName>()
            .AddScoped<UploadBankStatement>()
            .AddScoped<StopAccountTracking>()
            .AddScoped<ResumeAccountTracking>()
            .AddScoped<AssignAccountLabel>();
    }
}