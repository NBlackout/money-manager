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

        this.categoryRepository.Clear();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddWriteDependencies().AddReadDependencies();

    [Fact]
    public async Task Should_retrieve_tracked_category_summaries()
    {
        CategoryBuilder aCategory = CategoryBuilder.For(Guid.NewGuid()) ;
        CategoryBuilder anotherCategory = CategoryBuilder.For(Guid.NewGuid()) ;
        this.categoryRepository.Feed(aCategory.Build(), anotherCategory.Build());

        IReadOnlyCollection<CategorySummaryPresentation> actual = await this.sut.Get();
        actual.Should().Equal(aCategory.ToSummary(), anotherCategory.ToSummary());
    }
}