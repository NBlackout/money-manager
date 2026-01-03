using App.Write.Model.Accounts;
using App.Write.Model.Categories;
using App.Write.Model.RecurringTransactions;
using App.Write.Model.ValueObjects;

namespace App.Write.Model.Transactions;

public class Transaction : DomainEntity<TransactionId, TransactionSnapshot>
{
    private readonly AccountId accountId;
    private readonly ExternalId externalId;
    private readonly Amount amount;
    private readonly Label label;
    private Label? preferredLabel;
    private readonly DateOnly date;
    private CategoryId? categoryId;
    private bool isRecurring;

    public override TransactionSnapshot Snapshot =>
        new(
            this.Id,
            this.accountId,
            this.externalId.Value,
            this.amount.Value,
            this.label.Value,
            this.preferredLabel?.Value,
            this.date,
            this.categoryId,
            this.isRecurring
        );

    public Transaction(TransactionSnapshot snapshot) : base(snapshot)
    {
        this.accountId = snapshot.AccountId;
        this.externalId = new ExternalId(snapshot.ExternalId);
        this.amount = new Amount(snapshot.Amount);
        this.label = new Label(snapshot.Label);
        this.preferredLabel = snapshot.PreferredLabel != null ? new Label(snapshot.PreferredLabel) : null;
        this.date = snapshot.Date;
        this.categoryId = snapshot.CategoryId;
        this.isRecurring = snapshot.IsRecurring;
    }

    private Transaction(
        TransactionId id,
        AccountId accountId,
        ExternalId externalId,
        Amount amount,
        Label label,
        Label? preferredLabel,
        DateOnly date,
        CategoryId? categoryId,
        bool isRecurring) : base(id)
    {
        this.accountId = accountId;
        this.externalId = externalId;
        this.amount = amount;
        this.label = label;
        this.preferredLabel = preferredLabel;
        this.date = date;
        this.categoryId = categoryId;
        this.isRecurring = isRecurring;
    }

    internal Transaction(TransactionId id, AccountId accountId, ExternalId externalId, Amount amount, Label label, DateOnly date, Category? category) : this(
        id,
        accountId,
        externalId,
        amount,
        label,
        null,
        date,
        category?.Id,
        false
    )
    {
    }

    public void AssignCategory(CategoryId newCategoryId) =>
        this.categoryId = newCategoryId;

    public void UnassignCategory() =>
        this.categoryId = null;

    public void Prefer(Label preferredLabel) =>
        this.preferredLabel = preferredLabel;

    public RecurringTransaction MarkAsRecurring(RecurringTransactionId id)
    {
        if (this.isRecurring)
            throw new TransactionAlreadyMarkedAsRecurringException();

        this.isRecurring = true;

        return new RecurringTransaction(id, this.amount, this.label, this.date.AddMonths(1), this.categoryId);
    }
}