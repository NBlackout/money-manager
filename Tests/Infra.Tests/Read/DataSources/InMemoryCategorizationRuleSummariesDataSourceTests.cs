using App.Read.Ports;
using App.Tests.Read.Tooling;
using App.Write.Ports;
using Infra.Read.DataSources;
using Infra.Tests.Tooling;
using Infra.Write.Repositories;

namespace Infra.Tests.Read.DataSources;

public class InMemoryCategorizationRuleSummariesDataSourceTests
    : InfraTest<ICategorizationRuleSummariesDataSource, InMemoryCategorizationRuleSummariesDataSource>
{
    private readonly InMemoryCategorizationRuleRepository categorizationRuleRepository;
    private readonly InMemoryCategoryRepository categoryRepository;

    public InMemoryCategorizationRuleSummariesDataSourceTests()
    {
        this.categorizationRuleRepository = this.Resolve<ICategorizationRuleRepository, InMemoryCategorizationRuleRepository>();
        this.categoryRepository = this.Resolve<ICategoryRepository, InMemoryCategoryRepository>();
    }

    [Theory, RandomData]
    public async Task Gives_categorization_rules(CategorizationRuleBuilder[] expected)
    {
        this.Feed(expected);
        await this.Verify(expected.Select(c => c.ToSummary()).ToArray());
    }

    private async Task Verify(CategorizationRuleSummaryPresentation[] expected)
    {
        CategorizationRuleSummaryPresentation[] actual = await this.Sut.All();
        actual.Should().Equal(expected);
    }

    private void Feed(CategorizationRuleBuilder[] categorizationRules)
    {
        this.categorizationRuleRepository.Feed([..categorizationRules.Select(c => c.ToSnapshot())]);
        this.categoryRepository.Feed([..categorizationRules.Select(c => c.ToCategorySnapshot())]);
    }
}