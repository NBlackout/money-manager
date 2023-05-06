namespace MoneyManager.Write.Application.Model.Transactions;

public class Transaction : DomainEntity
{
    private readonly Guid accountId;
    private readonly string externalId;

    public TransactionSnapshot Snapshot =>
        new(this.Id, this.accountId, this.externalId);

    private Transaction(Guid id, Guid accountId, string externalId) : base(id)
    {
        this.accountId = accountId;
        this.externalId = externalId;
    }

    public static Transaction From(TransactionSnapshot snapshot) =>
        new(snapshot.Id, snapshot.AccountId, snapshot.ExternalId);

    public static Transaction Of(Guid id, Guid accountId, string identifier) =>
        new(id, accountId, identifier);
}