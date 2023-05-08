namespace MoneyManager.Client.Read.Infrastructure.Gateways;

public class StubbedAccountSummariesGateway : IAccountSummariesGateway
{
    private readonly IReadOnlyCollection<AccountSummaryPresentation> expected;

    public StubbedAccountSummariesGateway(params AccountSummaryPresentation[] expected)
    {
        this.expected = expected;
    }

    public Task<IReadOnlyCollection<AccountSummaryPresentation>> Get() =>
        Task.FromResult(this.expected);
}