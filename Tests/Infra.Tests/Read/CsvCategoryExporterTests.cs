using App.Read.Ports;
using Infra.Read;
using Infra.Tests.Tooling;

namespace Infra.Tests.Read;

public class CsvCategoryExporterTests : InfraTest<ICategoryExporter, CsvCategoryExporter>
{
    [Theory, RandomData]
    public async Task Exports(CategorySummaryPresentation aCategory, CategorySummaryPresentation anotherCategory) =>
        await this.Verify(
            [aCategory, anotherCategory],
            $"""
             Label,Keywords
             {aCategory.Label},{aCategory.Keywords}
             {anotherCategory.Label},{anotherCategory.Keywords}
             """
        );

    [Fact]
    public async Task Exports_when_no_category() =>
        await this.Verify([], "Label,Keywords");

    private async Task Verify(CategorySummaryPresentation[] categories, string expected)
    {
        Stream actual = await this.Sut.Export(categories);
        actual.Should().Equal(expected);
    }
}