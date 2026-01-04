using App.Read.Ports;
using App.Read.UseCases.CategorizationRules;
using App.Tests.Read.TestDoubles;

namespace App.Tests.Read.UseCases;

public class CategorizationRuleSummariesTests
{
    private readonly StubbedCategorizationRuleSummariesDataSource dataSource = new();
    private readonly CategorizationRuleSummaries sut;

    public CategorizationRuleSummariesTests()
    {
        this.sut = new CategorizationRuleSummaries(this.dataSource);
    }

    [Theory, RandomData]
    public async Task Gives_categorization_rules(CategorizationRuleSummaryPresentation[] expected)
    {
        this.Feed(expected);
        await this.Verify(expected);
    }

    private async Task Verify(CategorizationRuleSummaryPresentation[] expected)
    {
        CategorizationRuleSummaryPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }

    private void Feed(CategorizationRuleSummaryPresentation[] expected) =>
        this.dataSource.Feed(expected);
}