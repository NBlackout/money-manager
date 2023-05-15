using MoneyManager.Read.Infrastructure.DataSources.CategorySummaries;
using MoneyManager.Write.Infrastructure.Repositories;

namespace MoneyManager.Read.Infrastructure.Tests.DataSources;

public sealed class RepositoryCategorySummariesDataSourceTests : IDisposable
{
    private readonly IHost host;
    private readonly RepositoryCategorySummariesDataSource sut;
    private readonly InMemoryCategoryRepository categoryRepository;

    public RepositoryCategorySummariesDataSourceTests()
    {
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services => services.AddWriteDependencies().AddReadDependencies())
            .Build();
        this.sut = this.host.Service<ICategorySummariesDataSource, RepositoryCategorySummariesDataSource>();
        this.categoryRepository = this.host.Service<ICategoryRepository, InMemoryCategoryRepository>();

        this.categoryRepository.Clear();
    }

    [Fact]
    public async Task Should_retrieve_tracked_category_summaries()
    {
        CategoryBuilder aCategory = CategoryBuilder.For(Guid.NewGuid()) ;
        CategoryBuilder anotherCategory = CategoryBuilder.For(Guid.NewGuid()) ;
        this.categoryRepository.Feed(aCategory.Build(), anotherCategory.Build());

        IReadOnlyCollection<CategorySummaryPresentation> actual = await this.sut.Get();
        actual.Should().Equal(aCategory.ToSummary(), anotherCategory.ToSummary());
    }

    public void Dispose() =>
        this.host.Dispose();
}