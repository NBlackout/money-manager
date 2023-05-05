using MoneyManager.Write.Application.Model;

namespace MoneyManager.Read.TestTooling;

public record AccountBuilder(Guid Id, Guid BankId, string Number, string Label, decimal Balance, bool Tracked)
{
    public static AccountBuilder For(Guid id) =>
        new(id, Guid.Parse("04150905-B6AD-4D9F-AB2B-191AB9B11A9D"), "123ABC", "Account label", 12.34m, true);

    public Account Build() =>
        Account.From(new AccountSnapshot(this.Id, this.BankId, this.Number, this.Label, this.Balance, this.Tracked));
}