using App.Write.Model.ValueObjects;

namespace App.Write.Model.Budgets;

public class Budget : DomainEntity<BudgetId>
{
    private readonly Label name;
    private readonly Amount amount;
    private readonly DateOnly beginDate;

    public BudgetSnapshot Snapshot => new(this.Id, this.name.Value, this.amount.Value, this.beginDate);

    internal Budget(BudgetId id, Label name, Amount amount, DateOnly beginDate) : base(id)
    {
        this.name = name;
        this.amount = amount;
        this.beginDate = beginDate;
    }

    public static Budget From(BudgetSnapshot snapshot) =>
        new(snapshot.Id, new Label(snapshot.Name), new Amount(snapshot.Amount), snapshot.BeginDate);
}