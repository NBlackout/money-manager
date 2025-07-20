using App.Read.Ports;

namespace App.Read.UseCases;

public class AccountSummaries(IAccountSummariesDataSource dataSource)
{
    public async Task<AccountSummaryPresentation[]> Execute() =>
        await dataSource.All();
}