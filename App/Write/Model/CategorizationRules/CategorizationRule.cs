using App.Write.Model.Categories;

namespace App.Write.Model.CategorizationRules;

public class CategorizationRule : DomainEntity<CategorizationRuleId>
{
    private readonly CategoryId categoryId;
    private readonly string keywords;

    public CategorizationRuleSnapshot Snapshot => new(this.Id, this.categoryId, this.keywords);

    internal CategorizationRule(CategorizationRuleId id, CategoryId categoryId, string keywords) : base(id)
    {
        this.categoryId = categoryId;
        this.keywords = keywords;
    }

    public static CategorizationRule From(CategorizationRuleSnapshot snapshot) =>
        new(snapshot.Id, snapshot.CategoryId, snapshot.Keywords);
}