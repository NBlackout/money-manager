namespace MoneyManager.Write.Application.Model;

public class Bank : DomainEntity
{
    private readonly string externalId;

    public BankSnapshot Snapshot =>
        new(this.Id, this.externalId);

    private Bank(Guid id, string externalId) : base(id)
    {
        this.externalId = externalId;
    }

    public static Bank From(BankSnapshot snapshot) =>
        new(snapshot.Id, snapshot.ExternalId);

    public static Bank Track(Guid id, string bankIdentifier) =>
        new(id, bankIdentifier);

    public Account TrackAccount(Guid id, string accountNumber, decimal balance) =>
        Account.StartTracking(id, this.Id, accountNumber, balance);
}