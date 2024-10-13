using System.Diagnostics.CodeAnalysis;
using Shared.Presentation;
using Write.App.Model.Categories;

namespace Read.TestTooling;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public record CategoryBuilder(Guid Id, string Label, string Keywords)
{
    public CategorySnapshot ToSnapshot() =>
        new(this.Id, this.Label, this.Keywords);

    public CategorySummaryPresentation ToSummary() =>
        new(this.Id, this.Label, this.Keywords);
}