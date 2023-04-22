namespace MoneyManager.Application.Write.Model;

public class Account : DomainEntity
{
    private readonly ExternalId externalId;
    private decimal balance;

    public AccountSnapshot Snapshot =>
        new(this.Id, this.externalId.BankIdentifier, this.externalId.Number, this.balance);

    public Account(Guid id, string bankIdentifier, string number, decimal balance) : base(id)
    {
        this.externalId = new ExternalId(bankIdentifier, number);
        this.balance = balance;
    }

    public static Account From(AccountSnapshot snapshot) =>
        new(snapshot.Id, snapshot.BankIdentifier, snapshot.Number, snapshot.Balance);

    public void Synchronize(decimal balance)
    {
        this.balance = balance;
    }
}