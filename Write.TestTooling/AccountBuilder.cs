using Write.App.Model.Accounts;

namespace Write.TestTooling;

public record AccountBuilder(
    Guid Id,
    Guid BankId,
    string Number,
    string Label,
    decimal Balance,
    DateTime BalanceDate,
    bool Tracked)
{
    public static AccountBuilder Create()
    {
        return new AccountBuilder(Guid.NewGuid(), Guid.Parse("02D8EB76-49C5-47DC-8973-5484959DA047"), "123ABC",
            "Account label",
            12.34m, DateTime.Parse("2023-05-06"), true);
    }

    public Account Build() =>
        Account.From(this.ToSnapshot());

    public AccountSnapshot ToSnapshot() =>
        new(this.Id, this.BankId, this.Number, this.Label, this.Balance, this.BalanceDate, this.Tracked);
}