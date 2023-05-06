namespace MoneyManager.Read.Application.UseCases;

public class AccountSummaries
{
    private readonly IAccountSummariesDataSource dataSource;

    public AccountSummaries(IAccountSummariesDataSource dataSource)
    {
        this.dataSource = dataSource;
    }

    public async Task<IReadOnlyCollection<AccountSummaryPresentation>> Execute() =>
        await this.dataSource.Get();
}