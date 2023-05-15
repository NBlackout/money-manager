using MoneyManager.Client.Read.Infrastructure.Gateways.Category;

namespace MoneyManager.Client.Read.Application.Tests.UseCases;

public class CategorySummariesTests
{
    [Fact]
    public async Task Should_retrieve_category_summaries()
    {
        CategorySummaryPresentation[] expected =
        {
            new(Guid.NewGuid(), "My category label"),
            new(Guid.NewGuid(), "The label")
        };
        StubbedCategoryGateway gateway = new(expected);
        CategorySummaries sut = new(gateway);

        IReadOnlyCollection<CategorySummaryPresentation> actual = await sut.Execute();

        actual.Should().Equal(expected);
    }
}