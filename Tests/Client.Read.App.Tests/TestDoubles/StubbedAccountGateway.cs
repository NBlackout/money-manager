namespace Client.Read.App.Tests.TestDoubles;

public class StubbedAccountGateway : IAccountGateway
{
    private AccountSummaryPresentation[] summaries = null!;
    private readonly Dictionary<Guid, AccountDetailsPresentation> details = new();
    private readonly Dictionary<(Guid, int, int), TransactionSummaryPresentation[]> transactionsSummaries = new();

    public Task<AccountSummaryPresentation[]> Summaries() =>
        Task.FromResult(this.summaries);

    public Task<AccountDetailsPresentation> Details(Guid id) =>
        Task.FromResult(this.details[id]);

    public Task<TransactionSummaryPresentation[]> TransactionsOfMonth(Guid id, int year, int month) =>
        Task.FromResult(this.transactionsSummaries[(id, year, month)]);

    public void Feed(AccountSummaryPresentation[] summaries) =>
        this.summaries = summaries;

    public void Feed(Guid id, AccountDetailsPresentation details) =>
        this.details[id] = details;

    public void Feed(Guid id, int year, int month, params TransactionSummaryPresentation[] transactionsSummaries) =>
        this.transactionsSummaries[(id, year, month)] = transactionsSummaries;
}