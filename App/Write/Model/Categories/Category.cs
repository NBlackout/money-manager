using App.Write.Model.CategorizationRules;
using App.Write.Model.ValueObjects;

namespace App.Write.Model.Categories;

public class Category : DomainEntity<CategoryId, CategorySnapshot>
{
    private Label label;
    private readonly CategoryId? parentId;

    public override CategorySnapshot Snapshot => new(this.Id, this.label.Value, this.parentId);

    public Category(CategorySnapshot snapshot) : base(snapshot)
    {
        this.label = new Label(snapshot.Label);
        this.parentId = snapshot.ParentId;
    }

    internal Category(CategoryId id, Label label, CategoryId? parentId) : base(id)
    {
        this.label = label;
        this.parentId = parentId;
    }

    public CategorizationRule ApplyWhenMatches(CategorizationRuleId id, string keywords, Amount? amount, Amount? margin) =>
        new(id, this.Id, keywords, amount, margin);

    public void Rename(Label newLabel) =>
        this.label = newLabel;
}