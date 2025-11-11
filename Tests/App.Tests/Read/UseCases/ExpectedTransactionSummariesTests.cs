using App.Read.Ports;
using App.Read.UseCases;
using App.Tests.Read.TestDoubles;

namespace App.Tests.Read.UseCases;

public class ExpectedTransactionSummariesTests
{
    private readonly StubbedExpectedTransactionSummariesDataSource dataSource = new();
    private readonly ExpectedTransactionSummaries sut;

    public ExpectedTransactionSummariesTests() =>
        this.sut = new ExpectedTransactionSummaries(this.dataSource);

    [Theory]
    [RandomData]
    public async Task Gives_category_summaries(ExpectedTransactionSummaryPresentation[] expected)
    {
        this.Feed(expected);
        await this.Verify(expected);
    }

    private async Task Verify(ExpectedTransactionSummaryPresentation[] expected)
    {
        ExpectedTransactionSummaryPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }

    private void Feed(ExpectedTransactionSummaryPresentation[] expected) =>
        this.dataSource.Feed(expected);
}