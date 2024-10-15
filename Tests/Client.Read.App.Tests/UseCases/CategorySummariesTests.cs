namespace Client.Read.App.Tests.UseCases;

public class CategorySummariesTests
{
    private readonly StubbedCategoryGateway gateway = new();
    private readonly CategorySummaries sut;

    public CategorySummariesTests()
    {
        this.sut = new CategorySummaries(this.gateway);
    }

    [Theory, RandomData]
    public async Task Retrieves_category_summaries(CategorySummaryPresentation[] expected)
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
        this.gateway.Feed(expected);
}