using App.Write.Model.CategorizationRules;
using App.Write.Model.ValueObjects;

namespace App.Write.Model.Categories;

public class Category : DomainEntity<CategoryId>
{
    private Label label;
    private readonly CategoryId? parentId;

    public CategorySnapshot Snapshot => new(this.Id, this.label.Value, this.parentId);

    internal Category(CategoryId id, Label label, CategoryId? parentId) : base(id)
    {
        this.label = label;
        this.parentId = parentId;
    }

    public static Category From(CategorySnapshot snapshot) =>
        new(snapshot.Id, new Label(snapshot.Label), snapshot.ParentId);

    public CategorizationRule ApplyWhenMatches(CategorizationRuleId id, string keywords) =>
        new(id, this.Id, keywords);

    public void Rename(Label newLabel) =>
        this.label = newLabel;
}