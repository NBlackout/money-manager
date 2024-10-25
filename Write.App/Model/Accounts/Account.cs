namespace Write.App.Model.Accounts;

public class Account : DomainEntity<AccountId>
{
    private readonly string externalId;
    private string label;
    private Balance balance;

    public AccountSnapshot Snapshot =>
        new(this.Id, this.externalId, this.label, this.balance.Amount, this.balance.BalanceDate);

    private Account(AccountId id, string externalId, string label, Balance balance) : base(id)
    {
        this.externalId = externalId;
        this.label = label;
        this.balance = balance;
    }

    public static Account From(AccountSnapshot snapshot) =>
        new(snapshot.Id, snapshot.Number, snapshot.Label, new Balance(snapshot.BalanceAmount, snapshot.BalanceDate));

    public static Account StartTracking(AccountId id, string accountNumber, Balance balance) =>
        new(id, accountNumber, accountNumber, balance);

    public void Synchronize(Balance newBalance) =>
        this.balance = newBalance;

    public void AssignLabel(string newLabel) =>
        this.label = newLabel;

    public Transaction AttachTransaction(TransactionId transactionId, string externalId, decimal amount,
        string transactionLabel, DateOnly date, Category? category) =>
        new(transactionId, this.Id, externalId, amount, transactionLabel, date, category);
}