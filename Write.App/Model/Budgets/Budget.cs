namespace Write.App.Model.Budgets;

public class Budget : DomainEntity<BudgetId>
{
    private readonly string name;
    private readonly decimal amount;
    private readonly DateOnly beginDate;

    public BudgetSnapshot Snapshot => new(this.Id, this.name, this.amount, this.beginDate);

    internal Budget(BudgetId id, string name, decimal amount, DateOnly beginDate) : base(id)
    {
        this.name = name;
        this.amount = amount;
        this.beginDate = beginDate;
    }

    public static Budget From(BudgetSnapshot snapshot) =>
        new(snapshot.Id, snapshot.Name, snapshot.Amount, snapshot.BeginDate);
}