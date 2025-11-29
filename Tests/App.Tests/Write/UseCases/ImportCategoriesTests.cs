using App.Tests.Write.TestDoubles;
using App.Write.Model.Categories;
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
    public async Task Imports_many_categories(CategoryToImport aCategory, CategoryToImport anotherCategory, CategoryId anId, CategoryId anotherId)
    {
        this.SetImportResultTo(aCategory, anotherCategory);
        this.FeedNextIdsTo(anId, anotherId);
        await this.Verify(ExpectedFrom(aCategory) with { Id = anId }, ExpectedFrom(anotherCategory) with { Id = anotherId });
    }

    private async Task Verify(params CategorySnapshot[] expected)
    {
        await this.sut.Execute(new MemoryStream(Content));
        this.categoryRepository.Data.ToArray().Should().Equal(expected);
    }

    private void SetImportResultTo(params CategoryToImport[] categories) =>
        this.categoryImporter.Feed(new MemoryStream(Content), categories);

    private void FeedNextIdsTo(params CategoryId[] ids)
    {
        int nextIdIndex = 0;

        this.categoryRepository.NextId = () => ids[nextIdIndex++];
    }

    private static CategorySnapshot ExpectedFrom(CategoryToImport category) =>
        new(Any<CategoryId>(), category.Label.Value);
}