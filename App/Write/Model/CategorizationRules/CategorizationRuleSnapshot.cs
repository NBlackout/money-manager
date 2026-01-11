using App.Write.Model.Categories;

namespace App.Write.Model.CategorizationRules;

public record CategorizationRuleSnapshot(CategorizationRuleId Id, CategoryId CategoryId, string Keywords, decimal? Amount, decimal? Margin)
    : ISnapshot<CategorizationRuleId>;