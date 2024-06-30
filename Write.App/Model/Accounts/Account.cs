namespace Write.App.Model.Accounts;

public class Account : DomainEntity
{
    private readonly ExternalId externalId;
    private string label;
    private decimal balance;
    private DateTime balanceDate;

    public AccountSnapshot Snapshot =>
        new(this.Id, this.externalId.BankId, this.externalId.Number, this.label, this.balance, this.balanceDate);

    private Account(Guid id, ExternalId externalId, string label, decimal balance, DateTime balanceDate)
        : base(id)
    {
        this.externalId = externalId;
        this.label = label;
        this.balance = balance;
        this.balanceDate = balanceDate;
    }

    public static Account From(AccountSnapshot snapshot) =>
        new(snapshot.Id, new ExternalId(snapshot.BankId, snapshot.Number), snapshot.Label, snapshot.Balance,
            snapshot.BalanceDate);

    public static Account StartTracking(Guid id, Guid bankId, string accountNumber, decimal balance,
        DateTime balanceDate) =>
        new(id, new ExternalId(bankId, accountNumber), accountNumber, balance, balanceDate);

    public void Synchronize(decimal updatedBalance, DateTime updatedBalanceDate)
    {
        this.balance = updatedBalance;
        this.balanceDate = updatedBalanceDate;
    }

    public void AssignLabel(string newLabel) =>
        this.label = newLabel;

    public Transaction AttachTransaction(Guid transactionId, string transactionExternalId, decimal amount,
        string transactionLabel, DateTime date) =>
        new(transactionId, this.Id, transactionExternalId, amount, transactionLabel, date);
}
