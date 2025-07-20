using App.Read.Ports;
using App.Tests.Read.Tooling;
using App.Write.Ports;
using Infra.Read.DataSources;
using Infra.Tests.Tooling;
using Infra.Write.Repositories;

namespace Infra.Tests.Read.DataSources;

public class InMemoryCategorySummariesDataSourceTests : InfraTest<ICategorySummariesDataSource, InMemoryCategorySummariesDataSource>
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
        await this.Verify(expected.Select(c => c.ToSummary()).ToArray());
    }

    private async Task Verify(CategorySummaryPresentation[] expected)
    {
        CategorySummaryPresentation[] actual = await this.Sut.All();
        actual.Should().Equal(expected);
    }

    private void Feed(CategoryBuilder[] expected) =>
        this.categoryRepository.Feed([..expected.Select(c => c.ToSnapshot())]);
}