using Write.App.Model.Categories;

namespace Write.App.Tests.UseCases;

public class DeleteCategoryTests
{
    private readonly InMemoryCategoryRepository repository = new();
    private readonly DeleteCategory sut;

    public DeleteCategoryTests()
    {
        this.sut = new DeleteCategory(this.repository);
    }

    [Theory, RandomData]
    public async Task Deletes_category(CategorySnapshot category)
    {
        this.Feed(category);
        await this.Verify(category);
    }

    private async Task Verify(CategorySnapshot category)
    {
        await this.sut.Execute(category.Id);
        bool exists = this.repository.Exists(category.Id);
        exists.Should().BeFalse();
    }

    private void Feed(CategorySnapshot category) =>
        this.repository.Feed(category);
}