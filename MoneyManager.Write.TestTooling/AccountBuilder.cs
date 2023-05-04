namespace MoneyManager.Write.TestTooling;

public record AccountBuilder(Guid Id, Guid BankId, string Number, string Label, decimal Balance, bool Tracked)
{
    public static AccountBuilder For(Guid id) =>
        new(id, Guid.Parse("02D8EB76-49C5-47DC-8973-5484959DA047"), "123ABC", "Account label", 12.34m, true);

    public Account Build()
    {
        return Account.From(
            new AccountSnapshot(this.Id, this.BankId, this.Number, this.Label, this.Balance, this.Tracked)
        );
    }
}