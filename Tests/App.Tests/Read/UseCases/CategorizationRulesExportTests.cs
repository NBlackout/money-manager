using App.Read.Ports;
using App.Read.UseCases;
using App.Tests.Read.TestDoubles;

namespace App.Tests.Read.UseCases;

public class CategorizationRulesExportTests
{
    private readonly StubbedCategorizationRuleSummariesDataSource dataSource = new();
    private readonly StubbedCategorizationRuleExporter exporter = new();
    private readonly CategorizationRulesExport sut;

    public CategorizationRulesExportTests() =>
        this.sut = new CategorizationRulesExport(this.dataSource, this.exporter);

    [Theory, RandomData]
    public async Task Gives_categorization_rules_export(CategorizationRuleSummaryPresentation[] categorizationRules, Stream content)
    {
        this.Feed(categorizationRules, content);
        await this.Verify(content);
    }

    private async Task Verify(Stream expected)
    {
        Stream actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }

    private void Feed(CategorizationRuleSummaryPresentation[] categorizationRules, Stream content)
    {
        this.dataSource.Feed(categorizationRules);
        this.exporter.Feed(categorizationRules, content);
    }
}