using App.Read.Ports;

namespace App.Tests.Read.TestDoubles;

public class StubbedAccountSummariesDataSource : IAccountSummariesDataSource
{
    private AccountSummaryPresentation[] data = null!;

    public Task<AccountSummaryPresentation[]> All() =>
        Task.FromResult(this.data);

    public void Feed(AccountSummaryPresentation[] summaries) =>
        this.data = summaries;
}