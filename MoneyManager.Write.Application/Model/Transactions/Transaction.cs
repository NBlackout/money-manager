namespace MoneyManager.Write.Application.Model.Transactions;

public class Transaction : DomainEntity
{
    private readonly Guid accountId;
    private readonly string externalId;
    private readonly decimal amount;

    public TransactionSnapshot Snapshot =>
        new(this.Id, this.accountId, this.externalId, this.amount);

    internal Transaction(Guid id, Guid accountId, string externalId, decimal amount) : base(id)
    {
        this.accountId = accountId;
        this.externalId = externalId;
        this.amount = amount;
    }

    public static Transaction From(TransactionSnapshot snapshot) =>
        new(snapshot.Id, snapshot.AccountId, snapshot.ExternalId, snapshot.Amount);
}