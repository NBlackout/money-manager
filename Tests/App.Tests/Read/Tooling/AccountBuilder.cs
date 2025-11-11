using App.Read.Ports;
using App.Write.Model.Accounts;

namespace App.Tests.Read.Tooling;

public record AccountBuilder(Guid Id, string Label, string Number, decimal Balance, DateOnly BalanceDate)
{
    public AccountSnapshot ToSnapshot() =>
        new(new AccountId(this.Id), this.Number, this.Label, this.Balance, this.BalanceDate);

    public AccountSummaryPresentation ToSummary() =>
        new(this.Id, this.Label, this.Number, this.Balance);

    public AccountDetailsPresentation ToDetails() =>
        new(this.Id, this.Label, this.Number, this.Balance, this.BalanceDate);
}