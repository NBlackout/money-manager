using App.Read.Ports;
using App.Write.Model.Categories;

namespace App.Tests.Read.Tooling;

public record CategoryBuilder(Guid Id, string Label)
{
    public CategorySnapshot ToSnapshot() =>
        new(new CategoryId(this.Id), this.Label);

    public CategorySummaryPresentation ToSummary() =>
        new(this.Id, this.Label);
}