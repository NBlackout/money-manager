using Write.App.Model.Categories;

namespace Write.TestTooling;

public record CategoryBuilder(Guid Id, string Label, string Keywords)
{
    public Category Build() =>
        Category.From(this.ToSnapshot());

    public CategorySnapshot ToSnapshot() =>
        new(this.Id, this.Label, this.Keywords);
}
