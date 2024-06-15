namespace Read.Infra.DataSources.AccountSummaries;

public class StubbedAccountSummariesDataSource : IAccountSummariesDataSource
{
    private readonly IReadOnlyCollection<AccountSummaryPresentation> data;

    public StubbedAccountSummariesDataSource(IReadOnlyCollection<AccountSummaryPresentation> data) =>
        this.data = data;

    public Task<IReadOnlyCollection<AccountSummaryPresentation>> Get() =>
        Task.FromResult(this.data);
}