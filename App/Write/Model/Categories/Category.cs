using App.Write.Model.CategorizationRules;
using App.Write.Model.ValueObjects;

namespace App.Write.Model.Categories;

public class Category : DomainEntity<CategoryId>
{
    private readonly Label label;

    public CategorySnapshot Snapshot => new(this.Id, this.label.Value);

    internal Category(CategoryId id, Label label) : base(id)
    {
        this.label = label;
    }

    public static Category From(CategorySnapshot snapshot) =>
        new(snapshot.Id, new Label(snapshot.Label));

    public CategorizationRule ApplyWhenMatches(CategorizationRuleId id, string keywords) =>
        new(id, this.Id, keywords);
}