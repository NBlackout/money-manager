using App.Read.Ports;
using App.Read.UseCases;
using App.Tests.Read.TestDoubles;

namespace App.Tests.Read.UseCases;

public class CategoriesExportTests
{
    private readonly StubbedCategorySummariesDataSource dataSource = new();
    private readonly StubbedCategoryExporter exporter = new();
    private readonly CategoriesExport sut;

    public CategoriesExportTests()
    {
        this.sut = new CategoriesExport(this.dataSource, this.exporter);
    }

    [Theory, RandomData]
    public async Task Gives_categories_export(CategorySummaryPresentation[] categories, Stream content)
    {
        this.Feed(categories, content);
        await this.Verify(content);
    }

    private async Task Verify(Stream expected)
    {
        Stream actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }

    private void Feed(CategorySummaryPresentation[] categories, Stream content)
    {
        this.dataSource.Feed(categories);
        this.exporter.Feed(categories, content);
    }
}