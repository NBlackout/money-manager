namespace Write.App.Model.Accounts;

public class Account : DomainEntity
{
    private readonly string externalId;
    private string label;
    private decimal balance;
    private DateOnly balanceDate;

    public AccountSnapshot Snapshot =>
        new(this.Id, this.externalId, this.label, this.balance, this.balanceDate);

    private Account(Guid id, string externalId, string label, decimal balance, DateOnly balanceDate)
        : base(id)
    {
        this.externalId = externalId;
        this.label = label;
        this.balance = balance;
        this.balanceDate = balanceDate;
    }

    public static Account From(AccountSnapshot snapshot) =>
        new(snapshot.Id, snapshot.Number, snapshot.Label, snapshot.Balance, snapshot.BalanceDate);

    public static Account StartTracking(Guid id, string accountNumber, decimal balance, DateOnly balanceDate) =>
        new(id, accountNumber, accountNumber, balance, balanceDate);

    public void Synchronize(decimal updatedBalance, DateOnly updatedBalanceDate)
    {
        this.balance = updatedBalance;
        this.balanceDate = updatedBalanceDate;
    }

    public void AssignLabel(string newLabel) =>
        this.label = newLabel;

    public Transaction AttachTransaction(Guid transactionId, string externalId, decimal amount,
        string transactionLabel, DateOnly date, Category? category) =>
        new(transactionId, this.Id, externalId, amount, transactionLabel, date, category);
}
