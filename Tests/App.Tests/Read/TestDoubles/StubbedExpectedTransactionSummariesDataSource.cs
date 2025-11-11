using App.Read.Ports;

namespace App.Tests.Read.TestDoubles;

public class StubbedExpectedTransactionSummariesDataSource : IExpectedTransactionSummariesDataSource
{
    private ExpectedTransactionSummaryPresentation[] data = null!;

    public Task<ExpectedTransactionSummaryPresentation[]> All() =>
        Task.FromResult(this.data);

    public void Feed(ExpectedTransactionSummaryPresentation[] transactions) =>
        this.data = transactions;
}