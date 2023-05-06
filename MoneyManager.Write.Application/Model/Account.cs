namespace MoneyManager.Write.Application.Model;

public class Account : DomainEntity
{
    private readonly ExternalId externalId;
    private string label;
    private decimal balance;
    private DateTime balanceDate;
    private bool tracked;

    public AccountSnapshot Snapshot =>
        new(this.Id, this.externalId.BankId, this.externalId.Number, this.label, this.balance, this.balanceDate,
            this.tracked);

    private Account(Guid id, ExternalId externalId, string label, decimal balance, DateTime balanceDate, bool tracked)
        : base(id)
    {
        this.externalId = externalId;
        this.label = label;
        this.balance = balance;
        this.balanceDate = balanceDate;
        this.tracked = tracked;
    }

    public static Account From(AccountSnapshot snapshot) =>
        new(snapshot.Id, new ExternalId(snapshot.BankId, snapshot.Number), snapshot.Label, snapshot.Balance,
            snapshot.BalanceDate, snapshot.Tracked);

    public static Account StartTracking(Guid id, Guid bankId, string accountNumber, decimal balance,
        DateTime balanceDate) =>
        new(id, new ExternalId(bankId, accountNumber), accountNumber, balance, balanceDate, true);

    public void Synchronize(decimal updatedBalance, DateTime updatedBalanceDate)
    {
        if (!this.tracked)
            return;

        this.balance = updatedBalance;
        this.balanceDate = updatedBalanceDate;
    }

    public void StopTracking() =>
        this.tracked = false;

    public void ResumeTracking() =>
        this.tracked = true;

    public void AssignLabel(string newLabel) =>
        this.label = newLabel;
}