using App.Write.Model.Categories;
using App.Write.Model.Exceptions;
using App.Write.Model.ValueObjects;
using App.Write.UseCases;
using Infra.Write.Repositories;

namespace App.Tests.Write.UseCases;

public class CreateCategoryTests
{
    private readonly InMemoryCategoryRepository repository = new();
    private readonly CreateCategory sut;

    public CreateCategoryTests()
    {
        this.sut = new CreateCategory(this.repository);
    }

    [Theory]
    [RandomData]
    public async Task Creates_a_new(CategorySnapshot category) =>
        await this.Verify(category);

    [Theory]
    [RandomData]
    public async Task Prevents_duplicate_creation_of(CategorySnapshot existingCategory, CategorySnapshot newCategory)
    {
        this.repository.Feed(existingCategory with { Label = " label " });
        await this.Verify<DuplicateCategoryException>(newCategory with { Label = "  LABEL  " });
    }

    private async Task Verify(CategorySnapshot expected)
    {
        await this.sut.Execute(expected.Id, new Label(expected.Label), expected.ParentId);
        Category actual = await this.repository.By(expected.Id);
        actual.Snapshot.Should().Be(expected);
    }

    private async Task Verify<TException>(CategorySnapshot category) where TException : Exception =>
        await this.Invoking(s => s.Verify(category)).Should().ThrowAsync<TException>();
}