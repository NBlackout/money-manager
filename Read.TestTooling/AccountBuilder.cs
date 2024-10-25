using System.Diagnostics.CodeAnalysis;
using Shared.Presentation;
using Write.App.Model.Accounts;

namespace Read.TestTooling;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public record AccountBuilder(Guid Id, string Label, string Number, decimal Balance, DateOnly BalanceDate)
{
    public AccountSnapshot ToSnapshot() =>
        new(new AccountId(this.Id), this.Number, this.Label, this.Balance, this.BalanceDate);

    public AccountSummaryPresentation ToSummary() =>
        new(this.Id, this.Label, this.Number, this.Balance);

    public AccountDetailsPresentation ToDetails() =>
        new(this.Id, this.Label, this.Number, this.Balance, this.BalanceDate);
}