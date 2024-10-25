namespace Write.App.Model.Categories;

public class Category : DomainEntity
{
    private readonly string label;
    private readonly string keywords;

    public CategorySnapshot Snapshot => new(this.Id, this.label, this.keywords);

    internal Category(Guid id, string label) : this(id, label, label)
    {
    }

    internal Category(Guid id, string label, string keywords) : base(id)
    {
        this.label = label;
        this.keywords = keywords;
    }

    public static Category From(CategorySnapshot snapshot) =>
        new(snapshot.Id, snapshot.Label, snapshot.Keywords);
}