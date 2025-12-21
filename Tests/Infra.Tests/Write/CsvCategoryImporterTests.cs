using App.Write.Model.ValueObjects;
using App.Write.Ports;
using Infra.Tests.Tooling;
using Infra.Write;
using Tooling;

namespace Infra.Tests.Write;

public class CsvCategoryImporterTests : InfraTest<ICategoryImporter, CsvCategoryImporter>
{
    [Fact]
    public async Task Parses_categories() =>
        await this.Verify(
            """
            Label;Parent label
            A label;
            Another label;A parent label
            Yet another label;Another parent label
            """,
            new CategoryToImport(new Label("A label"), null),
            new CategoryToImport(new Label("Another label"), new Label("A parent label")),
            new CategoryToImport(new Label("Yet another label"), new Label("Another parent label"))
        );

    [Fact]
    public async Task Tells_when_empty() =>
        await this.Verify(string.Empty);

    [Fact]
    public async Task Tells_when_contain_only_headers() =>
        await this.Verify("A single row");

    private async Task Verify(string content, params CategoryToImport[] expected)
    {
        CategoryToImport[] categories = await this.Sut.Parse(content.ToUtf8Stream());
        categories.Should().Equal(expected);
    }
}