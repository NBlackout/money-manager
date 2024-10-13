using Read.Infra.DataSources.CategorySummaries;
using Shared.Infra.TestTooling;
using Write.Infra.Repositories;

namespace Read.Infra.Tests.DataSources;

public sealed class RepositoryCategorySummariesDataSourceTests : HostFixture
{
    private readonly RepositoryCategorySummariesDataSource sut;
    private readonly InMemoryCategoryRepository categoryRepository;

    public RepositoryCategorySummariesDataSourceTests()
    {
        this.sut = this.Resolve<ICategorySummariesDataSource, RepositoryCategorySummariesDataSource>();
        this.categoryRepository = this.Resolve<ICategoryRepository, InMemoryCategoryRepository>();
    }

    [Theory, RandomData]
    public async Task Retrieves_categories(CategoryBuilder[] expected)
    {
        this.Feed(expected);
        await this.Verify(expected);
    }

    private async Task Verify(CategoryBuilder[] expected)
    {
        CategorySummaryPresentation[] actual = await this.sut.All();
        actual.Should().Equal(expected.Select(c => c.ToSummary()));
    }

    private void Feed(CategoryBuilder[] expected) =>
        this.categoryRepository.Feed(expected.Select(c => c.ToSnapshot()).ToArray());
}