namespace MoneyManager.Write.Application.Model;

public class Bank : DomainEntity
{
    private readonly string externalId;
    private string name;

    public BankSnapshot Snapshot =>
        new(this.Id, this.externalId, this.name);

    private Bank(Guid id, string externalId, string name) : base(id)
    {
        this.externalId = externalId;
        this.name = name;
    }

    public static Bank From(BankSnapshot snapshot) =>
        new(snapshot.Id, snapshot.ExternalId, snapshot.Name);

    public static Bank Track(Guid id, string bankIdentifier) =>
        new(id, bankIdentifier, bankIdentifier);

    public Account TrackAccount(Guid id, string accountNumber, decimal balance) =>
        Account.StartTracking(id, this.Id, accountNumber, balance);

    public void AssignName(string newName) =>
        this.name = newName;
}