using App.Read.Ports;
using Infra.Read;
using Infra.Tests.Tooling;

namespace Infra.Tests.Read;

public class CsvCategoryExporterTests : InfraTest<ICategoryExporter, CsvCategoryExporter>
{
    [Theory, RandomData]
    public async Task Exports(
        CategorySummaryPresentation aParentCategory,
        ChildCategorySummaryPresentation aChildCategory,
        ChildCategorySummaryPresentation anotherChildCategory,
        CategorySummaryPresentation anotherParentCategory,
        ChildCategorySummaryPresentation yetAnotherChildCategory)
    {
        aParentCategory = aParentCategory with { Children = [aChildCategory, anotherChildCategory] };
        anotherParentCategory = anotherParentCategory with { Children = [yetAnotherChildCategory] };
        await this.Verify(
            [aParentCategory, anotherParentCategory],
            $"""
             Label;Parent label
             {aParentCategory.Label};
             {aChildCategory.Label};{aParentCategory.Label}
             {anotherChildCategory.Label};{aParentCategory.Label}
             {anotherParentCategory.Label};
             {yetAnotherChildCategory.Label};{anotherParentCategory.Label}
             """
        );
    }

    [Fact]
    public async Task Exports_when_no_category() =>
        await this.Verify([], "Label;Parent label");

    private async Task Verify(CategorySummaryPresentation[] categories, string expected)
    {
        Stream actual = await this.Sut.Export(categories);
        actual.Should().Equal(expected);
    }
}