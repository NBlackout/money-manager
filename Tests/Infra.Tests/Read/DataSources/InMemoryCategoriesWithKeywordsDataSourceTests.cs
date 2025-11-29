using App.Read.Ports;
using App.Tests.Read.Tooling;
using App.Write.Ports;
using Infra.Read.DataSources;
using Infra.Tests.Tooling;
using Infra.Write.Repositories;

namespace Infra.Tests.Read.DataSources;

public class InMemoryCategoriesWithKeywordsDataSourceTests : InfraTest<ICategoriesWithKeywordsDataSource, InMemoryCategoriesWithKeywordsDataSource>
{
    private readonly InMemoryCategoryRepository categoryRepository;
    private readonly InMemoryCategorizationRuleRepository categorizationRuleRepository;

    public InMemoryCategoriesWithKeywordsDataSourceTests()
    {
        this.categoryRepository = this.Resolve<ICategoryRepository, InMemoryCategoryRepository>();
        this.categorizationRuleRepository = this.Resolve<ICategorizationRuleRepository, InMemoryCategorizationRuleRepository>();
    }

    [Theory]
    [RandomData]
    public async Task Gives_categories(CategorizationRuleBuilder[] expected)
    {
        this.Feed(expected);
        await this.Verify(expected.Select(c => new CategoryWithKeywords(c.CategoryId, c.CategoryLabel, c.Keywords)).ToArray());
    }

    private async Task Verify(params CategoryWithKeywords[] expected)
    {
        CategoryWithKeywords[] actual = await this.Sut.All();
        actual.Should().Equal(expected);
    }

    private void Feed(params CategorizationRuleBuilder[] categorizationRules)
    {
        this.categorizationRuleRepository.Feed([..categorizationRules.Select(c => c.ToSnapshot())]);
        this.categoryRepository.Feed([..categorizationRules.Select(c => c.ToCategorySnapshot())]);
    }
}