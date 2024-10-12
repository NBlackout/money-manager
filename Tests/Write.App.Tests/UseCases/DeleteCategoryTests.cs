namespace Write.App.Tests.UseCases;

// TODO prevent deletion of used category 
public class DeleteCategoryTests
{
    private readonly InMemoryCategoryRepository repository = new();
    private readonly DeleteCategory sut;

    public DeleteCategoryTests()
    {
        this.sut = new DeleteCategory(this.repository);
    }

    [Theory, RandomData]
    public async Task Deletes_category(CategoryBuilder category)
    {
        this.repository.Feed(category.Build());
        await this.sut.Execute(category.Id);
        bool exists = this.repository.Exists(category.Id);
        exists.Should().BeFalse();
    }
}