using App.Read.Ports;
using App.Write.Model.Categories;
using App.Write.Model.CategorizationRules;

namespace App.Tests.Read.Tooling;

public record CategorizationRuleBuilder(Guid Id, Guid CategoryId, string CategoryLabel, string Keywords, decimal? Amount)
{
    public CategorizationRuleSnapshot ToSnapshot() =>
        new(new CategorizationRuleId(this.Id), new CategoryId(this.CategoryId), this.Keywords, this.Amount);

    public CategorySnapshot ToCategorySnapshot() =>
        new(new CategoryId(this.CategoryId), this.CategoryLabel, null);

    public CategorizationRuleSummaryPresentation ToSummary() =>
        new(this.Id, this.CategoryId, this.CategoryLabel, this.Keywords, this.Amount);
}