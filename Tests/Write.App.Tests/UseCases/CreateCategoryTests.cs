using Write.App.Model.Categories;

namespace Write.App.Tests.UseCases;

public class CreateCategoryTests
{
    private readonly InMemoryCategoryRepository repository = new();
    private readonly CreateCategory sut;

    public CreateCategoryTests()
    {
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