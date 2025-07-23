using App.Write.Model.ValueObjects;
using App.Write.Ports;
using Infra.Tests.Tooling;
using Infra.Write;
using Tooling;

namespace Infra.Tests.Write;

public class CsvCategoryImporterTests : InfraTest<ICategoryImporter, CsvCategoryImporter>
{
    [Fact]
    public async Task Parses_many_categories() =>
        await this.Verify(
            """
            Label,Keywords
            A label,Few keywords
            Another label,Few other keywords
            Yet another label,Yet few other keywords
            """,
            new CategoryToImport(new Label("A label"), "Few keywords"),
            new CategoryToImport(new Label("Another label"), "Few other keywords"),
            new CategoryToImport(new Label("Yet another label"), "Yet few other keywords")
        );

    [Fact]
    public async Task Tells_when_empty() =>
        await this.Verify(string.Empty);

    [Fact]
    public async Task Tells_when_only_headers() =>
        await this.Verify("Label,Keywords");

    private async Task Verify(string content, params CategoryToImport[] expected)
    {
        CategoryToImport[] categories = await this.Sut.Parse(content.ToUtf8Stream());
        categories.Should().Equal(expected);
    }
}