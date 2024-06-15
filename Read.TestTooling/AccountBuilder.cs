using Shared.Presentation;
using Write.App.Model.Accounts;
using Write.App.Model.Banks;

namespace Read.TestTooling;

public record AccountBuilder(Guid Id, Guid BankId, string Label, string Number, decimal Balance,
    DateTime BalanceDate, bool Tracked)
{
    public static AccountBuilder For(Guid id)
    {
        return new AccountBuilder(id, Guid.Parse("04150905-B6AD-4D9F-AB2B-191AB9B11A9D"), "Account label",
            "6793254", 12.34m, DateTime.Parse("2020-04-12"), true);
    }

    public Account Build()
    {
        return Account.From(new AccountSnapshot(this.Id, this.BankId, this.Number, this.Label, this.Balance,
            this.BalanceDate, this.Tracked));
    }

    public Bank BuildBank() =>
        Bank.From(new BankSnapshot(this.BankId, this.BankId.ToString("N")));

    public AccountSummaryPresentation ToSummary() =>
        new(this.Id, this.BankId, this.Label, this.Number, this.Balance, this.BalanceDate, this.Tracked);

    public AccountDetailsPresentation ToDetails() =>
        new(this.Id, this.Label, this.Number, this.Balance, this.BalanceDate);
}