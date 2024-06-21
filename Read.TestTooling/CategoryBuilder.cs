using Shared.Presentation;
using Write.App.Model.Categories;

namespace Read.TestTooling;

public record CategoryBuilder(Guid Id, string Label, string Pattern)
{
    public static CategoryBuilder Create() =>
        new(Guid.NewGuid(), Guid.NewGuid().ToString(), "pattern");

    public Category Build() =>
        Category.From(new CategorySnapshot(this.Id, this.Label, this.Pattern));

    public CategorySummaryPresentation ToSummary() =>
        new(this.Id, this.Label, this.Pattern);
}