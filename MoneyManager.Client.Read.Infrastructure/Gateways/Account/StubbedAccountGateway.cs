namespace MoneyManager.Client.Read.Infrastructure.Gateways.Account;

public class StubbedAccountGateway : IAccountGateway
{
    private readonly IReadOnlyCollection<AccountSummaryPresentation> summaries;
    private readonly Dictionary<Guid, AccountDetailsPresentation> details = new();
    private readonly Dictionary<(Guid, int, int), IReadOnlyCollection<TransactionSummaryPresentation>> data = new();

    public StubbedAccountGateway(params AccountSummaryPresentation[] summaries)
    {
        this.summaries = summaries;
    }

    public Task<IReadOnlyCollection<AccountSummaryPresentation>> Summaries() =>
        Task.FromResult(this.summaries);

    public Task<AccountDetailsPresentation> Details(Guid id) =>
        Task.FromResult(this.details[id]);

    public Task<IReadOnlyCollection<TransactionSummaryPresentation>>
        TransactionsOfMonth(Guid id, int year, int month) =>
        Task.FromResult(this.data[(id, year, month)]);

    public void Feed(Guid id, AccountDetailsPresentation expected) =>
        this.details[id] = expected;

    public void Feed(Guid id, int year, int month, params TransactionSummaryPresentation[] expected) =>
        this.data[(id, year, month)] = expected;
}