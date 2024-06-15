using Client.Read.Infra.Gateways.Category;

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
    public async Task Should_retrieve_category_summaries(CategorySummaryPresentation[] expected)
    {
        this.gateway.Feed(expected);
        CategorySummaryPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }
}