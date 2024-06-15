namespace Client.Read.Infra.Gateways.Account;

public class StubbedAccountGateway : IAccountGateway
{
    private AccountSummaryPresentation[] summaries = null!;
    private readonly Dictionary<Guid, AccountDetailsPresentation> details = new();
    private readonly Dictionary<(Guid, int, int), TransactionSummaryPresentation[]> data = new();

    public Task<AccountSummaryPresentation[]> Summaries() =>
        Task.FromResult(this.summaries);

    public Task<AccountDetailsPresentation> Details(Guid id) =>
        Task.FromResult(this.details[id]);

    public Task<TransactionSummaryPresentation[]> TransactionsOfMonth(Guid id, int year, int month) =>
        Task.FromResult(this.data[(id, year, month)]);

    public void Feed(AccountSummaryPresentation[] summaries) =>
        this.summaries = summaries;

    public void Feed(Guid id, AccountDetailsPresentation expected) =>
        this.details[id] = expected;

    public void Feed(Guid id, int year, int month, params TransactionSummaryPresentation[] expected) =>
        this.data[(id, year, month)] = expected;
}