using App.Read.Ports;
using App.Read.UseCases;
using App.Tests.Read.TestDoubles;

namespace App.Tests.Read.UseCases;

public class ExpectedTransactionSummariesTests
{
    private readonly StubbedExpectedTransactionSummariesDataSource dataSource = new();
    private readonly ExpectedTransactionSummaries sut;

    public ExpectedTransactionSummariesTests()
    {
        this.sut = new ExpectedTransactionSummaries(this.dataSource);
    }

    [Theory, RandomData]
    public async Task Gives_expected_transactions(int year, int month, ExpectedTransactionSummaryPresentation[] expected)
    {
        this.Feed(year, month, expected);
        await this.Verify(year, month, expected);
    }

    private async Task Verify(int year, int month, ExpectedTransactionSummaryPresentation[] expected)
    {
        ExpectedTransactionSummaryPresentation[] actual = await this.sut.Execute(year, month);
        actual.Should().Equal(expected);
    }

    private void Feed(int year, int month, ExpectedTransactionSummaryPresentation[] expected) =>
        this.dataSource.Feed(year, month, expected);
}