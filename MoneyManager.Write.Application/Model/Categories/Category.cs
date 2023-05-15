namespace MoneyManager.Write.Application.Model.Categories;

public class Category : DomainEntity
{
    private readonly string label;

    public CategorySnapshot Snapshot => new(this.Id, this.label);

    internal Category(Guid id, string label) : base(id)
    {
        this.label = label;
    }

    public static Category From(CategorySnapshot snapshot) =>
        new(snapshot.Id, snapshot.Label);
}