using MoneyManager.Write.Application.Model;

namespace MoneyManager.Read.TestTooling;

public record AccountBuilder(Guid Id, string Number, string Label, decimal Balance,
    bool Tracked)
{
    public static AccountBuilder For(Guid id) =>
        new(id, "123ABC", "Account label", 12.34m, true);

    public Account Build()
    {
        return Account.From(
            new AccountSnapshot(this.Id, Guid.Parse("04150905-B6AD-4D9F-AB2B-191AB9B11A9D"), this.Number, this.Label,
                this.Balance, this.Tracked)
        );
    }
}