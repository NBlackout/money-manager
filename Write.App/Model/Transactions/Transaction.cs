namespace Write.App.Model.Transactions;

public class Transaction : DomainEntity
{
    private readonly Guid accountId;
    private readonly string externalId;
    private readonly decimal amount;
    private readonly string label;
    private readonly DateOnly date;
    private Guid? categoryId;

    public TransactionSnapshot Snapshot =>
        new(this.Id, this.accountId, this.externalId, this.amount, this.label, this.date, this.categoryId);

    internal Transaction(Guid id, Guid accountId, string externalId, decimal amount, string label, DateOnly date,
        Category? category) :
        this(id, accountId, externalId, amount, label, date, category?.Id)
    {
    }

    private Transaction(Guid id, Guid accountId, string externalId, decimal amount, string label, DateOnly date,
        Guid? categoryId) : base(id)
    {
        this.accountId = accountId;
        this.externalId = externalId;
        this.amount = amount;
        this.label = label;
        this.date = date;
        this.categoryId = categoryId;
    }

    public static Transaction From(TransactionSnapshot snapshot)
    {
        return new Transaction(snapshot.Id, snapshot.AccountId, snapshot.ExternalId, snapshot.Amount, snapshot.Label,
            snapshot.Date, snapshot.CategoryId);
    }

    public void AssignCategory(Guid newCategoryId) =>
        this.categoryId = newCategoryId;
}
