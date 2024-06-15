namespace Read.App.UseCases;

public class AccountSummaries
{
    private readonly IAccountSummariesDataSource dataSource;

    public AccountSummaries(IAccountSummariesDataSource dataSource)
    {
        this.dataSource = dataSource;
    }

    public async Task<AccountSummaryPresentation[]> Execute() =>
        await this.dataSource.Get();
}