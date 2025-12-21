using App.Tests.Write.TestDoubles;
using App.Write.Model.Categories;
using App.Write.Model.ValueObjects;
using App.Write.Ports;
using App.Write.UseCases;
using Infra.Write.Repositories;

namespace App.Tests.Write.UseCases;

public class ImportCategoriesTests
{
    private static readonly byte[] Content = Any<byte[]>();

    private readonly InMemoryCategoryRepository categoryRepository = new();
    private readonly StubbedCategoryImporter categoryImporter = new();
    private readonly ImportCategories sut;

    public ImportCategoriesTests()
    {
        this.sut = new ImportCategories(this.categoryRepository, this.categoryImporter);
    }

    [Theory, RandomData]
    public async Task Imports_categories(CategoryId anId, CategoryId anotherId)
    {
        CategoryToImport aCategory = ACategoryToImport();
        CategoryToImport anotherCategory = ACategoryToImport();
        this.SetImportResultTo(aCategory, anotherCategory);
        this.FeedNextIdsTo(anId, anotherId);

        await this.Verify(ExpectedFrom(aCategory) with { Id = anId }, ExpectedFrom(anotherCategory) with { Id = anotherId });
    }

    [Theory, RandomData]
    public async Task Links_to_existing_parent(CategorySnapshot parentCategory, CategoryId anId)
    {
        CategoryToImport aCategory = ACategoryToImport() with { ParentLabel = new Label(parentCategory.Label) };
        this.Feed(parentCategory);
        this.SetImportResultTo(aCategory);
        this.FeedNextIdsTo(anId);

        await this.Verify(parentCategory, ExpectedFrom(aCategory) with { Id = anId, ParentId = parentCategory.Id });
    }

    [Theory, RandomData]
    public async Task Links_to_new_parent(CategoryId anId, CategoryId anotherId)
    {
        CategoryToImport parentCategory = ACategoryToImport();
        CategoryToImport childCategory = ACategoryToImport() with { ParentLabel = parentCategory.Label };
        this.SetImportResultTo(parentCategory, childCategory);
        this.FeedNextIdsTo(anId, anotherId);

        await this.Verify(ExpectedFrom(parentCategory) with { Id = anId }, ExpectedFrom(childCategory) with { Id = anotherId, ParentId = anId });
    }

    [Theory, RandomData]
    public async Task Imports_parent_first(Label parentLabel, CategoryId anId, CategoryId anotherId)
    {
        CategoryToImport childCategory = ACategoryToImport() with { ParentLabel = parentLabel };
        CategoryToImport parentCategory = ACategoryToImport() with { Label = parentLabel };
        this.SetImportResultTo(childCategory, parentCategory);
        this.FeedNextIdsTo(anId, anotherId);

        await this.Verify(ExpectedFrom(parentCategory) with { Id = anId }, ExpectedFrom(childCategory) with { Id = anotherId, ParentId = anId });
    }

    [Theory, RandomData]
    public async Task Skips_existing_ones(CategorySnapshot category)
    {
        this.SetImportResultTo(ACategoryToImport() with { Label = new Label(category.Label) });
        this.Feed(category);
        await this.Verify(category);
    }

    private async Task Verify(params CategorySnapshot[] expected)
    {
        await this.sut.Execute(new MemoryStream(Content));
        this.categoryRepository.Data.ToArray().Should().Equal(expected);
    }

    private void SetImportResultTo(params CategoryToImport[] categories) =>
        this.categoryImporter.Feed(new MemoryStream(Content), categories);

    private void Feed(CategorySnapshot category) =>
        this.categoryRepository.Feed(category);

    private void FeedNextIdsTo(params CategoryId[] ids)
    {
        int nextIdIndex = 0;

        this.categoryRepository.NextId = () => ids[nextIdIndex++];
    }

    private static CategoryToImport ACategoryToImport() =>
        Any<CategoryToImport>() with { ParentLabel = null };

    private static CategorySnapshot ExpectedFrom(CategoryToImport category) =>
        new(Any<CategoryId>(), category.Label.Value, null);
}