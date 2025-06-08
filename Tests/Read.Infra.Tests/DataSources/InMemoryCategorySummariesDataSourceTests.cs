using Write.Infra.Repositories;

namespace Read.Infra.Tests.DataSources;

public class InMemoryCategorySummariesDataSourceTests
    : InfraTest<ICategorySummariesDataSource, InMemoryCategorySummariesDataSource>
{
    private readonly InMemoryCategoryRepository categoryRepository;

    public InMemoryCategorySummariesDataSourceTests()
    {
        this.categoryRepository = this.Resolve<ICategoryRepository, InMemoryCategoryRepository>();
    }

    [Theory]
    [RandomData]
    public async Task Gives_categories(CategoryBuilder[] expected)
    {
        this.Feed(expected);
        await this.Verify(expected);
    }

    private async Task Verify(CategoryBuilder[] expected)
    {
        CategorySummaryPresentation[] actual = await this.Sut.All();
        actual.Should().Equal(expected.Select(c => c.ToSummary()));
    }

    private void Feed(CategoryBuilder[] expected) =>
        this.categoryRepository.Feed([..expected.Select(c => c.ToSnapshot())]);
}