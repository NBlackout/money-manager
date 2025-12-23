using App.Write.Model.Categories;
using App.Write.Model.ValueObjects;
using App.Write.UseCases;
using Infra.Write.Repositories;
using static App.Tests.Write.Tooling.SnapshotHelpers;

namespace App.Tests.Write.UseCases;

public class RenameCategoryTests
{
    private readonly InMemoryCategoryRepository repository = new();
    private readonly RenameCategory sut;
    private readonly CategorySnapshot existingCategory = ACategory();

    public RenameCategoryTests()
    {
        this.sut = new RenameCategory(this.repository);
        this.Feed(this.existingCategory);
    }

    [Theory, RandomData]
    public async Task Renames_category(Label label) =>
        await this.Verify(label);

    private async Task Verify(Label label)
    {
        await this.sut.Execute(this.existingCategory.Id, label);

        Category actual = await this.repository.By(this.existingCategory.Id);
        actual.Snapshot.Should().Be(this.existingCategory with { Label = label.Value });
    }

    private void Feed(CategorySnapshot category) =>
        this.repository.Feed(category);
}