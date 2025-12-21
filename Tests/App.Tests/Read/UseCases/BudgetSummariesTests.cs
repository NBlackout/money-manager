using App.Read.Ports;
using App.Read.UseCases;
using App.Tests.Read.TestDoubles;

namespace App.Tests.Read.UseCases;

public class BudgetSummariesTests
{
    private readonly StubbedBudgetSummariesDataSource dataSource = new();
    private readonly BudgetSummaries sut;

    public BudgetSummariesTests() =>
        this.sut = new BudgetSummaries(this.dataSource);

    [Theory]
    [RandomData]
    public async Task Gives_budgets(BudgetSummaryPresentation[] expected)
    {
        this.Feed(expected);
        await this.Verify(expected);
    }

    private async Task Verify(BudgetSummaryPresentation[] expected)
    {
        BudgetSummaryPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }

    private void Feed(BudgetSummaryPresentation[] expected) =>
        this.dataSource.Feed(expected);
}