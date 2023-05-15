using MoneyManager.Read.Application.UseCases;
using MoneyManager.Read.Infrastructure.DataSources.CategorySummaries;
using MoneyManager.Read.TestTooling;
using MoneyManager.Shared.Presentation;

namespace MoneyManager.Read.Application.Tests.UseCases;

public class CategorySummariesTests
{
    [Fact]
    public async Task Should_retrieve_category_summaries()
    {
        CategorySummaryPresentation[] expected =
        {
            CategoryBuilder.For(Guid.NewGuid()).ToSummary(), CategoryBuilder.For(Guid.NewGuid()).ToSummary()
        };
        CategorySummaries sut = new(new StubbedCategorySummariesDataSource(expected));

        IReadOnlyCollection<CategorySummaryPresentation> actual = await sut.Execute();

        actual.Should().Equal(expected);
    }
}