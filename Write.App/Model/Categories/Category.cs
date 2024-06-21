namespace Write.App.Model.Categories;

public class Category : DomainEntity
{
    private readonly string label;
    private readonly string pattern;

    public CategorySnapshot Snapshot => new(this.Id, this.label, this.pattern);

    internal Category(Guid id, string label, string pattern) : base(id)
    {
        this.label = label;
        this.pattern = pattern;
    }

    public static Category From(CategorySnapshot snapshot) =>
        new(snapshot.Id, snapshot.Label, snapshot.Pattern);
}