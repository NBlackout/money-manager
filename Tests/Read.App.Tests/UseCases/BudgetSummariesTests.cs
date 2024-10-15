namespace Read.App.Tests.UseCases;

public class BudgetSummariesTests
{
    private readonly StubbedBudgetSummariesDataSource dataSource = new();
    private readonly BudgetSummaries sut;

    public BudgetSummariesTests()
    {
        this.sut = new BudgetSummaries(this.dataSource);
    }

    [Theory, RandomData]
    public async Task Retrieves_budget_summaries(BudgetSummaryPresentation[] expected)
    {
        this.dataSource.Feed(expected);
        BudgetSummaryPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }
}