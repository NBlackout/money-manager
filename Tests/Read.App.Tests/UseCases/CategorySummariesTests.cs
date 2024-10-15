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
    public async Task Retrieves_category_summaries(CategorySummaryPresentation[] expected)
    {
        this.dataSource.Feed(expected);
        CategorySummaryPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }
}