using Microsoft.Extensions.DependencyInjection;
using Read.Infra.DataSources;
using Read.Infra.DataSources.CategoriesWithPattern;
using Write.Infra.Repositories;
using static Shared.TestTooling.Randomizer;

namespace Read.Infra.Tests.DataSources;

public sealed class RepositoryCategoriesWithPatternDataSourceTests : HostFixture
{
    private readonly RepositoryCategoriesWithPatternDataSource sut;
    private readonly InMemoryCategoryRepository categoryRepository;

    public RepositoryCategoriesWithPatternDataSourceTests()
    {
        this.sut = this.Resolve<ICategoriesWithPatternDataSource, RepositoryCategoriesWithPatternDataSource>();
        this.categoryRepository = this.Resolve<ICategoryRepository, InMemoryCategoryRepository>();

        this.categoryRepository.Clear();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddWriteDependencies().AddReadDependencies();

    [Theory, RandomData]
    public async Task Should_retrieve_categories(CategoryBuilder[] expected)
    {
        this.Feed(expected);
        await this.Verify(expected);
    }

    [Fact]
    public async Task Should_exclude_ones_without_pattern()
    {
        this.Feed(
            Any<CategoryBuilder>() with { Pattern = "" },
            Any<CategoryBuilder>() with { Pattern = "   " }
        );
        await this.Verify(Array.Empty<CategoryBuilder>());
    }

    private async Task Verify(params CategoryBuilder[] expected)
    {
        CategoryWithPattern[] actual = await this.sut.Get();
        actual.Should().Equal(expected.Select(c => new CategoryWithPattern(c.Id, c.Label, c.Pattern)));
    }

    private void Feed(params CategoryBuilder[] categories) =>
        this.categoryRepository.Feed(categories.Select(c => c.Build()).ToArray());
}