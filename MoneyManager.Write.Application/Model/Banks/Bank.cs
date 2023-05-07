namespace MoneyManager.Write.Application.Model.Banks;

public class Bank : DomainEntity
{
    private readonly string externalId;
    private string name;

    public BankSnapshot Snapshot =>
        new(this.Id, this.externalId, this.name);

    internal Bank(Guid id, string bankIdentifier) : this(id, bankIdentifier, bankIdentifier)
    {
    }

    private Bank(Guid id, string externalId, string name) : base(id)
    {
        this.externalId = externalId;
        this.name = name;
    }

    public static Bank From(BankSnapshot snapshot) =>
        new(snapshot.Id, snapshot.ExternalId, snapshot.Name);

    public Account TrackAccount(Guid id, string accountNumber, decimal balance, DateTime balanceDate) =>
        Account.StartTracking(id, this.Id, accountNumber, balance, balanceDate);

    public void AssignName(string newName) =>
        this.name = newName;
}