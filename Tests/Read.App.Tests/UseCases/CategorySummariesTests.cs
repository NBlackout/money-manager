using Read.App.UseCases;
using Read.Infra.DataSources.CategorySummaries;
using Read.TestTooling;
using Shared.Presentation;

namespace Read.App.Tests.UseCases;

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