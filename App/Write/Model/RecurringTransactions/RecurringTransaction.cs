using App.Write.Model.Categories;
using App.Write.Model.ValueObjects;

namespace App.Write.Model.RecurringTransactions;

public class RecurringTransaction : DomainEntity<RecurringTransactionId, RecurringTransactionSnapshot>
{
    private readonly Amount amount;
    private readonly Label label;
    private readonly DateOnly date;
    private readonly CategoryId? categoryId;

    public override RecurringTransactionSnapshot Snapshot => new(this.Id, this.amount.Value, this.label.Value, this.date, this.categoryId);

    public RecurringTransaction(RecurringTransactionSnapshot snapshot) : base(snapshot)
    {
        this.amount = new Amount(snapshot.Amount);
        this.label = new Label(snapshot.Label);
        this.date = snapshot.Date;
        this.categoryId = snapshot.CategoryId;
    }

    internal RecurringTransaction(RecurringTransactionId id, Amount amount, Label label, DateOnly date, CategoryId? categoryId) : base(id)
    {
        this.amount = amount;
        this.label = label;
        this.date = date;
        this.categoryId = categoryId;
    }
}