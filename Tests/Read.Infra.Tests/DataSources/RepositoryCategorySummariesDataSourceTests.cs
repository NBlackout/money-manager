using Microsoft.Extensions.DependencyInjection;
using Read.Infra.DataSources.CategorySummaries;
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

    protected override void Configure(IServiceCollection services) =>
        services.AddWriteDependencies().AddReadDependencies();

    [Theory, RandomData]
    public async Task Retrieves_categories(CategoryBuilder[] expected)
    {
        this.categoryRepository.Feed(expected.Select(c => c.Build()).ToArray());
        CategorySummaryPresentation[] actual = await this.sut.All();
        actual.Should().Equal(expected.Select(c => c.ToSummary()));
    }
}
