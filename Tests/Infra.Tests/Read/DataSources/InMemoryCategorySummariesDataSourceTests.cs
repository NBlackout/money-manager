using App.Read.Ports;
using App.Tests.Read.Tooling;
using App.Write.Ports;
using Infra.Read.DataSources;
using Infra.Tests.Tooling;
using Infra.Write.Repositories;

namespace Infra.Tests.Read.DataSources;

public class InMemoryCategorySummariesDataSourceTests : InfraTest<ICategorySummariesDataSource, InMemoryCategorySummariesDataSource>
{
    private readonly InMemoryCategoryRepository categoryRepository;

    public InMemoryCategorySummariesDataSourceTests()
    {
        this.categoryRepository = this.Resolve<ICategoryRepository, InMemoryCategoryRepository>();
    }

    [Fact]
    public async Task Gives_categories()
    {
        CategoryBuilder aCategory = ACategory();
        CategoryBuilder anotherCategory = ACategory();
        this.Feed(aCategory, anotherCategory);

        await this.Verify(PresentationFrom(aCategory), PresentationFrom(anotherCategory));
    }

    [Fact]
    public async Task Gives_category_children()
    {
        CategoryBuilder parentCategory = ACategory();
        CategoryBuilder aChildCategory = ACategory() with { ParentId = parentCategory.Id };
        CategoryBuilder anotherChildCategory = ACategory() with { ParentId = parentCategory.Id };
        this.Feed(parentCategory, aChildCategory, anotherChildCategory);

        await this.Verify(PresentationFrom(parentCategory) with { Children = PresentationsFrom(aChildCategory, anotherChildCategory) });
    }

    [Fact]
    public async Task Tells_when_there_is_no_category()
    {
        await this.Verify();
    }

    private async Task Verify(params CategorySummaryPresentation[] expected)
    {
        CategorySummaryPresentation[] actual = await this.Sut.All();
        actual.Should().Equal(expected);
    }

    private void Feed(params CategoryBuilder[] expected) =>
        this.categoryRepository.Feed([..expected.Select(c => c.ToSnapshot())]);

    private static CategoryBuilder ACategory() =>
        Any<CategoryBuilder>() with { ParentId = null };

    private static CategorySummaryPresentation PresentationFrom(CategoryBuilder category) =>
        category.ToSummary();

    private static ChildCategorySummaryPresentation[] PresentationsFrom(params CategoryBuilder[] categories) =>
        categories.Select(c => c.ToChildSummary()).ToArray();
}