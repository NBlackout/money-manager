using MoneyManager.Write.Application.Model.Categories;

namespace MoneyManager.Write.Application.Tests.UseCases;

public class CreateCategoryTests
{
    private readonly InMemoryCategoryRepository repository;
    private readonly CreateCategory sut;

    public CreateCategoryTests()
    {
        this.repository = new InMemoryCategoryRepository();
        this.sut = new CreateCategory(this.repository);
    }

    [Fact]
    public async Task Should_create_category()
    {
        CategorySnapshot expected = new(Guid.NewGuid(), "A label");

        await this.sut.Execute(expected.Id, expected.Label);

        Category actual = await this.repository.ById(expected.Id);
        actual.Snapshot.Should().Be(expected);
    }
}