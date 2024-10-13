using Microsoft.Extensions.DependencyInjection;
using Read.Infra.DataSources.CategoriesWithKeywords;
using Write.Infra.Repositories;
using static Shared.TestTooling.Randomizer;

namespace Read.Infra.Tests.DataSources;

public sealed class RepositoryCategoriesWithKeywordsDataSourceTests : HostFixture
{
    private readonly RepositoryCategoriesWithKeywordsDataSource sut;
    private readonly InMemoryCategoryRepository categoryRepository;

    public RepositoryCategoriesWithKeywordsDataSourceTests()
    {
        this.sut = this.Resolve<ICategoriesWithKeywordsDataSource, RepositoryCategoriesWithKeywordsDataSource>();
        this.categoryRepository = this.Resolve<ICategoryRepository, InMemoryCategoryRepository>();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddWriteDependencies().AddReadDependencies();

    [Theory, RandomData]
    public async Task Retrieves_categories(CategoryBuilder[] expected)
    {
        this.Feed(expected);
        await this.Verify(expected);
    }

    [Fact]
    public async Task Excludes_ones_without_keywords()
    {
        this.Feed(
            Any<CategoryBuilder>() with { Keywords = "" },
            Any<CategoryBuilder>() with { Keywords = "   " }
        );
        await this.Verify([]);
    }

    private async Task Verify(params CategoryBuilder[] expected)
    {
        CategoryWithKeywords[] actual = await this.sut.All();
        actual.Should().Equal(expected.Select(c => new CategoryWithKeywords(c.Id, c.Label, c.Keywords)));
    }

    private void Feed(params CategoryBuilder[] categories) =>
        this.categoryRepository.Feed(categories.Select(c => c.Build()).ToArray());
}
