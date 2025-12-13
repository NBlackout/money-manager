using App.Write.Model.Categories;
using App.Write.Model.ValueObjects;

namespace App.Write.Model.RecurringTransactions;

public class RecurringTransaction : DomainEntity<RecurringTransactionId>
{
    private readonly Amount amount;
    private readonly Label label;
    private readonly DateOnly date;
    private readonly CategoryId? categoryId;

    public RecurringTransactionSnapshot Snapshot => new(this.Id, this.amount.Value, this.label.Value, this.date, this.categoryId);

    internal RecurringTransaction(RecurringTransactionId id, Amount amount, Label label, DateOnly date, CategoryId? categoryId) : base(id)
    {
        this.amount = amount;
        this.label = label;
        this.date = date;
        this.categoryId = categoryId;
    }

    public static RecurringTransaction From(RecurringTransactionSnapshot snapshot) =>
        new(snapshot.Id, new Amount(snapshot.Amount), new Label(snapshot.Label), snapshot.Date, snapshot.CategoryId);
}