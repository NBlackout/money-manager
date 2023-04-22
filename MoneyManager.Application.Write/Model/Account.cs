namespace MoneyManager.Application.Write.Model;

public class Account : DomainEntity
{
    private readonly ExternalId externalId;
    private decimal balance;
    private bool tracked;

    public AccountSnapshot Snapshot =>
        new(this.Id, this.externalId.BankIdentifier, this.externalId.Number, this.balance, this.tracked);

    private Account(Guid id, ExternalId externalId, decimal balance, bool tracked) : base(id)
    {
        this.externalId = externalId;
        this.balance = balance;
        this.tracked = tracked;
    }

    public static Account StartTracking(Guid id, string bankIdentifier, string accountNumber, decimal balance) =>
        new(id, new ExternalId(bankIdentifier, accountNumber), balance, true);

    public static Account From(AccountSnapshot snapshot) =>
        new(snapshot.Id, new ExternalId(snapshot.BankIdentifier, snapshot.Number), snapshot.Balance, snapshot.Tracked);

    public void Synchronize(decimal updatedBalance)
    {
        if (!this.tracked)
            return;

        this.balance = updatedBalance;
    }

    public void StopTracking() =>
        this.tracked = false;
}