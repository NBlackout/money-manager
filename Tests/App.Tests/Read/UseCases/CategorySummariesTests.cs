using App.Read.Ports;
using App.Read.UseCases;
using App.Tests.Read.TestDoubles;

namespace App.Tests.Read.UseCases;

public class CategorySummariesTests
{
    private readonly StubbedCategorySummariesDataSource dataSource = new();
    private readonly CategorySummaries sut;

    public CategorySummariesTests() =>
        this.sut = new CategorySummaries(this.dataSource);

    [Theory]
    [RandomData]
    public async Task Gives_category_summaries(CategorySummaryPresentation[] expected)
    {
        this.Feed(expected);
        await this.Verify(expected);
    }

    private async Task Verify(CategorySummaryPresentation[] expected)
    {
        CategorySummaryPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }

    private void Feed(CategorySummaryPresentation[] expected) =>
        this.dataSource.Feed(expected);
}