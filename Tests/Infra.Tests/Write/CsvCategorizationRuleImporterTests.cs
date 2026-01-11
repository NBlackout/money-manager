using App.Write.Model.ValueObjects;
using App.Write.Ports;
using Infra.Tests.Tooling;
using Infra.Write;
using Tooling;

namespace Infra.Tests.Write;

public class CsvCategorizationRuleImporterTests : InfraTest<ICategorizationRuleImporter, CsvCategorizationRuleImporter>
{
    [Fact]
    public async Task Parses_categorization_rules() =>
        await this.Verify(
            """
            Category;Keywords;Amount;Margin
            A category;Few keywords;;
            Another category;Few other keywords;2;
            Yet another category;Yet few other keywords;3.5;0.5
            """,
            new CategorizationRuleToImport(new Label("A category"), "Few keywords", null, null),
            new CategorizationRuleToImport(new Label("Another category"), "Few other keywords", 2, null),
            new CategorizationRuleToImport(new Label("Yet another category"), "Yet few other keywords", 3.5m, 0.5m)
        );

    [Fact]
    public async Task Tells_when_empty() =>
        await this.Verify(string.Empty);

    [Fact]
    public async Task Tells_when_only_headers() =>
        await this.Verify("Category;Keywords;Amount;Margin");

    private async Task Verify(string content, params CategorizationRuleToImport[] expected)
    {
        CategorizationRuleToImport[] categories = await this.Sut.Parse(content.ToUtf8Stream());
        categories.Should().Equal(expected);
    }
}