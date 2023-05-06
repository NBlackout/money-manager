using MoneyManager.Write.Application.Model.Accounts;

namespace MoneyManager.Write.TestTooling;

public record AccountBuilder(Guid Id, Guid BankId, string Number, string Label, decimal Balance, DateTime BalanceDate,
    bool Tracked)
{
    public static AccountBuilder For(Guid id)
    {
        return new AccountBuilder(id, Guid.Parse("02D8EB76-49C5-47DC-8973-5484959DA047"), "123ABC", "Account label",
            12.34m, DateTime.Parse("2023-05-06"), true);
    }

    public Account Build()
    {
        return Account.From(
            new AccountSnapshot(this.Id, this.BankId, this.Number, this.Label, this.Balance, this.BalanceDate, this.Tracked)
        );
    }
}