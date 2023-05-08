namespace MoneyManager.Read.Infrastructure.DataSources.TransactionsOfMonth;

public class StubbedTransactionsOfMonthDataSource : ITransactionsOfMonthDataSource
{
    private readonly Dictionary<(Guid, int, int), IReadOnlyCollection<TransactionSummaryPresentation>> data = new();

    public Task<IReadOnlyCollection<TransactionSummaryPresentation>> Get(Guid accountId, int year, int month) =>
        Task.FromResult(this.data[(accountId, year, month)]);

    public void Feed(Guid accountId, int year, int month, params TransactionSummaryPresentation[] expected) =>
        this.data[(accountId, year, month)] = expected;
}