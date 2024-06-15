using Shared.Presentation;
using Write.App.Model.Categories;

namespace Read.TestTooling;

public record CategoryBuilder(Guid Id, string Label)
{
    public static CategoryBuilder For(Guid id) =>
        new(id, id.ToString());

    public Category Build() =>
        Category.From(new CategorySnapshot(this.Id, this.Label));

    public CategorySummaryPresentation ToSummary() =>
        new(this.Id, this.Label);
}