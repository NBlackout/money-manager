using Shared.Presentation;
using Write.App.Model.Categories;

namespace Read.TestTooling;

public record CategoryBuilder(Guid Id, string Label, string Keywords)
{
    public Category Build() =>
        Category.From(new CategorySnapshot(this.Id, this.Label, this.Keywords));

    public CategorySummaryPresentation ToSummary() =>
        new(this.Id, this.Label, this.Keywords);
}
