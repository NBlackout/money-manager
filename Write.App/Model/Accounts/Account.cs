namespace Write.App.Model.Accounts;

public class Account : DomainEntity<AccountId>
{
    private readonly string externalId;
    private Label label;
    private Balance balance;

    public AccountSnapshot Snapshot =>
        new(this.Id, this.externalId, this.label.Value, this.balance.Amount, this.balance.BalanceDate);

    private Account(AccountId id, string externalId, Label label, Balance balance) : base(id)
    {
        this.externalId = externalId;
        this.label = label;
        this.balance = balance;
    }

    public static Account StartTracking(AccountId id, string accountNumber, Balance balance) =>
        new(id, accountNumber, new Label(accountNumber), balance);

    public void Synchronize(Balance newBalance) =>
        this.balance = newBalance;

    public void AssignLabel(Label newLabel) =>
        this.label = newLabel;

    public Transaction AttachTransaction(TransactionId transactionId, string externalId, decimal amount,
        Label transactionLabel, DateOnly date, Category? category) =>
        new(transactionId, this.Id, externalId, amount, transactionLabel, date, category);

    public static Account From(AccountSnapshot snapshot)
    {
        return new Account(snapshot.Id, snapshot.Number, new Label(snapshot.Label),
            new Balance(snapshot.BalanceAmount, snapshot.BalanceDate));
    }
}