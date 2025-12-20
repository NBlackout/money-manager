using App.Write.Model.ValueObjects;

namespace App.Write.Model.Budgets;

public class Budget : DomainEntity<BudgetId, BudgetSnapshot>
{
    private readonly Label name;
    private readonly Amount amount;
    private readonly DateOnly beginDate;

    public override BudgetSnapshot Snapshot => new(this.Id, this.name.Value, this.amount.Value, this.beginDate);

    public Budget(BudgetSnapshot snapshot) : base(snapshot)
    {
        this.name = new Label(snapshot.Name);
        this.amount = new Amount(snapshot.Amount);
        this.beginDate = snapshot.BeginDate;
    }

    internal Budget(BudgetId id, Label name, Amount amount, DateOnly beginDate) : base(id)
    {
        this.name = name;
        this.amount = amount;
        this.beginDate = beginDate;
    }
}