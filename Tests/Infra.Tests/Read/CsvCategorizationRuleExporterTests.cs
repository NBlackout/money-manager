using App.Read.Ports;
using Infra.Read;
using Infra.Tests.Tooling;

namespace Infra.Tests.Read;

public class CsvCategorizationRuleExporterTests : InfraTest<ICategorizationRuleExporter, CsvCategorizationRuleExporter>
{
    [Theory, RandomData]
    public async Task Exports(CategorizationRuleSummaryPresentation aCategorizationRule, CategorizationRuleSummaryPresentation anotherCategorizationRule) =>
        await this.Verify(
            [aCategorizationRule, anotherCategorizationRule with { Amount = null }],
            $"""
             Category;Keywords;Amount
             {aCategorizationRule.CategoryLabel};{aCategorizationRule.Keywords};{aCategorizationRule.Amount.ToString()}
             {anotherCategorizationRule.CategoryLabel};{anotherCategorizationRule.Keywords};
             """
        );

    [Fact]
    public async Task Exports_when_no_categorization_rule() =>
        await this.Verify([], "Category;Keywords;Amount");

    private async Task Verify(CategorizationRuleSummaryPresentation[] categories, string expected)
    {
        Stream actual = await this.Sut.Export(categories);
        actual.Should().Equal(expected);
    }
}