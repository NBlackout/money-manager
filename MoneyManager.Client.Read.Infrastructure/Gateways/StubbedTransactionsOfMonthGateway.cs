namespace MoneyManager.Client.Read.Infrastructure.Gateways;

public class StubbedTransactionsOfMonthGateway : ITransactionsOfMonthGateway
{
    private readonly Dictionary<(Guid, int, int), IReadOnlyCollection<TransactionSummaryPresentation>> data = new();

    public Task<IReadOnlyCollection<TransactionSummaryPresentation>> Get(Guid id, int year, int month) =>
        Task.FromResult(this.data[(id, year, month)]);

    public void Feed(Guid id, int year, int month, params TransactionSummaryPresentation[] expected) =>
        this.data[(id, year, month)] = expected;
}