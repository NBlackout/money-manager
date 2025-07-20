using App.Read.Ports;
using App.Read.UseCases;
using App.Tests.Read.TestDoubles;

namespace App.Tests.Read.UseCases;

public class CategorySummariesTests
{
    private readonly StubbedCategorySummariesDataSource dataSource = new();
    private readonly CategorySummaries sut;

    public CategorySummariesTests()
    {
        this.sut = new CategorySummaries(this.dataSource);
    }

    [Theory]
    [RandomData]
    public async Task Gives_category_summaries(CategorySummaryPresentation[] expected)
    {
        this.dataSource.Feed(expected);
        CategorySummaryPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }
}