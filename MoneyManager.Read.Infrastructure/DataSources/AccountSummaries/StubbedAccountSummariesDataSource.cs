namespace MoneyManager.Read.Infrastructure.DataSources.AccountSummaries;

public class StubbedAccountSummariesDataSource : IAccountSummariesDataSource
{
    private readonly IReadOnlyCollection<AccountSummary> data;

    public StubbedAccountSummariesDataSource(IReadOnlyCollection<AccountSummary> data) =>
        this.data = data;

    public Task<IReadOnlyCollection<AccountSummary>> Get() =>
        Task.FromResult(this.data);
}