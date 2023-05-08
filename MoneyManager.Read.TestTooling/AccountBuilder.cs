using MoneyManager.Shared.Presentation;
using MoneyManager.Write.Application.Model.Accounts;
using MoneyManager.Write.Application.Model.Banks;

namespace MoneyManager.Read.TestTooling;

public record AccountBuilder(Guid Id, Guid BankId, string BankName, string Label, string Number, decimal Balance,
    DateTime BalanceDate, bool Tracked)
{
    public static AccountBuilder For(Guid id)
    {
        return new AccountBuilder(id, Guid.Parse("04150905-B6AD-4D9F-AB2B-191AB9B11A9D"), "The bank", "Account label",
            "6793254", 12.34m, DateTime.Parse("2020-04-12"), true);
    }

    public Account Build()
    {
        return Account.From(new AccountSnapshot(this.Id, this.BankId, this.Number, this.Label, this.Balance,
            this.BalanceDate, this.Tracked));
    }

    public Bank BuildBank() =>
        Bank.From(new BankSnapshot(this.BankId, this.BankId.ToString("N"), this.BankName));

    public AccountSummaryPresentation ToSummary() =>
        new(this.Id, this.BankId, this.BankName, this.Label, this.Balance, this.BalanceDate, this.Tracked);

    public AccountDetailsPresentation ToDetails() =>
        new(this.Id, this.Label, this.Number, this.Balance);
}