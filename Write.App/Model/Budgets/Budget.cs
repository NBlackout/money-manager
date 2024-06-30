namespace Write.App.Model.Budgets;

public class Budget : DomainEntity
{
    private readonly string name;
    private readonly decimal amount;

    public BudgetSnapshot Snapshot => new(this.Id, this.name, this.amount);

    internal Budget(Guid id, string name, decimal amount) : base(id)
    {
        this.name = name;
        this.amount = amount;
    }

    public static Budget From(BudgetSnapshot snapshot) =>
        new(snapshot.Id, snapshot.Name, snapshot.Amount);
}