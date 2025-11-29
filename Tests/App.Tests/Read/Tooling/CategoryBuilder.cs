using App.Read.Ports;
using App.Write.Model.Categories;

namespace App.Tests.Read.Tooling;

public record CategoryBuilder(Guid Id, string Label, Guid? ParentId)
{
    public CategorySnapshot ToSnapshot() =>
        new(new CategoryId(this.Id), this.Label, this.ParentId != null ? new CategoryId(this.ParentId.Value) : null);

    public CategorySummaryPresentation ToSummary() =>
        new(this.Id, this.Label);

    public ChildCategorySummaryPresentation ToChildSummary() =>
        new(this.Id, this.Label);
}