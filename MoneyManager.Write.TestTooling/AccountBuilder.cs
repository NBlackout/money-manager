namespace MoneyManager.Write.TestTooling;

public record AccountBuilder(Guid Id, string BankIdentifier, string Number, string Label, decimal Balance,
    bool Tracked)
{
    public static AccountBuilder For(Guid id) =>
        new(id, "Secure bank", "123ABC", "Account label", 12.34m, true);

    public Account Build()
    {
        return Account.From(
            new AccountSnapshot(this.Id, this.BankIdentifier, this.Number, this.Label, this.Balance, this.Tracked)
        );
    }
}