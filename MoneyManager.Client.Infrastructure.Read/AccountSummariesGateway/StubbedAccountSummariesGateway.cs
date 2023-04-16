using MoneyManager.Shared;

namespace MoneyManager.Client.Infrastructure.Read.AccountSummariesGateway;

public class StubbedAccountSummariesGateway : IAccountSummariesGateway
{
    private readonly IReadOnlyCollection<AccountSummary> expected;

    public StubbedAccountSummariesGateway(params AccountSummary[] expected)
    {
        this.expected = expected;
    }

    public Task<IReadOnlyCollection<AccountSummary>> Get() =>
        Task.FromResult(this.expected);
}