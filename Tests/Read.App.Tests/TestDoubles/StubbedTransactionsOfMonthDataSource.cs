namespace Read.App.Tests.TestDoubles;

public class StubbedTransactionsOfMonthDataSource : ITransactionsOfMonthDataSource
{
    private readonly Dictionary<(Guid, int, int), TransactionSummaryPresentation[]> data = new();

    public Task<TransactionSummaryPresentation[]> By(Guid accountId, int year, int month) =>
        Task.FromResult(this.data[(accountId, year, month)]);

    public void Feed(Guid accountId, int year, int month, params TransactionSummaryPresentation[] transactionsSummaries) =>
        this.data[(accountId, year, month)] = transactionsSummaries;
}