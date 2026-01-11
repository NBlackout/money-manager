using App.Write.Model.Categories;
using App.Write.Model.ValueObjects;

namespace App.Write.Model.CategorizationRules;

public class CategorizationRule : DomainEntity<CategorizationRuleId, CategorizationRuleSnapshot>
{
    private readonly CategoryId categoryId;
    private readonly string keywords;
    private readonly Amount? amount;
    private readonly Amount? margin;

    public override CategorizationRuleSnapshot Snapshot => new(this.Id, this.categoryId, this.keywords, this.amount?.Value, this.margin?.Value);

    public CategorizationRule(CategorizationRuleSnapshot snapshot) : base(snapshot)
    {
        this.categoryId = snapshot.CategoryId;
        this.keywords = snapshot.Keywords;
        this.amount = snapshot.Amount.HasValue ? new Amount(snapshot.Amount.Value) : null;
        this.margin = snapshot.Margin.HasValue ? new Amount(snapshot.Margin.Value) : null;
    }

    internal CategorizationRule(CategorizationRuleId id, CategoryId categoryId, string keywords, Amount? amount, Amount? margin) : base(id)
    {
        this.categoryId = categoryId;
        this.keywords = keywords;
        this.amount = amount;
        this.margin = margin;
    }
}