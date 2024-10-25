namespace Write.App.Model.Categories;

public class Category : DomainEntity<CategoryId>
{
    private readonly Label label;
    private readonly string keywords;

    public CategorySnapshot Snapshot => new(this.Id, this.label.Value, this.keywords);

    internal Category(CategoryId id, Label label) : this(id, label, label.Value)
    {
    }

    internal Category(CategoryId id, Label label, string keywords) : base(id)
    {
        this.label = label;
        this.keywords = keywords;
    }

    public static Category From(CategorySnapshot snapshot) =>
        new(snapshot.Id, new Label(snapshot.Label), snapshot.Keywords);
}