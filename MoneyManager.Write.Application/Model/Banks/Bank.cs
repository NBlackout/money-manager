namespace MoneyManager.Write.Application.Model.Banks;

public class Bank : DomainEntity
{
    private readonly string externalId;

    public BankSnapshot Snapshot =>
        new(this.Id, this.externalId);

    internal Bank(Guid id, string externalId) : base(id)
    {
        this.externalId = externalId;
    }

    public static Bank From(BankSnapshot snapshot) =>
        new(snapshot.Id, snapshot.ExternalId);

    public Account TrackAccount(Guid id, string accountNumber, decimal balance, DateTime balanceDate) =>
        Account.StartTracking(id, this.Id, accountNumber, balance, balanceDate);
}