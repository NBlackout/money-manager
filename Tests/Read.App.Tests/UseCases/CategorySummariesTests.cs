using Read.App.UseCases;
using Read.Infra.DataSources.CategorySummaries;
using Shared.Presentation;

namespace Read.App.Tests.UseCases;

public class CategorySummariesTests
{
    private readonly StubbedCategorySummariesDataSource dataSource = new();
    private readonly CategorySummaries sut;

    public CategorySummariesTests()
    {
        this.sut = new CategorySummaries(this.dataSource);
    }

    [Theory, RandomData]
    public async Task Should_retrieve_category_summaries(CategorySummaryPresentation[] expected)
    {
        this.dataSource.Feed(expected);
        CategorySummaryPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }
}