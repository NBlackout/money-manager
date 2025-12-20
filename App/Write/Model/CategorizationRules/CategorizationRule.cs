using App.Write.Model.Categories;

namespace App.Write.Model.CategorizationRules;

public class CategorizationRule : DomainEntity<CategorizationRuleId, CategorizationRuleSnapshot>
{
    private readonly CategoryId categoryId;
    private readonly string keywords;

    public override CategorizationRuleSnapshot Snapshot => new(this.Id, this.categoryId, this.keywords);

    public CategorizationRule(CategorizationRuleSnapshot snapshot) : base(snapshot)
    {
        this.categoryId = snapshot.CategoryId;
        this.keywords = snapshot.Keywords;
    }

    internal CategorizationRule(CategorizationRuleId id, CategoryId categoryId, string keywords) : base(id)
    {
        this.categoryId = categoryId;
        this.keywords = keywords;
    }
}