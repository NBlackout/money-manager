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
            Label
            A label
            Another label
            Yet another label
            """,
            new CategoryToImport(new Label("A label")),
            new CategoryToImport(new Label("Another label")),
            new CategoryToImport(new Label("Yet another label"))
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