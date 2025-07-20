using System.Diagnostics.CodeAnalysis;
using App.Read.Ports;
using App.Write.Model.Categories;

namespace App.Tests.Read.Tooling;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public record CategoryBuilder(Guid Id, string Label, string Keywords)
{
    public CategorySnapshot ToSnapshot() =>
        new(new CategoryId(this.Id), this.Label, this.Keywords);

    public CategorySummaryPresentation ToSummary() =>
        new(this.Id, this.Label, this.Keywords);
}