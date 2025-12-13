using App.Read.Ports;

namespace App.Tests.Read.TestDoubles;

public class StubbedExpectedTransactionSummariesDataSource : IExpectedTransactionSummariesDataSource
{
    private readonly Dictionary<(int, int), ExpectedTransactionSummaryPresentation[]> data = [];

    public Task<ExpectedTransactionSummaryPresentation[]> By(int year, int month) =>
        Task.FromResult(this.data[(year, month)]);

    public void Feed(int year, int month, ExpectedTransactionSummaryPresentation[] transactions) =>
        this.data[(year, month)] = transactions;
}