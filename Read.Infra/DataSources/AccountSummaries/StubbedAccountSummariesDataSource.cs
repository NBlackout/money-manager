namespace Read.Infra.DataSources.AccountSummaries;

public class StubbedAccountSummariesDataSource : IAccountSummariesDataSource
{
    private AccountSummaryPresentation[] data = null!;

    public Task<AccountSummaryPresentation[]> All() =>
        Task.FromResult(this.data);

    public void Feed(AccountSummaryPresentation[] summaries) =>
        this.data = summaries;
}