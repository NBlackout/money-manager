namespace MoneyManager.Client.Read.Infrastructure.AccountSummariesGateway;

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