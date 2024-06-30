namespace Read.App.UseCases;

public class AccountSummaries(IAccountSummariesDataSource dataSource)
{
    public async Task<AccountSummaryPresentation[]> Execute() =>
        await dataSource.Get();
}
