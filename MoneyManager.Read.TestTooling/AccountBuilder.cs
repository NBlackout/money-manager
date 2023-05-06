using MoneyManager.Write.Application.Model.Accounts;

namespace MoneyManager.Read.TestTooling;

public record AccountBuilder(Guid Id, Guid BankId, string Label, decimal Balance, DateTime BalanceDate, bool Tracked)
{
    public static AccountBuilder For(Guid id)
    {
        return new AccountBuilder(id, Guid.Parse("04150905-B6AD-4D9F-AB2B-191AB9B11A9D"), "Account label", 12.34m,
            DateTime.Parse("2020-04-12"), true);
    }

    public Account Build()
    {
        return Account.From(new AccountSnapshot(this.Id, this.BankId, "123ABC", this.Label, this.Balance,
            this.BalanceDate, this.Tracked));
    }
}