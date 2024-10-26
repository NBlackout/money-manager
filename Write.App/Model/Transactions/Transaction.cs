namespace Write.App.Model.Transactions;

public class Transaction : DomainEntity<TransactionId>
{
    private readonly AccountId accountId;
    private readonly ExternalId externalId;
    private readonly decimal amount;
    private readonly Label label;
    private readonly DateOnly date;
    private CategoryId? categoryId;

    public TransactionSnapshot Snapshot =>
        new(this.Id, this.accountId, this.externalId.Value, this.amount, this.label.Value, this.date, this.categoryId);

    internal Transaction(TransactionId id, AccountId accountId, ExternalId externalId, decimal amount, Label label,
        DateOnly date,
        Category? category) :
        this(id, accountId, externalId, amount, label, date, category?.Id)
    {
    }

    private Transaction(TransactionId id, AccountId accountId, ExternalId externalId, decimal amount, Label label,
        DateOnly date,
        CategoryId? categoryId) : base(id)
    {
        this.accountId = accountId;
        this.externalId = externalId;
        this.amount = amount;
        this.label = label;
        this.date = date;
        this.categoryId = categoryId;
    }

    public void AssignCategory(CategoryId newCategoryId) =>
        this.categoryId = newCategoryId;

    public static Transaction From(TransactionSnapshot snapshot)
    {
        return new Transaction(snapshot.Id, snapshot.AccountId, new ExternalId(snapshot.ExternalId), snapshot.Amount,
            new Label(snapshot.Label), snapshot.Date, snapshot.CategoryId);
    }
}